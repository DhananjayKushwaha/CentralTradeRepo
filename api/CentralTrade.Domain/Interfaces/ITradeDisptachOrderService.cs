﻿using CentralTrade.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CentralTrade.Domain.Services
{
    public interface ITradeDispatchOrderService
    {
        Task<bool> DispatchOrder(Order order);
    }
}