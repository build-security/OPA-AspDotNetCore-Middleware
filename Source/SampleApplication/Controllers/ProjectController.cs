using Microsoft.AspNetCore.Mvc;
using Opa.AspDotNetCore.Middleware.Attributes;

namespace SampleApplication.Controllers
{
    [ApiController]
    [Route("project/{id}")]
    public class ProjectController : Controller
    {
        [HttpGet]
        [BuildSecurityAuthorize("project.view")]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        [BuildSecurityAuthorize("project.create")]
        public IActionResult Create()
        {
            return Accepted();
        }
    }
}
