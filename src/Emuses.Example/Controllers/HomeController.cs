using Microsoft.AspNetCore.Mvc;

namespace Emuses.Example.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISessionStorage _storage;

        public HomeController(ISessionStorage storage)
        {
            _storage = storage;
        }

        public IActionResult Index()
        {
            Request.Cookies.TryGetValue("Emuses.SessionId", out var sessionId);

            return sessionId != null ? View(_storage.GetBySessionId(sessionId)) : View();
        }
    }
}
