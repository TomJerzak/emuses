using Microsoft.AspNetCore.Mvc;

namespace Emuses.Example.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
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
