using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api1.Controllers
{
    [Route(Program.ServiceName + "/" + Program.Version)]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        [HttpGet("api.get")]
        public IActionResult Get()
        {
            return Ok("this is service1 api");
        }
    }
}
