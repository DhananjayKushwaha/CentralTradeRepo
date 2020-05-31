using CentralTrade.Models;
using CentralTrade.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CentralTrade.Models
{
    public class Order : IMessage
    {
        public Guid Id { get; set; }
        public Guid StockId { get; set; } //used to place message to respective entity queue
        public Guid UserStockId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
