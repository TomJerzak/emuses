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

        private EmusesMiddleware()
        {
        }

        public EmusesMiddleware(RequestDelegate next, int sessionTimeout, IStorage storage)
        {
            _session = new Session(sessionTimeout, storage);
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            if(IsAnonymousAccessPath(context.Request.Path.Value))            
                return _next(context);

            context.Request.Cookies.TryGetValue("Emuses.SessionId", out var sessionId);

            return string.IsNullOrEmpty(sessionId) ? OpenSession(context) : UpdateSession(context, sessionId);
        }

        private static bool IsAnonymousAccessPath(string path)
        {
            var anonymousPaths = new List<string> {"/Account/Login", "/Account/Logout", "/Account/Expired"};

            return anonymousPaths.Any(anonymousPath => path.ToLower().Contains(anonymousPath.ToLower()));
        }

        private Task OpenSession(HttpContext context)
        {
            _session.Open();
            context.Response.Cookies.Append("Emuses.SessionId", _session.GetSessionId(), new CookieOptions
            {
                HttpOnly = true
                // Secure = true
            });

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
                context.Response.Redirect("/Account/Expired", true);
                return _next(context);
            }
        }
    }
}
