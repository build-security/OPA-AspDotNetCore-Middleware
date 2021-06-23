using Build.Security.AspNetCore.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace SampleApplication.Controllers
{
    [Authorize("manager")]
    public class ProjectController : Controller
    {
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
