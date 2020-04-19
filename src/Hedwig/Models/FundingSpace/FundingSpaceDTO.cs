using System.Collections.Generic;

namespace Hedwig.Models
{
	public class FundingSpaceDTO
	{
		public int Id { get; set; }
		public int Capacity { get; set; }
		public int OrganizationId { get; set; }
		public FundingSource Source { get; set; }
		public Age AgeGroup { get; set; }
		public List<FundingTimeAllocation> FundingTimeAllocations { get; set; }
	}
}
