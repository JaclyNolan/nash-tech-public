using EF_Core_Assignment1.Application.DTOs.Category;
using EF_Core_Assignment1.Application.DTOs.Common;
using EF_Core_Assignment1.Application.Services;
using EF_Core_Assignment1.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.WebAPI.Tests.Controllers
{
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly CategoriesController _categoriesController;

        public CategoriesControllerTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _categoriesController = new CategoriesController(_mockCategoryService.Object);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsPaginatedResult()
        {
            // Arrange
            var request = new GetAllCategoryRequest { PageNumber = 1, PageSize = 10 };
            var categories = new List<CategoryAdminViewModel> { new CategoryAdminViewModel { Id = Guid.NewGuid(), Name = "Test Category" } };
            var totalCount = 1;

            _mockCategoryService.Setup(service => service.GetAllCategoriesAsync(request))
                .ReturnsAsync((categories, totalCount));

            // Act
            var result = await _categoriesController.GetAllCategories(request);
            var okResult = result.Result as OkObjectResult;
            var paginatedResult = okResult.Value as PaginatedResult<CategoryAdminViewModel>;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.IsType<PaginatedResult<CategoryAdminViewModel>>(okResult.Value);
            Assert.Equal(categories, paginatedResult.Data);
            Assert.Equal(totalCount, paginatedResult.TotalCount);
            Assert.Equal(request.PageNumber, paginatedResult.PageNumber);
            Assert.Equal(request.PageSize, paginatedResult.PageSize);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsCategory_WhenCategoryExists()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new CategoryAdminViewModel { Id = categoryId, Name = "Test Category" };

            _mockCategoryService.Setup(service => service.GetCategoryByIdAsync(categoryId))
                .ReturnsAsync(category);

            // Act
            var result = await _categoriesController.GetCategoryById(categoryId);
            var okResult = result.Result as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(okResult);
            Assert.Equal(category, okResult.Value);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            _mockCategoryService.Setup(service => service.GetCategoryByIdAsync(categoryId))
                .ReturnsAsync((CategoryAdminViewModel)null);

            // Act
            var result = await _categoriesController.GetCategoryById(categoryId);
            var notFoundResult = result.Result as NotFoundResult;

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async Task AddCategory_ValidRequest_ReturnsCreatedCategory()
        {
            // Arrange
            var createCategoryRequest = new CreateCategoryRequest { Name = "New Category" };
            var createdCategory = new CategoryAdminViewModel { Id = Guid.NewGuid(), Name = "New Category" };

            _mockCategoryService.Setup(service => service.AddCategoryAsync(createCategoryRequest))
                .ReturnsAsync(createdCategory);

            // Act
            var result = await _categoriesController.AddCategory(createCategoryRequest);
            var createdResult = result.Result as CreatedAtActionResult;

            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResult);
            Assert.NotNull(createdResult);
            Assert.Equal(nameof(_categoriesController.GetCategoryById), createdResult.ActionName);
            Assert.Equal(createdCategory.Id, createdResult.RouteValues["id"]);
            Assert.Equal(createdCategory, createdResult.Value);
        }

        [Fact]
        public async Task UpdateCategory_ValidRequest_ReturnsNoContent()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var updateCategoryRequest = new UpdateCategoryRequest { Name = "Updated Category" };

            _mockCategoryService.Setup(service => service.IsCategoryExist(categoryId))
                .ReturnsAsync(true);

            // Act
            var result = await _categoriesController.UpdateCategory(categoryId, updateCategoryRequest);
            var noContentResult = result as NoContentResult;

            // Assert
            Assert.IsType<NoContentResult>(noContentResult);
            _mockCategoryService.Verify(service => service.UpdateCategoryAsync(categoryId, updateCategoryRequest), Times.Once);
        }

        [Fact]
        public async Task UpdateCategory_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var updateCategoryRequest = new UpdateCategoryRequest { Name = "Updated Category" };

            _mockCategoryService.Setup(service => service.IsCategoryExist(categoryId))
                .ReturnsAsync(false);

            // Act
            var result = await _categoriesController.UpdateCategory(categoryId, updateCategoryRequest);
            var badRequestResult = result as BadRequestResult;

            // Assert
            Assert.IsType<BadRequestResult>(badRequestResult);
        }

        [Fact]
        public async Task DeleteCategory_ReturnsNoContent()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            _mockCategoryService.Setup(service => service.DeleteCategoryAsync(categoryId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _categoriesController.DeleteCategory(categoryId);
            var noContentResult = result as NoContentResult;

            // Assert
            Assert.IsType<NoContentResult>(noContentResult);
            _mockCategoryService.Verify(service => service.DeleteCategoryAsync(categoryId), Times.Once);
        }
    }
}
