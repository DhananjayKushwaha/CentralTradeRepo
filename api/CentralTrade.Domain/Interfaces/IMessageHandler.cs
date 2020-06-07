using CentralTrade.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CentralTrade.Domain.Interfaces
{
    public interface IMessageHandler<T> where T : IMessage
    {
        void Send(T message, string queueName);
    }
}
