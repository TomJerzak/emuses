using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Emuses.Dashboard
{
    public class EmusesDashboardMiddlewareToRemove
    {
        /*private readonly RequestDelegate _next;
        private readonly EmusesDashboardOptions _options;

        public EmusesDashboardMiddlewareToRemove(RequestDelegate next, EmusesDashboardOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!RequestingEmusesDashboardIndex(httpContext.Request))
            {
                await _next(httpContext);
                return;
            }

            RespondWithIndexHtml(httpContext.Response);
        }

        private bool RequestingEmusesDashboardIndex(HttpRequest request)
        {
            return request.Method == "GET" && request.Path == $"/{_options.RoutePrefix}/";
        }

        private async void RespondWithIndexHtml(HttpResponse response)
        {
            response.StatusCode = 200;
            response.ContentType = "text/html";

            using (var rawStream = _options.IndexStream())
            {
                var htmlBuilder = new StringBuilder(new StreamReader(rawStream).ReadToEnd());
                foreach (var entry in _options.IndexSettings.ToTemplateParameters())
                {
                    htmlBuilder.Replace(entry.Key, entry.Value);
                }

                await response.WriteAsync(htmlBuilder.ToString(), Encoding.UTF8);
            }
        }*/
    }
}
