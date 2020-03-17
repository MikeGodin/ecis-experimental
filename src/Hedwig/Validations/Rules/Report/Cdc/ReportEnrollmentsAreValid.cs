using Hedwig.Models;
using Hedwig.Repositories;
using System.Linq;

namespace Hedwig.Validations.Rules
{
	public class ReportEnrollmentsAreValid : SubObjectIsValid, IValidationRule<CdcReport>
	{
		readonly IReportRepository _reports;

		public ReportEnrollmentsAreValid(
			INonBlockingValidator validator,
			IReportRepository reports
		) : base(validator)
		{
			_reports = reports;
		}

		public ValidationError Execute(CdcReport report, NonBlockingValidationContext context)
		{

			var enrollments = report.Enrollments ?? _reports.GetEnrollmentsForReport(report);
			ValidateSubObject(enrollments, report);
			if (enrollments.Any(e => e.ValidationErrors.Count > 0))
			{
				return new ValidationError(
					field: "Enrollments",
					message: "Enrollments have validation errors",
					isSubObjectValidation: true
				);
			}

			return null;
		}
	}
}