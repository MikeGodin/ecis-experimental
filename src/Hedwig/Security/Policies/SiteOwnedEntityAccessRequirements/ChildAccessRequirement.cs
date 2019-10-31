using System.Threading.Tasks;
using System;

namespace Hedwig.Security
{
    public class ChildAccessRequirement : IAuthorizationRequirement
    {
        public Task Authorize(AuthorizationContext context)
        {
            var childIdString = (string) context.Arguments.ValueFor("id")?.Value;
            var userIdString = (string) context.User.FindFirst("sub")?.Value;

            if (childIdString == null || userIdString == null) {
                return Task.CompletedTask;
            }


            var childId = Guid.Parse(childIdString);
            var userId = Int32.Parse(userIdString);

            var permissionsHelper = RequestContextAccessor.GetRequestContext(context).PermissionsHelper;
            return permissionsHelper.UserCanAccessChild(childId, userId);
        }
    }
}