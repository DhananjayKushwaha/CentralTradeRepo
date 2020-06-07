using Xunit;
using System.Threading.Tasks;
using Moq;
using System;
using CentralTrade.Repositories.Interfaces;
using CentralTrade.Domain.Services;
using CentralTrade.Logger;
using CentralTrade.Domain.Interfaces;
using CentralTrade.Models;

namespace Dell.Solution.PGI.UnitTests
{
    public class TradeTxServiceTests
    {
        private readonly Mock<ITradeRepository> _tradeRepositoryMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<IMessageHandler<IMessage>> _messageHandlerMock;
        
        public TradeTxServiceTests()
        {
            _tradeRepositoryMock = new Mock<ITradeRepository>();
            _loggerMock = new Mock<ILogger>();
            _messageHandlerMock = new Mock<IMessageHandler<IMessage>>();
        }

        [Fact]
        public async Task Buy_ValidRequest_RepoInvoked()
        {
            //arrange
            var tradeTxService = new TradeTxService(_loggerMock.Object, _tradeRepositoryMock.Object, _messageHandlerMock.Object);
            var userId = Guid.NewGuid();
            var stockId = Guid.NewGuid();
            var noOfUnits = 10;
            var userStockId = Guid.NewGuid();

            _tradeRepositoryMock.Setup(x => x.BuyStock(userId, stockId, noOfUnits)).ReturnsAsync(userStockId);
            _messageHandlerMock.Setup(x => x.Send(It.IsAny<IMessage>(), It.IsAny<string>()));

            //act
            var result = await tradeTxService.Buy(userId, stockId, noOfUnits);

            //assert
            Assert.True(result);
            _tradeRepositoryMock.Verify(x => x.BuyStock(userId, stockId, noOfUnits), Times.Once);
            _messageHandlerMock.Verify(x => x.Send(It.Is<Order>(y => y.StockId == stockId), "Order - " + stockId), Times.Once);
        }

        [Fact]
        public async Task Buy_InvalidInput_ErrorLogged()
        {
            //arrange
            var tradeTxService = new TradeTxService(_loggerMock.Object, _tradeRepositoryMock.Object, _messageHandlerMock.Object);
            var userId = Guid.NewGuid();
            var stockId = Guid.NewGuid();
            var noOfUnits = 10;
            var userStockId = Guid.NewGuid();

            _tradeRepositoryMock.Setup(x => x.BuyStock(userId, stockId, noOfUnits))
                .ThrowsAsync(new ArgumentException("Invalid stock id."));
            _loggerMock.Setup(x => x.Log(LogSeverity.Error, It.IsAny<string>()));
            _messageHandlerMock.Setup(x => x.Send(It.IsAny<IMessage>(), It.IsAny<string>()));

            //act
            var result = await tradeTxService.Buy(userId, stockId, noOfUnits);

            //assert
            Assert.False(result);
            _loggerMock.Verify(x => x.Log(LogSeverity.Error, It.Is<string>(y => y == "Invalid stock id.")), Times.Once);
            _messageHandlerMock.Verify(x => x.Send(It.IsAny<Order>(), "Order - " + stockId), Times.Never);
        }

        [Fact]
        public async Task Sell_ValidRequest_RepoInvoked()
        {
            //arrange
            var tradeTxService = new TradeTxService(_loggerMock.Object, _tradeRepositoryMock.Object, _messageHandlerMock.Object);
            var userId = Guid.NewGuid();
            var stockId = Guid.NewGuid();
            var noOfUnits = 10;
            var userStockId = Guid.NewGuid();

            _tradeRepositoryMock.Setup(x => x.SellStock(userId, stockId, noOfUnits)).ReturnsAsync(userStockId);
            _messageHandlerMock.Setup(x => x.Send(It.IsAny<IMessage>(), It.IsAny<string>()));

            //act
            var result = await tradeTxService.Sell(userId, stockId, noOfUnits);

            //assert
            Assert.True(result);
            _tradeRepositoryMock.Verify(x => x.SellStock(userId, stockId, noOfUnits), Times.Once);
            _messageHandlerMock.Verify(x => x.Send(It.Is<Order>(y => y.StockId == stockId), "Order - " + stockId), Times.Once);
        }

        [Fact]
        public async Task UpdateStockPrice_ValidRequest_RepoInvoked()
        {
            //arrange
            var tradeTxService = new TradeTxService(_loggerMock.Object, _tradeRepositoryMock.Object, _messageHandlerMock.Object);
            var userId = Guid.NewGuid();
            var stockId = Guid.NewGuid();
            var newStockPrice = 10.20;
            var userStockId = Guid.NewGuid();

            _tradeRepositoryMock.Setup(x => x.UpdateStockPrice(stockId, newStockPrice)).ReturnsAsync(userStockId);
            _messageHandlerMock.Setup(x => x.Send(It.IsAny<IMessage>(), It.IsAny<string>()));

            //act
            var result = await tradeTxService.UpdateStockPrice(stockId, newStockPrice);

            //assert
            Assert.True(result);
            _tradeRepositoryMock.Verify(x => x.UpdateStockPrice(stockId, newStockPrice), Times.Once);
            _messageHandlerMock.Verify(x => x.Send(It.Is<StockUpdate>(y => y.Id == stockId), "Stock - " + stockId), Times.Once);
        }
    }
}
