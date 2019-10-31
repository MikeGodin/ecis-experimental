using System.Threading.Tasks;
using System;

namespace Hedwig.Security
{
    public class EnrollmentAccessRequirement : IAuthorizationRequirement
    {
        public async Task Authorize(AuthorizationContext context)
        {
            var enrollmentIdString = (string) context.Arguments.ValueFor("id")?.Value;
            var userIdString = (string) context.User.FindFirst("sub")?.Value;

            // no id or userId present --> nothing to check
            if (enrollmentIdString == null || userIdString == null) return;


            var enrollmentId = Int32.Parse(enrollmentIdString);
            var userId = Int32.Parse(userIdString);

            // check user can access enrollment
            var permissionsHelper = RequestContextAccessor.GetRequestContext(context).PermissionsHelper;
            if(! await permissionsHelper.UserCanAccessEnrollment(enrollmentId, userId)) {
                context.ReportError(ErrorMessages.USER_CANNOT_ACCESS_ENTITY("Enrollment"));
            }
        }
    }
}