using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralTrade.API.Models
{
    public class BuyStockRequest : BaseRequest
    {
        public Guid UserId { get; set; }
        public Guid StockId { get; set; }
        public int NoOfUnits { get; set; }
    }
}
