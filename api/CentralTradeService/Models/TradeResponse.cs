using CentralTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CentralTrade.API.Models
{
    public class TradeResponse : BaseResponse
    {
        public List<StockView> Stocks { get; set; }
    }
}
