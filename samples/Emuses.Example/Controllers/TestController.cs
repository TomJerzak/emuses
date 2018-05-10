using Emuses.Example.Models.Home;
using Microsoft.AspNetCore.Mvc;

namespace Emuses.Example.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
