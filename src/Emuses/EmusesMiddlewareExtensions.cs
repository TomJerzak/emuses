using Microsoft.AspNetCore.Builder;

namespace Emuses
{
    public static class EmusesMiddlewareExtensions
    {
        public static IApplicationBuilder UseEmuses(this IApplicationBuilder builder, ISession session)
        {
            return builder.UseMiddleware<EmusesMiddleware>(session);
        }
    }
}
