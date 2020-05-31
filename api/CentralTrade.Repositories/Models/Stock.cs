using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace CentralTrade.Repositories.Models
{
    public class Stock
    {        
        public Guid Id { get; set; }
        public string Code { get; set; }

        public int TotalUnits { get; set; }
        public StockPrice UnitPrice { get; set; }

        public Stock()
        {

        }
    }
}
