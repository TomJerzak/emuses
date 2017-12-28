using System;
using Microsoft.AspNetCore.Builder;

namespace Emuses
{
    public static class EmusesMiddlewareExtensions
    {
        public static IApplicationBuilder UseEmuses(this IApplicationBuilder builder, Action<EmusesOptions> setupAction)
        {
            var options = new EmusesOptions();
            setupAction?.Invoke(options);

            return builder.UseMiddleware<EmusesMiddleware>(options);
        }
    }
}
