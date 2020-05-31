using CentralTrade.Models;
using CentralTrade.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CentralTrade.Repositories.Models
{
    public class UserStock
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid StockId { get; set; }
        public TransactionType TransactionType { get; set; }
        public int Units { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
