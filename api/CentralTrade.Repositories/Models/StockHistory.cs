using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace CentralTrade.Repositories.Models
{
    public class StockHistory
    {        
        public Guid Id { get; set; }
        public Guid StockId { get; set; }
        public DateTime ChangeDate { get; set; }
        public StockPrice UnitPrice { get; set; }
        public StockPrice NewUnitPrice { get; set; }

        public StockHistory() { }        
    }
}
