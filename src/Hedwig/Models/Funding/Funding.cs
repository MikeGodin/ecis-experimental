using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations.Schema;
using Hedwig.Validations;
using Hedwig.Validations.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Hedwig.Models
{
	public class Funding : TemporalEntity, IHedwigIdEntity<int>, INonBlockingValidatableObject
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public int EnrollmentId { get; set; }
		public Enrollment Enrollment { get; set; }

		// TODO: Figure out how to access higher-level objects from the request pipeline in validationContext
		// (Getting enrollment from DB does not work if setting Enrollment.AgeGroup and Funding.FundingSpace in the same request)
		// [FundingSpaceAgeGroupMatches]
		// [FundingSpaceOrganizationMatches]
		public FundingSpace FundingSpace { get; protected set; }

		[Required]
		public int? FundingSpaceId { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public FundingSource? Source { get; set; }

		// CDC funding fields
		[RequiredForFundingSource(FundingSource.CDC)]
		public int? FirstReportingPeriodId { get; set; }
		public ReportingPeriod FirstReportingPeriod { get; protected set; }
		public int? LastReportingPeriodId { get; set; }
		[LastReportingPeriodAfterFirst]
		public ReportingPeriod LastReportingPeriod { get; protected set; }

		[NotMapped]
		public List<ValidationError> ValidationErrors { get; set; }
	}
}