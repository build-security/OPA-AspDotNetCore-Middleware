using Build.Security.AspNetCore.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace SampleApplication.Controllers
{
    [ApiController]
    [Authorize("teacher")]
    [Route("project/{id}")]
    public class ChildProjectController : ProjectController
    {
        [HttpGet]
        [Authorize("read", "edit")]
        public override IActionResult Get()
        {
            return base.Get();
        }
    }
}
