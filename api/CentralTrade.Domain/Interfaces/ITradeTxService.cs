using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CentralTrade.Domain.Services
{
    public interface ITradeTxService
    {
        Task<bool> Buy(Guid userId, Guid stockId, int noOfUnits);
        Task<bool> Sell(Guid userId, Guid stockId, int noOfUnits);
    }
}