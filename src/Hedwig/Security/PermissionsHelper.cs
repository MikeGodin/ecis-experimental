using System.Threading.Tasks;
using Hedwig.Repositories;
using System.Linq;

namespace Hedwig.Security
{
    public class PermissionsHelper
    {
        private readonly IPermissionRepository _permissions;

        public PermissionsHelper(IPermissionRepository permissions) => _permissions = permissions;


        public async Task<bool> UserCanAccessFamily(int familyId, int userId) 
        {
            var familyPermission = _permissions.GetFamilyPermissionByFamilyIdAsync(familyId);
            var userSitePermissions = _permissions.GetSitePermissionsByUserId(userId, includeOrganizations: true);
            var userOrganizationPermissions = _permissions.GetOrganizationPermissionsByUserId(userId);

            await Task.WhenAll(familyPermission);

            // get all organization ids that this user can read from
            var organizationIds = userSitePermissions.Result.Select(sp => sp.Site.OrganizationId)
                .Concat(userOrganizationPermissions.Result.Select(op => op.OrganizationId))
                .ToList(); 

            return organizationIds.Contains(familyPermission.Result.OrganizationId);
        }
    }
}