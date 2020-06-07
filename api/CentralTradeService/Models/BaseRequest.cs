using CentralTrade.Models;
using CentralTrade.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace CentralTrade.API.Models
{
    public class BaseRequest
    {
        [Required]
        public string CorrelationId { get; set; }
    }
}
