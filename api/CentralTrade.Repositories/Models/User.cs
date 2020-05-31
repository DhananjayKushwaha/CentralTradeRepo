using CentralTrade.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CentralTrade.Repositories.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
