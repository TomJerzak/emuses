using Microsoft.AspNetCore.Builder;

namespace Emuses
{
    public static class EmusesMiddlewareExtensions
    {
        public static IApplicationBuilder UseEmuses(this IApplicationBuilder builder, int minutes)
        {
            return builder.UseMiddleware<EmusesMiddleware>(minutes);
        }
    }
}
