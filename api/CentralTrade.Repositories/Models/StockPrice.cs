using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace CentralTrade.Repositories.Models
{
    public class StockPrice
    {
        public string CurrencySymbol { get; set; }
        public double Price { get; set; }

        public StockPrice()
        {

        }
    }
}
