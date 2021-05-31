using Microsoft.AspNetCore.Mvc;

namespace Api2.Controllers
{
    [Route(Program.Version)]
    // [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("get")]
        public IActionResult Get()
        {
            return Ok("this is service2 api:"+ ApplicationBuilderExtensions.Port);
        }

        [HttpGet("inside.get")]
        public IActionResult InsideGet()
        {
            return Ok("this is service2 inside api:" + ApplicationBuilderExtensions.Port);
        }
    }
}
