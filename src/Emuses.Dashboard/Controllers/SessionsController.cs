using System.Linq;
using Emuses.Dashboard.Models.Session;
using Microsoft.AspNetCore.Mvc;

namespace Emuses.Dashboard.Controllers
{
    [Route("api/v1/[controller]")]
    public class SessionsController
    {
        private readonly ISessionStorage _storage;

        public SessionsController(ISessionStorage storage)
        {
            _storage = storage;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _storage.GetAll();
            if (!result.Any())
                return new NotFoundResult();

            return new OkObjectResult(result.Select(item => new SessionGetModel(
                item.GetSessionId(),
                item.GetVersion(),
                item.GetSessionTimeout(),
                item.GetExpirationDate())));
        }
    }
}
