using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using CentralTrade.API.Models;
using System.Net;
using CentralTrade.Models.Enums;
using CentralTrade.Models;
using CentralTrade.Domain.Services;
using Newtonsoft.Json;
using Dell.Solution.Cloud.Core.Helpers;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CentralTrade.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{api-version:apiVersion}/TradeService")]
    public class TradeController : Controller
    {
        private readonly ILogger _logger;
        private readonly ITradeTxService _tradeTxService;

        public TradeController(ILogger<TradeController> logger, ITradeTxService tradeTxService)
        {
            _logger = logger;
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

        private ObjectResult HandleBadRequest(BaseRequest request, BaseResponse response)
        {
            response.ValidationResult.Success = false;
            response.ValidationResult.Message = "Input parameters are invalid.";
            response.ValidationResult.ErrorCode = ErrorCode.InvalidInput;

            LogInfo(request, response);

            return StatusCode((int)HttpStatusCode.BadRequest, response);
        }

        private void LogInfo(BaseRequest request, BaseResponse response)
        {
            string reqResMsg = "";

            if (request != null)
            {
                reqResMsg += "Request - " + JsonConvert.SerializeObject(request);
            }

            if (response != null)
            {
                if (reqResMsg != "")
                {
                    reqResMsg += "\r\n";
                }

                reqResMsg += "Response - " + JsonConvert.SerializeObject(response);
            }

            _logger.LogInformation(reqResMsg);
        }

        private ObjectResult HandleError(HttpStatusCode statusCode, BaseRequest request, BaseResponse response, Exception ex = null)
        {
            response.ValidationResult.Success = false;
            response.ValidationResult.ErrorCode = ErrorCode.Unspecified;

            LogInfo(request, response);

            if (ex != null)
            {
                var errMessage = ex.InnerException != null ? ex.Message + " Details:" + ex.InnerException.Message : ex.Message;
                errMessage += " StackTrace:" + ex.StackTrace;
                response.ValidationResult.Message = errMessage;
                _logger.LogError(ex, errMessage);
            }

            return StatusCode((int)statusCode, response);
        }
    }    
}
