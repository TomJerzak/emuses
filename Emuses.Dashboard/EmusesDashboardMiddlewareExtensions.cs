using System;
using System.Reflection;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Builder;

namespace Emuses.Dashboard
{
    public static class EmusesDashboardMiddlewareExtensions
    {
        private const string EmbeddedFilesNamespace = "Emuses.Dashboard.bower_components.emuses_dashboard.dist";
        
        public static IApplicationBuilder UseEmusesDashboard(this IApplicationBuilder builder, Action<EmusesDashboardOptions> setupAction)
        {
            var options = new EmusesDashboardOptions();
            setupAction?.Invoke(options);

            builder.UseMiddleware<EmusesDashboardMiddleware>(options);
            builder.UseFileServer(new FileServerOptions
            {
                RequestPath = $"/{options.RoutePrefix}",
                FileProvider = new EmbeddedFileProvider(typeof(EmusesDashboardMiddlewareExtensions).GetTypeInfo().Assembly, EmbeddedFilesNamespace),
                EnableDefaultFiles = true //serve index.html at /{ options.RoutePrefix }/
            });

            return builder;
        }
    }
}
