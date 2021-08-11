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
            return Ok("You were able to get from server");
        }

        [HttpPost]
        [Authorize("create")]
        public override IActionResult Create()
        {
            return Ok("You created a new object successfully");
        }

        [HttpPut]
        [Authorize("put")]
        public override IActionResult Put()
        {
            return Ok("You change your object successfully");
        }
    }
}
