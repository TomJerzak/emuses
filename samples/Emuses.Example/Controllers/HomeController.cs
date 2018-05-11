using Emuses.Example.Models.Home;
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
            Request.Cookies.TryGetValue("Emuses.Session", out var sessionId);
            
            return sessionId != null ? View(new SessionModel(_storage.GetBySessionId(sessionId))) : View();
        }
    }
}
