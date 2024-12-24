using Microsoft.AspNetCore.Mvc;

namespace MiddlewareApp.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Content("Hello from HomeController!");
        }
    }
}