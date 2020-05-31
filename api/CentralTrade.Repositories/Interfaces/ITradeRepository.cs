using CentralTrade.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CentralTrade.Repositories.Interfaces
{
    public interface ITradeRepository
    {
        Task<Guid> AddStock(Guid stockId, string code, string currencySymbol, double unitPrice, int noOfUnits);
        Task<Guid> BuyStock(Guid userId, Guid stockId, int noOfUnits);
        Task<Guid> SellStock(Guid userId, Guid stockId, int noOfUnits);
        Task<Guid> UpdateStockPrice(Guid stockId, double unitPrice);
    }
}
