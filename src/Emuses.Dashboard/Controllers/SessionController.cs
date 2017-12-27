using Emuses.Dashboard.Models.Session;
using Microsoft.AspNetCore.Mvc;

namespace Emuses.Example.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionStorage _storage;

        public SessionController(ISessionStorage storage)
        {
            _storage = storage;
        }

        public IActionResult Index()
        {
            var session = _storage.GetAll().ToArray()[0];
            var model = new SessionGetModel(session.GetSessionId(), session.GetVersion(), session.GetSessionTimeout(), session.GetExpirationDate());
            /*Request.Cookies.TryGetValue("Emuses.SessionId", out var sessionId);
            
            return sessionId != null ? View(new SessionModel(_storage.GetBySessionId(sessionId))) : View();*/
            return View(model);
        }
    }
}
