using CentralTrade.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CentralTrade.Models
{
    public class ValidationResult
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ErrorCode ErrorCode { get; set; } = ErrorCode.None;
    }
}
