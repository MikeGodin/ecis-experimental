namespace Hedwig.Models
{
	public class UserOrganizationPermission : UserPermission
	{
		public int OrganizationId { get; set; }
		public Organization Organization { get; set; }
	}
}