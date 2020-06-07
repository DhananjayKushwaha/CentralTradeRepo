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
        private readonly ITradeViewRepository _tradeViewRepository;
        public TradeService(ITradeViewRepository tradeViewRepository)
        {
            _tradeViewRepository = tradeViewRepository;
        }

        public async Task<List<StockView>> Get(int topN)
        {
            return await _tradeViewRepository.GetTrendingStocks(topN, default);
        }

        public async Task<List<StockView>> GetMyWatchStocks()
        {
            //read messages from watchStockQueue
            //build Stock list and return
            //this may call another external service

            //added to simulate delay in response
            Thread.Sleep(1000);

            return await Task.FromResult(new List<StockView>());
        }
    }
}
