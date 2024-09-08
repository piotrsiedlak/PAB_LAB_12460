using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using LabWebApi.Controllers;
using LabWebApi.Core.Interfaces;
using LabWebApi.Core.Entities;

namespace LabTesty.Tests.Controller
{
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly CategoriesController _controller;

        public CategoriesControllerTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _controller = new CategoriesController(_mockCategoryService.Object);
        }

        [Fact]
        public async Task GetCategories_ReturnsOkResult_WithListOfCategories()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category1" },
                new Category { Id = 2, Name = "Category2" }
            };

            _mockCategoryService.Setup(service => service.GetCategoriesAsync()).ReturnsAsync((IEnumerable<Category>)categories);

            var result = await _controller.GetCategories();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnCategories = Assert.IsType<List<Category>>(okResult.Value);
            Assert.Equal(2, returnCategories.Count);
        }

        [Fact]
        public async Task GetCategory_ReturnsOkResult_WithCategory()
        {
            var category = new Category { Id = 1, Name = "Category1" };
            _mockCategoryService.Setup(service => service.GetCategoryByIdAsync(1)).ReturnsAsync(category);

            var result = await _controller.GetCategory(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnCategory = Assert.IsType<Category>(okResult.Value);
            Assert.Equal("Category1", returnCategory.Name);
        }

        [Fact]
        public async Task GetCategory_ReturnsNotFoundResult_WhenCategoryNotFound()
        {
            _mockCategoryService.Setup(service => service.GetCategoryByIdAsync(1)).ReturnsAsync((Category)null);

            var result = await _controller.GetCategory(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateCategory_ReturnsCreatedAtActionResult()
        {
            var category = new Category { Id = 1, Name = "Category1" };

            _mockCategoryService.Setup(service => service.AddCategoryAsync(category)).Returns(Task.CompletedTask);

            var result = await _controller.CreateCategory(category);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetCategory), createdAtActionResult.ActionName);
            Assert.Equal(category.Id, ((Category)createdAtActionResult.Value).Id);
        }

        [Fact]
        public async Task UpdateCategory_ReturnsNoContentResult()
        {
            var category = new Category { Id = 1, Name = "UpdatedCategory" };

            _mockCategoryService.Setup(service => service.UpdateCategoryAsync(category)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateCategory(1, category);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateCategory_ReturnsBadRequest_WhenIdMismatch()
        {
            var category = new Category { Id = 1, Name = "UpdatedCategory" };

            var result = await _controller.UpdateCategory(2, category);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteCategory_ReturnsNoContentResult()
        {
            _mockCategoryService.Setup(service => service.DeleteCategoryAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteCategory(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
