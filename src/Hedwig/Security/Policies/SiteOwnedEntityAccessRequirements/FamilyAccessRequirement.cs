using System.Threading.Tasks;
using System;

namespace Hedwig.Security
{
    public class FamilyAccessRequirement : IAuthorizationRequirement
    {
        public async Task Authorize(AuthorizationContext context)
        {
            var familyIdString = (string) context.Arguments.ValueFor("id")?.Value;
            var siteIdString = (string) context.Arguments.ValueFor("siteId")?.Value;
            var userIdString = (string) context.User.FindFirst("sub")?.Value;

            // No userId present --> nothing to check
            if (userIdString == null) return;

            var userId = Int32.Parse(userIdString);
            var permissionsHelper = RequestContextAccessor.GetRequestContext(context).PermissionsHelper;

            // siteId is present --> check user can access site
            if (siteIdString != null) {
                var siteId = Int32.Parse(siteIdString);
                if (!await permissionsHelper.UserCanAccessSite(siteId, userId)) {
                    context.ReportError(ErrorMessages.USER_CANNOT_ACCESS_ENTITY("Site"));
                }
            }

            // familyId is present --> check user can access family
            if (familyIdString != null) {
                var familyId = Int32.Parse(familyIdString);
                if(! await permissionsHelper.UserCanAccessFamily(familyId, userId)){
                    context.ReportError(ErrorMessages.USER_CANNOT_ACCESS_ENTITY("Family"));
                }
            }
        }
    }
}