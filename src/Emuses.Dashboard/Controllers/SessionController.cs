using System.Linq;
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

        [HttpGet]
        public IActionResult Index()
        {
            var result = _storage.GetAll();
            if (!result.Any())
                return NotFound();

            var sessions = result.Select(item => new SessionModel(
                item.GetSessionId(),
                item.GetVersion(),
                item.GetSessionTimeout(),
                item.GetExpirationDate()));

            return View(sessions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            _storage.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
