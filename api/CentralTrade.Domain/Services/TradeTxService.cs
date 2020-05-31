using Dell.Solution.Cloud.Core.Helpers;
using CentralTrade.Models;
using CentralTrade.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace CentralTrade.Domain.Services
{
    public class TradeTxService : ITradeTxService
    {
        private readonly ITradeRepository _tradeRepository;

        public TradeTxService(ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }

        public async Task<bool> Buy(Guid userId, Guid stockId, int noOfUnits)
        {
            //validate stock & available units from repo
            
            //buy stock
            var userStockId = await _tradeRepository.BuyStock(userId, stockId, noOfUnits);

            Order order = new Order()
            {
                StockId = stockId,
                OrderDate = DateTime.Now,
                UserStockId = userStockId,
                OrderStatus = CentralTrade.Models.Enums.OrderStatus.Placed,
                TransactionType = CentralTrade.Models.Enums.TransactionType.Sell,
            };

            RabbitMqHelper.SendOrder(order, "Order - " + order.StockId);

            return true;
        }

        public async Task<bool> Sell(Guid userId, Guid stockId, int noOfUnits)
        {
            //validate stock & available units from repo

            //sell stock
            var userStockId = await _tradeRepository.SellStock(userId, stockId, noOfUnits);

            Order order = new Order()
            {
                StockId = stockId,
                OrderDate = DateTime.Now,
                UserStockId = userStockId,
                TransactionType = CentralTrade.Models.Enums.TransactionType.Sell,
                OrderStatus = CentralTrade.Models.Enums.OrderStatus.Placed
            };

            RabbitMqHelper.SendOrder(order, "Order - " + order.StockId);

            return true;
        }

        public async Task<bool> UpdateStockPrice(Guid stockId, double unitPrice)
        {
            //validate stock from repo

            //update stock
            await _tradeRepository.UpdateStockPrice(stockId, unitPrice);

            StockUpdate stockUpdate = new StockUpdate()
            {
                Id = stockId,
                UnitPrice = unitPrice
            };

            RabbitMqHelper.SendStockUpdate(stockUpdate, "Stock - " + stockId);

            return true;
        }
    }
}
