using System.Threading.Tasks;
using System;

namespace Hedwig.Security
{
    public class FamilyDeterminationAccessRequirement : IAuthorizationRequirement
    {
        public async Task Authorize(AuthorizationContext context)
        {
            var familyDeterminationIdString = (string) context.Arguments.ValueFor("id")?.Value;
            var familyIdString = (string) context.Arguments.ValueFor("familyId")?.Value;
            var userIdString = (string) context.User.FindFirst("sub")?.Value;

            // no userId is present --> nothing to check 
            if (userIdString == null) return;

            var userId = Int32.Parse(userIdString);
            var permissionsHelper = RequestContextAccessor.GetRequestContext(context).PermissionsHelper;

            // familyId is present --> check user can access family
            if (familyIdString != null) {
                var familyId = Int32.Parse(familyIdString);
                if(! await permissionsHelper.UserCanAccessFamily(familyId, userId)) {
                    context.ReportError(ErrorMessages.USER_CANNOT_ACCESS_ENTITY("Family"));
                }
                return;
            }

            // familyId is not present --> check user can access family determination
            // by checking that they can access associated family
            var familyDeterminationId = Int32.Parse(familyDeterminationIdString);
            if(! await permissionsHelper.UserCanAccessFamilyForFamilyDetermination(familyDeterminationId, userId)) {
                context.ReportError(ErrorMessages.USER_CANNOT_ACCESS_ENTITY("FamilyDetermination"));
            }
        }
    }
}