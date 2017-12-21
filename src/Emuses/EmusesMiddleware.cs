using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emuses.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Emuses
{
    public class EmusesMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly EmusesConfiguration _configuration;

        private EmusesMiddleware()
        {
        }

        public EmusesMiddleware(RequestDelegate next, EmusesConfiguration emusesConfiguration)
        {
            _configuration = emusesConfiguration;
            if (_configuration.NoSessionAccessPages == null)
                _configuration.NoSessionAccessPages = new List<string>();

            _configuration.NoSessionAccessPages.Add(_configuration.SessionExpiredPage);

            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            if (IsAnonymousAccessPath(context.Request.Path.Value))
                return _next(context);

            context.Request.Cookies.TryGetValue("Emuses.SessionId", out var sessionId);

            return string.IsNullOrEmpty(sessionId) ? RedirectToSignIn(context) : UpdateSession(context, sessionId);
        }

        private bool IsAnonymousAccessPath(string path)
        {
            return _configuration.NoSessionAccessPages.Any(anonymousPath => path.ToLower().Contains(anonymousPath.ToLower()));
        }

        private Task RedirectToSignIn(HttpContext context)
        {
            context.Response.Redirect("/Account/Login", true);
            return _next(context);
        }

        private Task UpdateSession(HttpContext context, string sessionId)
        {
            try
            {
                var session = _configuration.Storage.GetBySessionId(sessionId);

                session.Update(sessionId);

                return _next(context);
            }
            catch (SessionExpiredException)
            {
                context.Response.Redirect(_configuration.SessionExpiredPage, true);
                return _next(context);
            }
        }
    }
}
