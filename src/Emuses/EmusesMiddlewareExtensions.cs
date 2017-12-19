using Microsoft.AspNetCore.Builder;

namespace Emuses
{
    public static class EmusesMiddlewareExtensions
    {
        public static IApplicationBuilder UseEmuses(this IApplicationBuilder builder, int sessionTimeout, IStorage storage)
        {
            return builder.UseMiddleware<EmusesMiddleware>(sessionTimeout, storage);
        }
    }
}
