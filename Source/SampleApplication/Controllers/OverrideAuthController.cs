using Build.Security.AspNetCore.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace SampleApplication.Controllers
{
    [ApiController]
    [Authorize("teacher")]
    [Route("project/overrideAuth")]
    public class OverrideAuthController : BaseAuthController
    {
        [HttpGet]
        [Authorize("read", "edit")]
        public override IActionResult Get()
        {
            return base.Get();
        }
    }
}
