using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CentralTrade.Models
{
    public class StockView
    {        
        public Guid Id { get; set; }
        public string Code { get; set; }
        public double UnitPrice { get; set; }
        public string CurrencySymbol { get; set; }
        public double DeltaPrice { get; set; }
    }
}
