using Build.Security.AspNetCore.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace SampleApplication.Controllers
{
    [ApiController]
    [Authorize("overrideAuthController")]
    public class OverrideAuthController : BaseAuthController
    {
        [HttpGet]
        [Authorize("read", "readonly")]
        public override IActionResult Get()
        {
            return base.Get();
        }
    }
}
