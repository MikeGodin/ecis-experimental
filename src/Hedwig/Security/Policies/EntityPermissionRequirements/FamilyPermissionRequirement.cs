using System.Threading.Tasks;
using Hedwig.Repositories;
using System;

namespace Hedwig.Security
{
    public class FamilyPermissionRequirement : IAuthorizationRequirement
    {
        public Task Authorize(AuthorizationContext context)
        {
            var familyId = (int?) context.Arguments.ValueFor("id")?.Value;
            var userIdString = context.User.FindFirst("sub")?.Value;

            if (familyId == null || userIdString == null) {
                return Task.CompletedTask;
            }


            var userId = Int32.Parse(userIdString);

            var permissionsHelper = RequestContextAccessor.GetRequestContext(context).PermissionsHelper;
            return permissionsHelper.UserCanAccessFamily(familyId.Value, userId);
        }
    }
}