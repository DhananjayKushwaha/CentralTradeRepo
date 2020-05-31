using Dell.Solution.Cloud.Core.Helpers;
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
    /// This service can be moved to another api and spin up a new process to handle load
    /// </summary>
    public class TradeService : ITradeService
    {
        public async Task<List<StockView>> Get(int topN)
        {
            //read messages from StockQueue
            //build Stock list and return
            return new List<StockView>();
        }
    }
}
