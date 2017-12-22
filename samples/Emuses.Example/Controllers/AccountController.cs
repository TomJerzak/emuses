using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Emuses.Example.Controllers
{
    public class AccountController : Controller
    {
        private readonly Session _session;

        public AccountController(ISessionStorage storage)
        {
            _session = new Session(2, storage);
        }

        public IActionResult Login()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Login(string model)
        {
            _session.Open();
            Response.Cookies.Append("Emuses.SessionId", _session.GetSessionId(), new CookieOptions
            {
                HttpOnly = true
                // Secure = true
            });

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {            
            return View();
        }

        public IActionResult Expired()
        {
            return View();
        }
    }
}
