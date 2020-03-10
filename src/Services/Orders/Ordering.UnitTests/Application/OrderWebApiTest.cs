using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Ordering.API.Application.Queries;
using Ordering.API.Infrastructure.Services;
using Ordering.API.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Ordering.API.Application.Commands;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Xunit;

namespace Ordering.UnitTests.Application
{
    public class OrderWebApiTest
    {
        private Mock<IMediator> _mediator;
        private Mock<IOrderQueries> _orderQueries;
        private Mock<IIdentityService> _identityService;
        private Mock<ILogger<OrdersController>> _logger;
        private OrdersController _sut;

        public OrderWebApiTest()
        {
            _mediator = new Mock<IMediator>();
            _orderQueries = new Mock<IOrderQueries>();
            _identityService = new Mock<IIdentityService>();
            _logger = new Mock<ILogger<OrdersController>>();
            _sut = new OrdersController(_mediator.Object, _orderQueries.Object, _identityService.Object, _logger.Object);
        }

        [Fact]
        public async Task Cancel_order_with_requestId_success()
        {
            // Arrange
            _mediator.Setup(x => x.Send(It.IsAny<IdentifiedCommand<CancelOrderCommand, bool>>(), default(CancellationToken)))
                .Returns(Task.FromResult(true));

            // Act
            var actionResult = (OkResult)await _sut.CancelOrderAsync(new CancelOrderCommand(1), Guid.NewGuid().ToString());

            // Assert
            Assert.Equal(actionResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Cancel_order_bad_request()
        {
            // Arrange
            _mediator.Setup(x => x.Send(It.IsAny<IdentifiedCommand<CancelOrderCommand, bool>>(), default(CancellationToken)))
                .Returns(Task.FromResult(true));

            // Act
            var actionResult = (BadRequestResult)await _sut.CancelOrderAsync(new CancelOrderCommand(1), string.Empty);

            // Assert
            Assert.Equal(actionResult.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Ship_order_with_requestId_success()
        {
            // Arrange
            _mediator.Setup(x => x.Send(It.IsAny<IdentifiedCommand<ShipOrderCommand, bool>>(), default(CancellationToken)))
                .Returns(Task.FromResult(true));

            // Act
            var actionResult = (OkResult)await _sut.ShipOrderAsync(new ShipOrderCommand(1), Guid.NewGuid().ToString());

            // Assert
            Assert.Equal(actionResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Ship_order_bad_request()
        {
            // Arrange
            _mediator.Setup(x => x.Send(It.IsAny<IdentifiedCommand<ShipOrderCommand, bool>>(), default(CancellationToken)))
                .Returns(Task.FromResult(true));

            // Act
            var actionResult = (BadRequestResult)await _sut.CancelOrderAsync(new CancelOrderCommand(1), string.Empty);

            // Assert
            Assert.Equal(actionResult.StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Get_orders_success()
        {
            // Arrange
            var fakeDynamicResult = Enumerable.Empty<OrderSummary>();

            _identityService.Setup(x => x.GetUserIdentity()).Returns(Guid.NewGuid().ToString());
            _orderQueries.Setup(x => x.GetOrdersFromUserAsync(Guid.NewGuid())).Returns(Task.FromResult(fakeDynamicResult));

            // Act
            var actionResult = (OkObjectResult)await _sut.GetOrdersFromUserAsync();

            // Assert
            Assert.Equal(actionResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_order_success()
        {
            // Arrange
            var fakeOrderId = 123;
            var fakeDynamicResult = new Order();
            _orderQueries.Setup(x => x.GetOrderAsync(It.IsAny<int>())).Returns(Task.FromResult(fakeDynamicResult));

            // Act
            var actionResult = (OkObjectResult)await _sut.GetOrderAsync(fakeOrderId);

            // Assert
            Assert.Equal(actionResult.StatusCode, (int)System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_cardTypes_success()
        {
            // Arrange
            var fakeDynamicResult = Enumerable.Empty<CardType>();
            _orderQueries.Setup(x => x.GetCardTypesAsync()).Returns(Task.FromResult(fakeDynamicResult));

            // Act
            var actionResult = await _sut.GetCardTypesAsync();

            // Assert
            Assert.Equal(((OkObjectResult)actionResult.Result).StatusCode, (int)System.Net.HttpStatusCode.OK);
        }
    }
}
