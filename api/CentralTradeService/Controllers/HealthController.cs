using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CentralTrade.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CentralTrade.API.Controllers
{
    [Route("api/[controller]")]
    public class HealthController : TradeBaseController
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="logger"></param>
        public HealthController(ILogger logger) : base(logger)
        {


        }
        [HttpGet("status")]
        public IActionResult Status()
        {
            _logger.Log( LogSeverity.Info, "HEALTH CHECK: /ping");
            return StatusCode((int)HttpStatusCode.OK, new
            {
                Service = "Trade Service",
                Status = "Up",
            });
        }
    }
}