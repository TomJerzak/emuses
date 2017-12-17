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
        private readonly IStorage _storage;

        public EmusesMiddleware(RequestDelegate next, int minutes, IStorage storage)
        {
            _next = next;
            _minutes = minutes;
            _storage = storage;
        }

        public Task Invoke(HttpContext context)
        {
            context.Request.Cookies.TryGetValue("Emuses.SessionId", out var sessionId);
            if (string.IsNullOrEmpty(sessionId))
            {
                _session = new Session().Open(_minutes, _storage);

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
