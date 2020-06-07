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
        public string Key
        {
            get { return Id.ToString(); }
        }

        public string Exchange
        {
            get { return "trade_stocks"; }
        }

        public string Type
        {
            get { return "direct"; }
        }
    }
}
