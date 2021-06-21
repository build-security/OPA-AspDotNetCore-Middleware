using Build.Security.AspNetCore.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace SampleApplication.Controllers
{
    // [ApiController]
    [Authorize("manager")]

    // [Route("project/{id}")]
    public class ProjectController : Controller
    {
        // [HttpGet]
        [Authorize("view")]
        public virtual IActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        [Authorize("create")]
        public virtual IActionResult Create()
        {
            return Accepted();
        }
    }
}
