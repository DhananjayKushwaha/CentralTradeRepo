using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using CentralTrade.API.Models;
using System.Net;
using CentralTrade.Models.Enums;
using CentralTrade.Models;
using CentralTrade.Domain.Services;
using Newtonsoft.Json;
using System.Linq;
using CentralTrade.Logger;

namespace CentralTrade.API.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/TradeViewController")]
    public class TradeViewController : TradeBaseController
    {
        private readonly ITradeService _tradeService;

        public TradeViewController(ILogger logger, ITradeService tradeService) : base(logger)
        {
            _tradeService = tradeService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(TradeResponse), 200)]
        [Route("GetTopTrades")]
        public async Task<IActionResult> GetTrades(int topN)
        {
            var tradeResponse = new TradeResponse() { ValidationResult = new ValidationResult { Success = true } };

            try
            {
                if (topN <= 0)
                {
                    return HandleBadRequest(new BaseRequest(), tradeResponse);
                }

                var stockViews = await _tradeService.Get(topN);
                
                tradeResponse.Stocks = stockViews;

                return Ok(tradeResponse);
            }
            catch (Exception ex)
            {
                return HandleError(HttpStatusCode.InternalServerError, new BaseRequest(), tradeResponse, ex);
            }
        }
    }    
}
