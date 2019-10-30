using System;
using System.Linq;
using System.Collections.Generic;
using Hedwig.Data;
using Hedwig.Models;

namespace HedwigTests.Helpers
{
	public class PermissionHelper
	{
		public static UserSitePermission CreateUserSitePermission(
			HedwigContext context,
			User user = null,
			Site site = null
		)
		{
			user = user ?? UserHelper.CreateUser(context);
			site = site ?? SiteHelper.CreateSite(context);

			var sitePermission = new UserSitePermission
			{
				SiteId = site.Id,
				UserId = user.Id
			};
			context.UserPermissions.Add(sitePermission);
			context.SaveChanges();
			return sitePermission;
		}

		public static UserOrganizationPermission CreateUserOrganizationPermission(
			HedwigContext context,
			User user = null,
			Organization org = null
		)
		{
			user = user ?? UserHelper.CreateUser(context);
			org = org ?? OrganizationHelper.CreateOrganization(context);

			var orgPermission = new UserOrganizationPermission
			{
				OrganizationId = org.Id,
				UserId = user.Id
			};
			context.UserPermissions.Add(orgPermission);
			context.SaveChanges();
			return orgPermission;
		}
	}
}
