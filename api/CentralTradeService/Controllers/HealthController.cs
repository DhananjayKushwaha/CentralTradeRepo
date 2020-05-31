using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CentralTrade.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly ILogger _logger;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="logger"></param>
        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        [HttpGet("status")]
        public IActionResult Status()
        {
            _logger.LogInformation("HEALTH CHECK: /ping");
            return StatusCode((int)HttpStatusCode.OK, new
            {
                Service = "Trade Service",
                Status = "Up",
            });
        }
    }
}