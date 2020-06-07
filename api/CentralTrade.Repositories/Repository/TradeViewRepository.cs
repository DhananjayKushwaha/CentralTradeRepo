using CentralTrade.Logger;
using CentralTrade.Models;
using CentralTrade.Repositories.Interfaces;
using Newtonsoft.Json;
using Polly;
using Polly.Timeout;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CentralTrade.Repositories
{
    public class TradeViewRepository : ITradeViewRepository
    {
        private readonly ILogger _logger;
        private static Polly.CircuitBreaker.AsyncCircuitBreakerPolicy circuitBreakerPolicy;
        private static int _timeoutSeconds = 30, _retryCount = 3, _sleepTimeAfterFail = 10;
        private readonly string _tradeViewApiUrl = "https://toptradings.com/api/Get";

        private readonly int _tradeViewApiTimeoutInSeconds = 10;
        private static int _errorCountForCircuitbreak = 3, _circuitBreakDuration = 1000;
        public TradeViewRepository(ILogger logger)
        {
            _logger = logger;
        }
        public async Task<List<StockView>> GetTrendingStocks(int topN, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_tradeViewApiUrl))
            {
                _logger.Log(LogSeverity.Error, "Trade view url is not configured.");
            }
            else
            {
                var policy = GetPolicy(_logger);

                return await policy.ExecuteAsync(() => GetStockData(topN, cancellationToken)).ContinueWith(task =>
                {
                    if (!task.IsFaulted && !task.IsCanceled)
                        return task.Result;
                    var error = (task.Exception.InnerException != null) ? task.Exception.InnerException.Message : task.Exception.Message;
                    _logger.Log(LogSeverity.Error, error + "\r\n GetSkuDetails actual function FAILURE - topN " + topN);
                    return null;

                });
            }
            //something went wrong so return null
            return null;
        }

        private async Task<List<StockView>> GetStockData(int topN, CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            try
            {
                //call a external service which is hosted in cloud using HttpClient
                //to similate http call add sleep
                Thread.Sleep(1000);
                return await Task.FromResult(new List<StockView>()
                { new StockView() { Code = "Soc Gen", CurrencySymbol = "$", DeltaPrice = 2.12, Id = Guid.NewGuid(), UnitPrice = 3000.10 } });

                //uncomment below code with real service

                //using (var client = new HttpClient())
                //{
                //    client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                //    client.Timeout = TimeSpan.FromSeconds(_tradeViewApiTimeoutInSeconds);
                //    var response = await client.GetAsync(new Uri($"{_tradeViewApiUrl}/{topN}"), cancellationToken)
                //        .ConfigureAwait(false);
                //    statusCode = response.StatusCode;
                //    response.EnsureSuccessStatusCode();
                //    var responseContents = await response.Content.ReadAsStringAsync();
                //    var result = JsonConvert.DeserializeObject<List<StockView>>(responseContents);
                //    return result;
                //}
            }
            catch (Exception ex)
            {
                var error = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                _logger.Log(LogSeverity.Error, error + "\r\n GetStockData actual function FAILURE");

                if (statusCode == HttpStatusCode.NotFound ||
                    statusCode == HttpStatusCode.RequestTimeout ||
                    statusCode == HttpStatusCode.InternalServerError ||
                    statusCode == HttpStatusCode.BadGateway ||
                    statusCode == HttpStatusCode.ServiceUnavailable ||
                    statusCode == HttpStatusCode.GatewayTimeout ||
                    statusCode == HttpStatusCode.Forbidden)
                {
                    throw;
                }
            }
            //something went wrong so return null
            return null;
        }

        public static IAsyncPolicy GetPolicy(ILogger logger)
        {
            if (circuitBreakerPolicy == null)
            {
                circuitBreakerPolicy = Policy.Handle<Exception>().CircuitBreakerAsync(_errorCountForCircuitbreak, TimeSpan.FromMilliseconds(_circuitBreakDuration),
                    (ex, breakDelay) =>
                    {
                        logger.Log(LogSeverity.Error, (ex.InnerException != null) ? ex.InnerException.Message : ex.Message + "\r\n GetTrendingStocks Circuit broken");
                    },
                    () => { });
            }

            var timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromSeconds(_timeoutSeconds), TimeoutStrategy.Pessimistic);
            var retryPolicy = Policy.Handle<Exception>((ex) =>
            {
                logger.Log(LogSeverity.Error, (ex.InnerException != null) ? ex.InnerException.Message : ex.Message + "\r\n GetTrendingStocks retry FAILURE");
                return !(ex is Polly.CircuitBreaker.BrokenCircuitException);
            }).WaitAndRetryAsync(_retryCount, x => TimeSpan.FromMilliseconds(_sleepTimeAfterFail));
            var pollyWrap = Policy.WrapAsync(circuitBreakerPolicy, timeoutPolicy, retryPolicy);
            return pollyWrap;
        }

    }
}
