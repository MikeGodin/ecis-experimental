using System.Threading.Tasks;
using System;

namespace Hedwig.Security
{
    public class ChildAccessRequirement : IAuthorizationRequirement
    {
        public async Task Authorize(AuthorizationContext context)
        {
            var childIdString = (string) context.Arguments.ValueFor("id")?.Value;
            var userIdString = (string) context.User.FindFirst("sub")?.Value;

            // No id or userId present --> nothing to check
            if (childIdString == null || userIdString == null) return;


            var childId = Guid.Parse(childIdString);
            var userId = Int32.Parse(userIdString);

            // check user can access child
            var permissionsHelper = RequestContextAccessor.GetRequestContext(context).PermissionsHelper;
            if(! await permissionsHelper.UserCanAccessChild(childId, userId)) {
                context.ReportError(ErrorMessages.USER_CANNOT_ACCESS_ENTITY("Child"));
            }
        }
    }
}