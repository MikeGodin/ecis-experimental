using System.Threading.Tasks;
using System;

namespace Hedwig.Security
{
    public class FamilyAccessRequirement : IAuthorizationRequirement
    {
        public async Task Authorize(AuthorizationContext context)
        {
            var familyIdString = (string) context.Arguments.ValueFor("id")?.Value;
            var userIdString = (string) context.User.FindFirst("sub")?.Value;

            // No id or userId present --> nothing to check
            if (familyIdString == null || userIdString == null) return;


            var familyId = Int32.Parse(familyIdString);
            var userId = Int32.Parse(userIdString);

            // check user can access family
            var permissionsHelper = RequestContextAccessor.GetRequestContext(context).PermissionsHelper;
            if(! await permissionsHelper.UserCanAccessFamily(familyId, userId)){
                context.ReportError(ErrorMessages.USER_CANNOT_ACCESS_ENTITY("Family"));
            }
        }
    }
}