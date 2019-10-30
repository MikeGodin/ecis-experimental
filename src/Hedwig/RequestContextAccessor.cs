using Hedwig.Schema;
using GraphQL.Types;
using Hedwig.Security;
namespace Hedwig
{
    public static class RequestContextAccessor
    {
        public static RequestContext GetRequestContext(AuthorizationContext context)
        {
            return context.UserContext as RequestContext;
        }


        public static RequestContext GetRequestContext<T>(ResolveFieldContext<T> context)
        {
            return context.UserContext as RequestContext;
        }
    }
}