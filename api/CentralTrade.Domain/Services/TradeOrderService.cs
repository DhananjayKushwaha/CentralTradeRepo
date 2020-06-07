using CentralTrade.Models;
using CentralTrade.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CentralTrade.Repositories.Models;

namespace CentralTrade.Domain.Services
{
    /// <summary>
    /// This service can be moved to another api and spin up a new process/thread to handle load
    /// </summary>
    public class TradeOrderService : ITradeOrderService
    {
        private readonly ITradeRepository _tradeRepository;

        public TradeOrderService(ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }

        public async Task<Guid> UpdateOrder(Order order)
        {
            //update UserStock & Stock data in repo
            return await _tradeRepository.UpdateStockPrice(order.StockId, 123.00);
        }
    }
}
