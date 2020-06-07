using CentralTrade.Models;
using CentralTrade.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CentralTrade.Repositories.Models;
using System.Threading;

namespace CentralTrade.Domain.Services
{
    /// <summary>
    /// This service can be moved to another api and spin up a new process to handle load
    /// </summary>
    public class TradeService : ITradeService
    {
        private readonly ITradeViewCacheRepository _tradeViewCacheRepository;
        public TradeService(ITradeViewCacheRepository tradeViewCacheRepository)
        {
            _tradeViewCacheRepository = tradeViewCacheRepository;
        }

        public async Task<List<StockView>> Get(int topN)
        {
            return await _tradeViewCacheRepository.GetTrendingStocks(topN, default);
        }

        public async Task<List<StockView>> GetMyWatchStocks()
        {
            //read messages from watchStockQueue
            //build Stock list and return
            //this may call another external service

            //added to simulate delay in response
            Thread.Sleep(1000);

            return await Task.FromResult(new List<StockView>() { new StockView() { Code = "Dell", CurrencySymbol = "$", DeltaPrice = 10.2, Id = Guid.NewGuid(), UnitPrice = 2000.5 } });
        }
    }
}
