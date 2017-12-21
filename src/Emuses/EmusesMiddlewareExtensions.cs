using Microsoft.AspNetCore.Builder;

namespace Emuses
{
    public static class EmusesMiddlewareExtensions
    {
        public static IApplicationBuilder UseEmuses(this IApplicationBuilder builder, EmusesConfiguration emusesConfiguration)
        {
            return builder.UseMiddleware<EmusesMiddleware>(emusesConfiguration);
        }
    }
}
