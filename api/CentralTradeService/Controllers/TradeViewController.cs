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
    [Route("api/v{api-version:apiVersion}/TradeViewController")]
    public class TradeViewController : Controller
    {
        private readonly ILogger _logger;
        private readonly ITradeService _tradeService;

        public TradeViewController(ILogger<TradeViewController> logger, ITradeService tradeService)
        {
            _logger = logger;
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
