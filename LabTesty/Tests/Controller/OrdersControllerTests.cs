using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LabWebApi.Controllers;
using LabWebApi.Core.Entities;
using LabWebApi.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LabTesty.Tests.Controller
{

    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _mockOrderService = new Mock<IOrderService>();
            _controller = new OrdersController(_mockOrderService.Object);
        }

        [Fact]
        public async Task GetOrders_ReturnsOkResult_WithListOfOrders()
        {
            var orders = new List<Order>
        {
            new Order { Id = 1, Quantity = 1, OrderDate = DateTime.Now, ProductId = 1 },
            new Order { Id = 2, Quantity = 2, OrderDate = DateTime.Now, ProductId = 2 }
        };

            _mockOrderService.Setup(service => service.GetOrdersAsync()).ReturnsAsync(orders);

            var result = await _controller.GetOrders();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnOrders = Assert.IsType<List<Order>>(okResult.Value);
            Assert.Equal(2, returnOrders.Count);
        }

        [Fact]
        public async Task GetOrder_ReturnsOkResult_WithOrder()
        {
            var order = new Order { Id = 1, Quantity = 1, OrderDate = DateTime.Now, ProductId = 1 };
            _mockOrderService.Setup(service => service.GetOrderByIdAsync(1)).ReturnsAsync(order);

            var result = await _controller.GetOrder(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnOrder = Assert.IsType<Order>(okResult.Value);
            Assert.Equal(1, returnOrder.Quantity);
        }

        [Fact]
        public async Task GetOrder_ReturnsNotFoundResult_WhenOrderNotFound()
        {
            _mockOrderService.Setup(service => service.GetOrderByIdAsync(1)).ReturnsAsync((Order)null);

            var result = await _controller.GetOrder(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateOrder_ReturnsCreatedAtActionResult()
        {
            var order = new Order { Id = 1, Quantity = 1, OrderDate = DateTime.Now, ProductId = 1 };

            _mockOrderService.Setup(service => service.AddOrderAsync(order)).Returns(Task.CompletedTask);

            var result = await _controller.CreateOrder(order);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetOrder), createdAtActionResult.ActionName);
            Assert.Equal(order.Id, ((Order)createdAtActionResult.Value).Id);
        }

        [Fact]
        public async Task UpdateOrder_ReturnsNoContentResult()
        {
            var order = new Order { Id = 1, Quantity = 2, OrderDate = DateTime.Now, ProductId = 1 };

            _mockOrderService.Setup(service => service.UpdateOrderAsync(order)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateOrder(1, order);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateOrder_ReturnsBadRequest_WhenIdMismatch()
        {
            var order = new Order { Id = 1, Quantity = 2, OrderDate = DateTime.Now, ProductId = 1 };

            var result = await _controller.UpdateOrder(2, order);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteOrder_ReturnsNoContentResult()
        {
            _mockOrderService.Setup(service => service.DeleteOrderAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteOrder(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
