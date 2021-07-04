using Microsoft.AspNetCore.Mvc;

namespace SampleApplication.Controllers
{
    [ApiController]
    [Route("project/inheriteAuth")]
    public class InheritedAuthController : BaseAuthController
    {
    }
}
