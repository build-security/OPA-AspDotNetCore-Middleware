using Microsoft.AspNetCore.Mvc;
using Opa.AspDotNetCore.Middleware.Attributes;

namespace SampleApplication.Controllers
{
    [ApiController]
    [Route("project/{id}")]
    public class ProjectController : Controller
    {
        [HttpGet]
        [BuildAuthorize("project_view")]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        [BuildAuthorize("project_create")]
        public IActionResult Create()
        {
            return Accepted();
        }
    }
}
