using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Emuses.Dashboard
{
    public class EmusesDashboardMiddleware
    {
        private readonly RequestDelegate _next;

        public EmusesDashboardMiddleware(RequestDelegate next)
        {
            _next = next;
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
            // return (request.Method == "GET" && request.Path == $"/{_options.RoutePrefix}/");
            return request.Method == "GET" && request.Path == "/test/";
        }

        private async void RespondWithIndexHtml(HttpResponse response)
        {
            response.StatusCode = 200;
            response.ContentType = "text/html";

            /*using (var rawStream = _options.IndexStream())
            {
                var rawText = new StreamReader(rawStream).ReadToEnd();
                var htmlBuilder = new StringBuilder(rawText);
                foreach (var entry in _options.IndexSettings.ToTemplateParameters())
                {
                    htmlBuilder.Replace(entry.Key, entry.Value);
                }

                await response.WriteAsync(htmlBuilder.ToString(), Encoding.UTF8);
            }*/

            await response.WriteAsync("<html><body>test</body></html>", Encoding.UTF8);
        }
    }
}
