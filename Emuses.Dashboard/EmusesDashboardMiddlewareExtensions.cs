using Microsoft.AspNetCore.Builder;

namespace Emuses.Dashboard
{
    public static class EmusesDashboardMiddlewareExtensions
    {
        public static IApplicationBuilder UseEmusesDashboard(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EmusesDashboardMiddleware>();
        }
    }
}
