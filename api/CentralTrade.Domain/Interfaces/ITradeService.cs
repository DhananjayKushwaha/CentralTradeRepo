using CentralTrade.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CentralTrade.Domain.Services
{
    public interface ITradeService
    {
        Task<List<StockView>> Get(int topN);
        Task<List<StockView>> GetMyWatchStocks();
    }
}