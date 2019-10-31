using System.Threading.Tasks;
using System;

namespace Hedwig.Security
{
    public class FundingAccessRequirement : IAuthorizationRequirement
    {
        public async Task Authorize(AuthorizationContext context)
        {
            var fundingIdString = (string) context.Arguments.ValueFor("id")?.Value;
            var enrollmentIdString = (string) context.Arguments.ValueFor("enrollmentId")?.Value;
            var userIdString = (string) context.User.FindFirst("sub")?.Value;

            // no userId is present --> nothing to check 
            if (userIdString == null) return;

            var userId = Int32.Parse(userIdString);
            var permissionsHelper = RequestContextAccessor.GetRequestContext(context).PermissionsHelper;

            // enrollmentId is present --> check user can access enrollment
            if (enrollmentIdString != null) {
                var enrollmentId = Int32.Parse(enrollmentIdString);
                if(! await permissionsHelper.UserCanAccessFamily(enrollmentId, userId)) {
                    context.ReportError(ErrorMessages.USER_CANNOT_ACCESS_ENTITY("Enrollment"));
                }
                return;
            }

            // enrollmentId is not present --> check user can access funding
            // by checking that they can access associated enrollment
            var fundingId = Int32.Parse(fundingIdString);
            if(! await permissionsHelper.UserCanAccessEnrollmentForFunding(fundingId, userId)) {
                context.ReportError(ErrorMessages.USER_CANNOT_ACCESS_ENTITY("Funding"));
            }
        }
    }
}