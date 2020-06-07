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
using System.Collections.Generic;

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
        public async Task<IActionResult> GetTrades(int topN, bool includeMyFavStocks)
        {
            var tradeResponse = new TradeResponse() { ValidationResult = new ValidationResult { Success = true } };

            try
            {
                if (topN <= 0)
                {
                    return HandleBadRequest(new BaseRequest(), tradeResponse);
                }

                //run both service call parallely
                var stockViews = _tradeService.Get(topN);
                Task<List<StockView>> myStockViews = null;

                if (includeMyFavStocks)
                {
                    myStockViews = _tradeService.GetMyWatchStocks();
                }

                //and then wait for both to get result
                tradeResponse.Stocks = await stockViews;

                if (myStockViews != null)
                {
                    tradeResponse.MyWatchStocks = await myStockViews;
                }

                return Ok(tradeResponse);
            }
            catch (Exception ex)
            {
                return HandleError(HttpStatusCode.InternalServerError, new BaseRequest(), tradeResponse, ex);
            }
        }
    }    
}
