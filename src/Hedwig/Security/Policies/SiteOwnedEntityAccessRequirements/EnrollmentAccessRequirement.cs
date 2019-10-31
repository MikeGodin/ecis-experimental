using System.Threading.Tasks;
using System;

namespace Hedwig.Security
{
    public class EnrollmentAccessRequirement : IAuthorizationRequirement
    {
        public async Task Authorize(AuthorizationContext context)
        {
            var enrollmentIdString = (string) context.Arguments.ValueFor("id")?.Value;
            var siteIdString = (string) context.Arguments.ValueFor("siteId")?.Value;
            var userIdString = (string) context.User.FindFirst("sub")?.Value;

            // no userId present --> nothing to check
            if (userIdString == null) return;

            var userId = Int32.Parse(userIdString);
            var permissionsHelper = RequestContextAccessor.GetRequestContext(context).PermissionsHelper;

            // siteId is present --> check user can access site
            if (siteIdString != null) {
                var siteId = Int32.Parse(siteIdString);
                if(! await permissionsHelper.UserCanAccessSite(siteId, userId)) {
                    context.ReportError(ErrorMessages.USER_CANNOT_ACCESS_ENTITY("Site"));
                }
            }

            // enrollmentId is present --> check user can access enrollment
            if (enrollmentIdString != null) {
                var enrollmentId = Int32.Parse(enrollmentIdString);
                if(! await permissionsHelper.UserCanAccessEnrollment(enrollmentId, userId)) {
                    context.ReportError(ErrorMessages.USER_CANNOT_ACCESS_ENTITY("Enrollment"));
                }
            }
        }
    }
}