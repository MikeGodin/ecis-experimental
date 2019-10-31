using System.Threading.Tasks;
using Hedwig.Repositories;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace Hedwig.Security
{
    public class PermissionsHelper
    {
        private readonly IPermissionRepository _permissions;
        private readonly IFamilyRepository _families;
        private readonly IChildRepository _children;
        private readonly IFamilyDeterminationRepository _familyDeterminations;
        private readonly IEnrollmentRepository _enrollments;
        private readonly IFundingRepository _fundings;

        public PermissionsHelper(
            IPermissionRepository permissions,
            IFamilyRepository families,
            IChildRepository children,
            IFamilyDeterminationRepository familyDeterminations,
            IEnrollmentRepository enrollments,
            IFundingRepository fundings
         )
         {
            _permissions = permissions;
            _families = families;
            _children = children;
            _familyDeterminations = familyDeterminations;
            _enrollments = enrollments;
            _fundings = fundings;
         } 


        public async Task<bool> UserCanAccessSite(int siteId, int userId) {
            var organizationPerms = _permissions.GetOrganizationPermissionsByUserId(userId, includeSites: true);
            var sitePerms = _permissions.GetSitePermissionsByUserId(userId, includeOrganizations: true);

            await Task.WhenAll(sitePerms, organizationPerms);

            // If user has organization permissions, check the organization contains the provided site 
            if(organizationPerms.Result.Count > 0) {
                var siteIds = organizationPerms.Result
                    .SelectMany(op => op.Organization.Sites)
                    .Select(s => s.Id)
                    .ToList();
                if (siteIds.Contains(siteId)) return true;
            }

            // If user has site permissions, check they contain the provided site
            if(sitePerms.Result.Count > 0) {
                var siteIds = sitePerms.Result
                    .Select(sp => sp.SiteId);

                if (siteIds.Contains(siteId)) return true;
            }

            // No permissions for user means no access
            return false;
        }
        public async Task<bool> UserCanAccessChild(Guid childId, int userId)
        {
            var child = await _children.GetChildByIdAsync(childId);
            if(child == null) return false;
            if(child.SiteId == 0) return false;
            return await UserCanAccessSite(child.SiteId, userId);
        }
    }
}