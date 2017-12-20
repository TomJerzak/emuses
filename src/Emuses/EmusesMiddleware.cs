using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Emuses
{
    public class EmusesMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Session _session;

        private EmusesMiddleware() { }

        public EmusesMiddleware(RequestDelegate next, int sessionTimeout, IStorage storage)
        {
            _session = new Session(sessionTimeout, storage);
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            context.Request.Cookies.TryGetValue("Emuses.SessionId", out var sessionId);
            if (string.IsNullOrEmpty(sessionId))
            {
                _session.Open();
                context.Response.Cookies.Append("Emuses.SessionId", _session.GetSessionId(), new CookieOptions
                {
                    HttpOnly = true
                    // Secure = true
                });
            }
            else
            {
                var sessionBySessionId = _session.GetStorage().GetBySessionId(sessionId);
                _session.Restore(sessionBySessionId.GetSessionId(), sessionBySessionId.GetVersion(), sessionBySessionId.GetExpirationDate(), sessionBySessionId.GetMinutes(), sessionBySessionId.GetStorage());

                _session.Update();
            }

            return _next(context);
        }
    }
}
