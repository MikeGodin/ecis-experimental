using System;
using System.Collections.Generic;

namespace Hedwig.Models
{
	public class Enrollment : SiteOwnedEntity
	{
		public int Id { get; set; }

		public Guid ChildId { get; set; }
		public Child Child { get; set; }

		public DateTime Entry { get; set; }

		public DateTime? Exit { get; set; }

		public ICollection<Funding> Fundings { get; set; }
	}
}
