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
        private readonly Session _session;
        private readonly string _sessionExpiredPage;
        private readonly List<string> _noSessionAccessPages;

        private EmusesMiddleware()
        {
        }

        public EmusesMiddleware(RequestDelegate next, int sessionTimeout, IStorage storage, string sessionExpiredPage, List<string> noSessionAccessPages)
        {
            _session = new Session(sessionTimeout, storage);
            _sessionExpiredPage = sessionExpiredPage;

            _noSessionAccessPages = noSessionAccessPages ?? new List<string>();
            _noSessionAccessPages.Add(sessionExpiredPage);

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
            return _noSessionAccessPages.Any(anonymousPath => path.ToLower().Contains(anonymousPath.ToLower()));
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
                var sessionBySessionId = _session.GetStorage().GetBySessionId(sessionId);
                _session.Restore(sessionBySessionId.GetSessionId(), sessionBySessionId.GetVersion(), sessionBySessionId.GetExpirationDate(), sessionBySessionId.GetMinutes(), sessionBySessionId.GetStorage());

                _session.Update();
                return _next(context);
            }
            catch (SessionExpiredException)
            {
                context.Response.Redirect(_sessionExpiredPage, true);
                return _next(context);
            }
        }
    }
}
