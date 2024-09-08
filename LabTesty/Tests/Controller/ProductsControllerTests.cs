using LabWebApi.Controllers;
using LabWebApi.Core.Entities;
using LabWebApi.Infrastructure.Repositories.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabTesty.Tests.Controller
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _controller = new ProductsController(_mockProductRepository.Object);
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
        {
            var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product1", Description = "Description1", Price = 10 },
            new Product { Id = 2, Name = "Product2", Description = "Description2", Price = 20 }
        };

            _mockProductRepository.Setup(repo => repo.GetProductsAsync()).ReturnsAsync(products);

            var result = await _controller.GetProducts();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnProducts = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(2, returnProducts.Count);
        }

        [Fact]
        public async Task GetProduct_ReturnsOkResult_WithProduct()
        {
            var product = new Product { Id = 1, Name = "Product1", Description = "Description1", Price = 10 };
            _mockProductRepository.Setup(repo => repo.GetProductByIdAsync(1)).ReturnsAsync(product);

            var result = await _controller.GetProduct(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal("Product1", returnProduct.Name);
        }

        [Fact]
        public async Task GetProduct_ReturnsNotFoundResult_WhenProductNotFound()
        {
            _mockProductRepository.Setup(repo => repo.GetProductByIdAsync(1)).ReturnsAsync((Product)null);

            var result = await _controller.GetProduct(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreatedAtActionResult()
        {
            var product = new Product { Id = 1, Name = "Product1", Description = "Description1", Price = 10 };

            _mockProductRepository.Setup(repo => repo.AddProductAsync(product)).Returns(Task.CompletedTask);

            var result = await _controller.CreateProduct(product);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetProduct), createdAtActionResult.ActionName);
            Assert.Equal(product.Id, ((Product)createdAtActionResult.Value).Id);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNoContentResult()
        {
            var product = new Product { Id = 1, Name = "UpdatedProduct", Description = "UpdatedDescription", Price = 15 };

            _mockProductRepository.Setup(repo => repo.UpdateProductAsync(product)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateProduct(1, product);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsBadRequest_WhenIdMismatch()
        {
            var product = new Product { Id = 1, Name = "UpdatedProduct", Description = "UpdatedDescription", Price = 15 };

            var result = await _controller.UpdateProduct(2, product);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNoContentResult()
        {
            _mockProductRepository.Setup(repo => repo.DeleteProductAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteProduct(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}