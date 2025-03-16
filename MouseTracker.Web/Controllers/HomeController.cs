using Microsoft.AspNetCore.Mvc;

namespace MouseTracker.Web.Controllers
{
    [Route("home")]
    public class HomeController : Controller
    {
        /// <summary>
        /// Страница трекинга мышки
        /// </summary>
        /// <returns></returns>
        [HttpGet("index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
