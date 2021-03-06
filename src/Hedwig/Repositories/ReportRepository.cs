using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Hedwig.Models;
using Hedwig.Data;

namespace Hedwig.Repositories
{
	public class ReportRepository : HedwigRepository, IReportRepository
	{
		public ReportRepository(HedwigContext context) : base(context) { }

		public void UpdateReport(CdcReport report, CdcReportDTO reportDTO)
		{
			// Should this be _context.Update(report) as in EnrollmentRepository ?
			// depends on if we include sub-objets on report that should _not_ be updated
			UpdateHedwigIdEntityWithNavigationProperties<CdcReport, CdcReportDTO, int>(report, reportDTO);
		}
		public List<CdcReport> GetReportsForOrganization(int orgId)
		{
			return _context.Reports
			.OfType<CdcReport>()
			.Where(r => r.OrganizationId == orgId)
			.Include(report => report.ReportingPeriod)
			.Include(report => report.TimeSplitUtilizations)
			.ToList();
		}

		public async Task<CdcReport> GetReportForOrganizationAsync(int id, int orgId, string[] include = null)
		{
			include = include ?? new string[] { };
			var include_orgs = include.Contains(INCLUDE_ORGANIZATIONS);
			var include_sites = include.Contains(INCLUDE_SITES);
			var include_enrollments = include.Contains(INCLUDE_ENROLLMENTS);
			var include_children = include.Contains(INCLUDE_CHILD);
			var include_funding_spaces = include.Contains(INCLUDE_FUNDING_SPACES);

			IQueryable<CdcReport> reportQuery = _context.Reports
			.OfType<CdcReport>()
			.Where(report => report.Id == id && report.OrganizationId == orgId)
			.Include(report => report.ReportingPeriod)
			.Include(report => report.TimeSplitUtilizations);

			if (include.Contains(INCLUDE_ORGANIZATIONS))
			{
				reportQuery = reportQuery.Include(report => report.Organization);
			}

			if (include.Contains(INCLUDE_SITES))
			{
				reportQuery = reportQuery
					.Include(report => report.Organization)
					.ThenInclude(organization => organization.Sites);
			}

			if (include.Contains(INCLUDE_FUNDING_SPACES))
			{
				reportQuery = reportQuery
					.Include(report => report.Organization)
						.ThenInclude(organization => organization.FundingSpaces)
							.ThenInclude(fundingSpace => fundingSpace.TimeSplit)
					.Include(report => report.Organization)
						.ThenInclude(organization => organization.FundingSpaces)
							.ThenInclude(fundingSpace => fundingSpace.TimeSplitUtilizations)
								.ThenInclude(timeSplitUtilization => timeSplitUtilization.ReportingPeriod)
					.Include(report => report.Organization)
						.ThenInclude(organization => organization.FundingSpaces)
							.ThenInclude(fundingSpace => fundingSpace.TimeSplitUtilizations)
								.ThenInclude(util => util.Report);
			}

			var reportResult = await reportQuery.FirstOrDefaultAsync();

			// Optionally manually insert time-versioned enrollment records
			if (reportResult != null && include.Contains(INCLUDE_ENROLLMENTS))
			{
				var enrollments = GetEnrollmentsForReport(reportResult);
				// Optionally manually insert time-versioned child records
				if (include.Contains(INCLUDE_CHILD))
				{
					var childIds = enrollments.Select(enrollment => enrollment.ChildId);
					var children = (reportResult.SubmittedAt.HasValue ? _context.Children.AsOf(reportResult.SubmittedAt.Value) : _context.Children)
					.Include(child => child.C4KCertificates)
					.Where(child => childIds.Contains(child.Id))
					.ToDictionary(child => child.Id);

					// Add children to enrollments
					enrollments.ForEach(enrollment =>
					{
						enrollment.Child = children[enrollment.ChildId];
					});
				}

				// Add enrollments to report
				reportResult.Enrollments = enrollments;
			}

			return reportResult;
		}

		public List<Enrollment> GetEnrollmentsForReport(CdcReport report)
		{
			var sites = report.Organization != null && report.Organization.Sites != null
			? report.Organization.Sites.ToList()
			: _context.Sites.Where(s => s.OrganizationId == report.OrganizationId).ToList();
			var siteIds = sites.Select(site => site.Id);

			// Potential optimization to fetch only the enrollments that are funded during the reporting period
			var enrollments = (report.SubmittedAt.HasValue ? _context.Enrollments.AsOf(report.SubmittedAt.Value) : _context.Enrollments)
			.Where(enrollment => siteIds.Contains(enrollment.SiteId))
			.ToList();

			var enrollmentIds = enrollments.Select(enrollment => enrollment.Id);
			var fundings = (report.SubmittedAt.HasValue ? _context.Fundings.AsOf(report.SubmittedAt.Value) : _context.Fundings)
			.Include(funding => funding.FirstReportingPeriod)
			.Include(funding => funding.LastReportingPeriod)
			.Include(funding => funding.FundingSpace)
				.ThenInclude(fundingSpace => fundingSpace.TimeSplit)
			.Where(funding => enrollmentIds.Contains(funding.EnrollmentId))
			.Where(funding => funding.FirstReportingPeriod.PeriodStart <= report.ReportingPeriod.PeriodStart)
			.Where(funding => funding.LastReportingPeriod == null || funding.LastReportingPeriod.PeriodEnd >= report.ReportingPeriod.PeriodEnd)
			.ToList();

			// Add fundings to enrollments
			enrollments.ForEach(enrollment =>
			{
				enrollment.Fundings = fundings.Where(funding => funding.EnrollmentId == enrollment.Id).ToList();
			});

			// Filter for funded enrollments (only funded enrollments are included in report)
			return enrollments.Where(enrollment => enrollment.Fundings.Any(funding => funding.Source == report.Type)).ToList();
		}

		public CdcReport GetMostRecentSubmittedCdcReportForOrganization(int orgId)
		{
			return _context.Reports
			.OfType<CdcReport>()
			.Include(report => report.ReportingPeriod)
			.Where(report => report.SubmittedAt != null)
			.Where(report => report.OrganizationId == orgId)
			.OrderByDescending(report => report.ReportingPeriod.Period)
			.FirstOrDefault();
		}

		public IEnumerable<CdcReport> GetReportsForOrganizationByFiscalYear(int orgId, DateTime periodDate)
		{
			var month = periodDate.Month;
			var fiscalYearStart = periodDate.Year;
			if (month < 7)
			{
				--fiscalYearStart;
			}
			var start = new DateTime(fiscalYearStart, 7, 1);
			var end = new DateTime(fiscalYearStart + 1, 6, 30);
			return _context
				.Reports
				.OfType<CdcReport>()
				.Where(r => r.OrganizationId == orgId)
				.Include(r => r.TimeSplitUtilizations)
				.ThenInclude(u => u.FundingSpace)
				.ThenInclude(f => f.TimeSplit)
				.Include(r => r.ReportingPeriod)
				.Where(
					report =>
						report.ReportingPeriod.Period.CompareTo(start) >= 0 &&
						report.ReportingPeriod.Period.CompareTo(end) <= 0
				)
				.ToList();
		}

		public void AddReport(Report report)
		{
			_context.Add(report);
		}

		public bool HasCdcReportForOrganizationAndReportingPeriod(int orgId, ReportingPeriod period)
		{
			return _context.Reports
			.OfType<CdcReport>()
			.Where(report => report.OrganizationId == orgId)
			.Where(report => report.ReportingPeriodId == period.Id)
			.FirstOrDefault() != null;
		}
	}

	public interface IReportRepository : IHedwigRepository
	{
		void UpdateReport(CdcReport report, CdcReportDTO reportDTO);
		List<CdcReport> GetReportsForOrganization(int orgId);
		Task<CdcReport> GetReportForOrganizationAsync(int id, int orgId, string[] include);
		List<Enrollment> GetEnrollmentsForReport(CdcReport report);
		CdcReport GetMostRecentSubmittedCdcReportForOrganization(int orgId);
		IEnumerable<CdcReport> GetReportsForOrganizationByFiscalYear(int orgId, DateTime periodDate);
		void AddReport(Report report);
		bool HasCdcReportForOrganizationAndReportingPeriod(int orgId, ReportingPeriod period);
	}
}
