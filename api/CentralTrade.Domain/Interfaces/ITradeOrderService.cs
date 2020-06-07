using CentralTrade.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CentralTrade.Domain.Services
{
    public interface ITradeOrderService
    {
        Task<Guid> UpdateOrder(Order order);
    }
}