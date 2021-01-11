using Microsoft.AspNetCore.Mvc;

namespace SampleApplication.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Accepted();
        }
    }
}
