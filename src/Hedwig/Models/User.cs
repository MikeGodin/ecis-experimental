using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hedwig.Models
{
	public class User
	{
		public int Id { get; set; }

		[Required]
		[StringLength(35)]
		public string FirstName { get; set; }

		[StringLength(35)]
		public string MiddleName { get; set; }

		[Required]
		[StringLength(35)]
		public string LastName { get; set; }

		[StringLength(10)]
		public string Suffix { get; set; }

		public ICollection<UserPermission> Permissions { get; set; }

		[NotMapped]
		public ICollection<Site> Sites { get; set; }
	}
}
