using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Emuses
{
    public class EmusesMiddleware
    {
        private readonly RequestDelegate _next;
        private Session _session;
        private readonly int _minutes;

        public EmusesMiddleware(RequestDelegate next, int minutes)
        {
            _next = next;
            _minutes = minutes;
        }

        public Task Invoke(HttpContext context)
        {
            context.Request.Cookies.TryGetValue("Emuses.SessionId", out string sessionId);
            if (string.IsNullOrEmpty(sessionId))
            {
                _session = new Session().Open(_minutes);

                context.Response.Cookies.Append("Emuses.SessionId", _session.GetSessionId(), new CookieOptions
                {
                    HttpOnly = true
                    // Secure = true
                });
            }
            else
            {
                Console.WriteLine("Update");
            }

            return _next(context);
        }
    }
}
