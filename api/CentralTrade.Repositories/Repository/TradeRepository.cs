using CentralTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
using CentralTrade.Repositories.Interfaces;
using CentralTrade.Repositories.Models;

namespace CentralTrade.Repositories
{
    public class TradeRepository : ITradeRepository
    {
        public async Task<Guid> BuyStock(Guid userId, Guid stockId, int noOfUnits)
        {
            UserStock userStock = new UserStock()
            {
                Id = Guid.NewGuid(),
                TransactionDate = DateTime.Now,
                TransactionType = CentralTrade.Models.Enums.TransactionType.Buy,
                Units = noOfUnits,
                StockId = stockId,
                UserId = userId
            };

            //save async userStock

            return await Task.FromResult(userStock.Id);
        }

        public async Task<Guid> SellStock(Guid userId, Guid stockId, int noOfUnits)
        {
            UserStock userStock = new UserStock()
            {
                Id = Guid.NewGuid(),
                TransactionDate = DateTime.Now,
                Units = noOfUnits,
                TransactionType = CentralTrade.Models.Enums.TransactionType.Sell,
                StockId = stockId,
                UserId = userId
            };

            //save async userStock

            return await Task.FromResult(userStock.Id);
        }

        public async Task<Guid> UpdateStockPrice(Guid stockId, double unitPrice)
        {
            //get the stock from repo
            Stock stock = new Stock();//set it from repo
            stock.UnitPrice.Price = unitPrice;

            StockHistory stockHistory = new StockHistory()
            {
                Id = Guid.NewGuid(),
                StockId = stockId,
                UnitPrice = stock.UnitPrice,
                ChangeDate = DateTime.Now,
                NewUnitPrice = new StockPrice() { Price = unitPrice, CurrencySymbol = stock.UnitPrice.CurrencySymbol }
            };

            //save stockHistory
            //update stock price

            return await Task.FromResult(stock.Id);
        }

        public async Task<Guid> AddStock(Guid stockId, string code, string currencySymbol, double unitPrice, int noOfUnits)
        {
            Stock stock = new Stock() { Id = stockId, Code = code, TotalUnits = noOfUnits, UnitPrice = new StockPrice() { CurrencySymbol = currencySymbol, Price = unitPrice } };

            //async save stock

            return await Task.FromResult(stock.Id);
        }
    }
}
