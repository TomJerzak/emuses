using System;
using Emuses.Example.Core.Entities;
using Emuses.Example.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Emuses.Example.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISession _session;
        private readonly IEmusesSessionRepository _emusesSessionRepository;

        public HomeController(ISession session, IEmusesSessionRepository emusesSessionRepository)
        {
            _session = session;
            _emusesSessionRepository = emusesSessionRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Open()
        {
            var emusesSession = _emusesSessionRepository.Create(new EmusesSession(_session.Open(30)));
            
            Response.Cookies.Append("Emuses.Example.SessionId", emusesSession.SessionId, new CookieOptions
            {
                Expires = emusesSession.ExpireDateTime,
                HttpOnly = true /*,
                Secure = true*/
            });

            ViewData["session"] = $"Created session. Id: {emusesSession.SessionId}, expired date: {emusesSession.ExpireDateTime}";
            return View("Index");
        }
    }
}
