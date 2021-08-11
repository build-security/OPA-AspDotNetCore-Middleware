using Build.Security.AspNetCore.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace SampleApplication.Controllers
{
    [Authorize("baseAuthController")]
    [Route("[controller]")]
    public class BaseAuthController : Controller
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

        [HttpPut]
        [Authorize("put")]
        public virtual IActionResult Put()
        {
            return Accepted();
        }
    }
}
