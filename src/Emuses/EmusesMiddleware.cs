using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emuses.Exceptions;
using Emuses.Services;
using Microsoft.AspNetCore.Http;

namespace Emuses
{
    public class EmusesMiddleware
    {
        private const string CacheControl = "Cache-Control";
        private const string NoCache = "no-cache";
        private const string SessionCookieName = "Emuses.Session";
        private const string LoginPage = "/Account/Login";
        private const string InvokeMethod = "Invoke ";
        private readonly RequestDelegate _next;
        private readonly EmusesOptions _configuration;

        public EmusesMiddleware(RequestDelegate next, EmusesOptions emusesConfiguration)
        {
            _configuration = emusesConfiguration;

            if (_configuration.NoSessionAccessPages == null)
                _configuration.NoSessionAccessPages = new List<string>();
            _configuration.NoSessionAccessPages.Add(_configuration.SessionExpiredPage);

            if (_configuration.LoginPage == null)
                _configuration.LoginPage = LoginPage;

            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            if (_configuration.Logger)
                new LoggerService(nameof(EmusesMiddleware)).PrintLog(InvokeMethod + context.Request.Path);

            if (!_configuration.DisableNoCache)
                AddNoCacheHeader(context);

            if (IsAnonymousAccessPath(context.Request.Path.Value))
                return _next(context);

            context.Request.Cookies.TryGetValue(SessionCookieName, out var sessionId);

            if (string.IsNullOrEmpty(sessionId))
                return RedirectToSignIn(context);
            else
                return UpdateSession(context, sessionId);
        }

        private static void AddNoCacheHeader(HttpContext context)
        {            
            context.Response.Headers[CacheControl] = NoCache;
        }

        private bool IsAnonymousAccessPath(string path)
        {
            return _configuration.NoSessionAccessPages.Any(anonymousPath =>
                path.ToLower().Contains(anonymousPath.ToLower()));
        }

        private Task RedirectToSignIn(HttpContext context)
        {
            context.Response.Redirect(_configuration.LoginPage, true);
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
