using CentralTrade.Models;
using CentralTrade.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using CentralTrade.Logger;
using CentralTrade.Domain.Interfaces;

namespace CentralTrade.Domain.Services
{
    public class TradeTxService : ITradeTxService
    {
        private readonly ITradeRepository _tradeRepository;
        private readonly ILogger _logger;
        private readonly IMessageHandler<IMessage> _messageHandler;

        public TradeTxService(ILogger logger, 
            ITradeRepository tradeRepository, 
            IMessageHandler<IMessage> messageHandler)
        {
            _logger = logger;
            _tradeRepository = tradeRepository;
            _messageHandler = messageHandler;
        }

        public async Task<bool> Buy(Guid userId, Guid stockId, int noOfUnits)
        {
            try
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

                _messageHandler.Send(order, "Order - " + order.StockId);

                return true;
            }
            catch(Exception ex)
            {
                _logger.Log(LogSeverity.Error, ex.Message);
            }

            return false;
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

            _messageHandler.Send(order, "Order - " + order.StockId);

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

            _messageHandler.Send(stockUpdate, "Stock - " + stockId);

            return true;
        }
    }
}
