using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CentralTrade.API.Models;
using CentralTrade.Logger;
using CentralTrade.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CentralTrade.API.Controllers
{
    public class TradeBaseController : Controller
    {
        protected readonly ILogger _logger;

        public TradeBaseController(ILogger logger)
        {
            _logger = logger;
        }

        protected ObjectResult HandleBadRequest(BaseRequest request, BaseResponse response)
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

            _logger.Log(LogSeverity.Info, reqResMsg);
        }

        protected ObjectResult HandleError(HttpStatusCode statusCode, BaseRequest request, BaseResponse response, Exception ex = null)
        {
            response.ValidationResult.Success = false;
            response.ValidationResult.ErrorCode = ErrorCode.Unspecified;

            LogInfo(request, response);

            if (ex != null)
            {
                var errMessage = ex.InnerException != null ? ex.Message + " Details:" + ex.InnerException.Message : ex.Message;
                errMessage += " StackTrace:" + ex.StackTrace;
                response.ValidationResult.Message = errMessage;
                _logger.Log(LogSeverity.Error, errMessage);
            }

            return StatusCode((int)statusCode, response);
        }

    }
}