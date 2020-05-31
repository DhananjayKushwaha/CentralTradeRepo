using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CentralTrade.Models
{
    public class StockUpdate: IMessage
    {        
        public Guid Id { get; set; }
        public double UnitPrice { get; set; }
    }
}
