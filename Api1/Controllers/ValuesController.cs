using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api1.Controllers
{
    [Route(Program.ServiceName+"/"+Program.Version)]
    // [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("get")]
        public IActionResult Get()
        {
            return Ok("this is service1 api:"+ ApplicationBuilderExtensions.Port);
        }

        [HttpGet("inside.get")]
        public async Task<IActionResult> InsideGet()
        {
           var result =   await "http://127.0.0.1:9999/service2/v1/inside.get".GetAsync().ReceiveString();
            return Ok(result);
        }
    }
}
