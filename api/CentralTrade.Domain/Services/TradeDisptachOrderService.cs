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
    /// This service can be moved to another api and spin up a new process/thread to handle load
    /// </summary>
    public class TradeDispatchOrderService : ITradeDispatchOrderService
    {
        private readonly ITradeRepository _tradeRepository;

        public TradeDispatchOrderService(ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }

        public async Task<bool> DispatchOrder(Order order)
        {
            //send emails/sms/send updates to UI through signal r or other machanism

            return await Task.FromResult(true);
        }
    }
}
