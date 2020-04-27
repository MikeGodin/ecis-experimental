using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Hedwig.Validations;
using System.ComponentModel.DataAnnotations;

namespace Hedwig.Models
{
	public abstract class Report : IHedwigIdEntity<int>, INonBlockingValidatableObject
	{
		[Required]
		public int Id { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public FundingSource Type { get; protected set; }

		public int ReportingPeriodId { get; set; }
		[JsonProperty("reportingPeriod")]
		public ReportingPeriod ReportingPeriod { get; protected set; }

		public DateTime? SubmittedAt { get; set; }

		[NotMapped]
		public List<Enrollment> Enrollments { get; set; }
		[NotMapped]
		public List<ValidationError> ValidationErrors { get; set; }
	}
}
