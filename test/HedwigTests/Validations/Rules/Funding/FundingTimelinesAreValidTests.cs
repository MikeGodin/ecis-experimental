using Xunit;
using Hedwig.Models;
using System.Collections.Generic;
using Hedwig.Validations.Rules;
using Hedwig.Validations;
using Moq;
using System;
using Hedwig.Repositories;
using HedwigTests.Fixtures;
using HedwigTests.Helpers;

namespace HedwigTests.Validations.Rules
{
	public class FundingTimelinesAreValidTests
	{
		[Theory]
		[InlineData(2010, 2011, 2012, 2013, false)]
		[InlineData(2010, 2011, 2012, null, false)]
		[InlineData(2010, 2011, 2011, 2013, true)]
		[InlineData(2010, 2011, 2011, null, true)]
		[InlineData(2010, 2011, 2010, 2013, true)]
		[InlineData(2010, 2011, 2009, 2013, true)]
		[InlineData(2010, 2011, 2010, null, true)]
		[InlineData(2010, 2011, 2009, null, true)]
		[InlineData(2010, null, 2012, 2013, true)]
		public void Execute_ReturnsError_IfAnyFundingIsNotValid(
			int? f1FirstReportingPeriodStartYear,
			int? f1LastReportingPeriodEndYear,
			int? f2FirstReportingPeriodStartYear,
			int? f2LastReportingPeriodEndYear,
			bool doesError
		)
		{
			// if
			var f1FirstReportingPeriod = f1FirstReportingPeriodStartYear != null ?
			new ReportingPeriod
			{
				PeriodStart = new DateTime((int)f1FirstReportingPeriodStartYear, 1, 1)
			} :
			null;
			var f1LastReportingPeriod = f1LastReportingPeriodEndYear != null ?
			new ReportingPeriod
			{
				PeriodEnd = new DateTime((int)f1LastReportingPeriodEndYear, 1, 1)
			} :
			null;
			var funding1 = new Funding
			{
				Id = 1,
				FirstReportingPeriodId = f1FirstReportingPeriod.Id,
				Source = FundingSource.CDC
			};
			funding1.GetType().GetProperty(nameof(funding1.FirstReportingPeriod)).SetValue(funding1, f1FirstReportingPeriod);
			funding1.GetType().GetProperty(nameof(funding1.LastReportingPeriod)).SetValue(funding1, f1LastReportingPeriod);

			var f2FirstReportingPeriod = f2FirstReportingPeriodStartYear != null ?
			new ReportingPeriod
			{
				PeriodStart = new DateTime((int)f2FirstReportingPeriodStartYear, 1, 1)
			} :
			null;
			var f2LastReportingPeriod = f2LastReportingPeriodEndYear != null ?
			new ReportingPeriod
			{
				PeriodEnd = new DateTime((int)f2LastReportingPeriodEndYear, 1, 1)
			} :
			null;
			var funding2 = new Funding
			{
				Id = 2,
				FirstReportingPeriodId = f2FirstReportingPeriod.Id,
				Source = FundingSource.CDC
			};
			funding2.GetType().GetProperty(nameof(funding2.FirstReportingPeriod)).SetValue(funding2, f2FirstReportingPeriod);
			funding2.GetType().GetProperty(nameof(funding2.LastReportingPeriod)).SetValue(funding2, f2LastReportingPeriod);

			var child = new Child();
			var enrollment = new Enrollment
			{
				Child = child,
				ChildId = child.Id,
				Fundings = new List<Funding> { funding1, funding2 },
			};
			funding1.Enrollment = enrollment;
			funding2.Enrollment = enrollment;

			var fundingRule = new Mock<IValidationRule<Funding>>();
			var _serviceProvider = new Mock<IServiceProvider>();
			_serviceProvider.Setup(sp => sp.GetService(typeof(IEnumerable<IValidationRule<Funding>>)))
			.Returns(new List<IValidationRule<Funding>> { fundingRule.Object });

			var _fundings = new Mock<IFundingRepository>();
			_fundings.Setup(f => f.GetFundingsByChildId(It.IsAny<Guid>())).Returns(new List<Funding> { funding1, funding2 });
			var _enrollments = new Mock<IEnrollmentRepository>();

			// when
			var rule = new FundingTimelinesAreValid(_fundings.Object, _enrollments.Object);
			var result = rule.Execute(funding1, new NonBlockingValidationContext());

			// Then
			Assert.Equal(doesError, result != null);
		}


		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void Execute_DoesNotAddEnrollmentToFunding(bool fundingHasEnrollmentReference)
		{
			Funding funding;
			using (var context = new TestHedwigContextProvider().Context)
			{
				funding = FundingHelper.CreateFunding(context);
			}

			if (!fundingHasEnrollmentReference) funding.Enrollment = null;

			using (var context = new TestHedwigContextProvider().Context)
			{
				context.Attach(funding);

				var _serviceProvider = new Mock<IServiceProvider>();
				var _validator = new NonBlockingValidator(_serviceProvider.Object);
				var _fundings = new FundingRepository(context);
				var _enrollments = new EnrollmentRepository(context);

				// when
				var rule = new FundingTimelinesAreValid(_fundings, _enrollments);
				rule.Execute(funding, new NonBlockingValidationContext());

				// then
				Assert.Equal(fundingHasEnrollmentReference, funding.Enrollment != null);
			}
		}
	}
}
