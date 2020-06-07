using System;
using System.Collections.Generic;
using System.Text;

namespace CentralTrade.Models
{
    public interface IMessage
    {
        string Key { get; }
        string Exchange { get; }
        string Type { get; }
    }
}
