using CentralTrade.Models;
using CentralTrade.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CentralTrade.Repositories.Interfaces
{
    public interface ITradeViewRepository
    {
        Task<List<StockView>> GetTrendingStocks(int topN, CancellationToken cancellationToken);
    }
}
