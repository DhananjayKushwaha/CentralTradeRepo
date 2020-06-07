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
    [Route("api/TradeService")]
    public class TradeController : TradeBaseController
    {
        private readonly ITradeTxService _tradeTxService;

        public TradeController(ILogger logger, ITradeTxService tradeTxService) : base(logger)
        {
            _tradeTxService = tradeTxService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(BuyStockResponse), 200)]
        [Route("Buy")]
        public async Task<IActionResult> Buy([FromBody]BuyStockRequest buyStockRequest)
        {
            var buyStockResponse = new BuyStockResponse() { ValidationResult = new ValidationResult { Success = true } };

            try
            {
                if (buyStockRequest == null ||
                    buyStockRequest.StockId == Guid.Empty ||
                    buyStockRequest.NoOfUnits <= 0)
                {
                    return HandleBadRequest(buyStockRequest, buyStockResponse);
                }

                var saveResponse = await _tradeTxService.Buy(buyStockRequest.UserId, buyStockRequest.StockId, buyStockRequest.NoOfUnits);

                if (!saveResponse)
                {
                    return HandleError(HttpStatusCode.BadRequest, buyStockRequest, buyStockResponse);
                }

                return Ok(buyStockResponse);
            }
            catch (Exception ex)
            {
                return HandleError(HttpStatusCode.InternalServerError, buyStockRequest, buyStockResponse, ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(BuyStockResponse), 200)]
        [Route("Sell")]
        public async Task<IActionResult> Sell([FromBody]SellStockRequest buyStockRequest)
        {
            var sellStockResponse = new SellStockResponse() { ValidationResult = new ValidationResult { Success = true } };

            try
            {
                if (buyStockRequest == null ||
                    buyStockRequest.StockId == Guid.Empty ||
                    buyStockRequest.NoOfUnits <= 0)
                {
                    return HandleBadRequest(buyStockRequest, sellStockResponse);
                }

                var saveResponse = await _tradeTxService.Buy(buyStockRequest.UserId, buyStockRequest.StockId, buyStockRequest.NoOfUnits);

                if (!saveResponse)
                {
                    return HandleError(HttpStatusCode.BadRequest, buyStockRequest, sellStockResponse);
                }

                return Ok(sellStockResponse);
            }
            catch (Exception ex)
            {
                return HandleError(HttpStatusCode.InternalServerError, buyStockRequest, sellStockResponse, ex);
            }
        }
    }
}
