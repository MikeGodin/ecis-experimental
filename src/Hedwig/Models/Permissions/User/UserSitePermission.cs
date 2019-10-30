namespace Hedwig.Models
{
	public class UserSitePermission : UserPermission
	{
		public int SiteId { get; set; }
		public Site Site { get; set; }
	}
}
