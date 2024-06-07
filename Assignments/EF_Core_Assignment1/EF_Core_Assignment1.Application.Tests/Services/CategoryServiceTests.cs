using AutoMapper;
using EF_Core_Assignment1.Application.DTOs.Category;
using EF_Core_Assignment1.Application.Services;
using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Application.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockMapper = new Mock<IMapper>();
            _categoryService = new CategoryService(_mockCategoryRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_ReturnsPaginatedResult()
        {
            // Arrange
            var request = new GetAllCategoryRequest { PageNumber = 1, PageSize = 10 };
            var categories = new List<Category> { new Category { Id = Guid.NewGuid(), Name = "Test Category" } };
            var totalCount = 1;

            _mockCategoryRepository.Setup(repo => repo.GetCategoriesAsync(
                request.PageNumber,
                request.PageSize,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>())).ReturnsAsync((categories, totalCount));

            var categoryViewModels = new List<CategoryAdminViewModel> { new CategoryAdminViewModel { Id = categories[0].Id, Name = "Test Category" } };

            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<CategoryAdminViewModel>>(categories)).Returns(categoryViewModels);

            // Act
            var (resultCategories, resultTotalCount) = await _categoryService.GetAllCategoriesAsync(request);

            // Assert
            Assert.Equal(categoryViewModels, resultCategories);
            Assert.Equal(totalCount, resultTotalCount);
        }

        [Fact]
        public async Task GetCategoryByIdAsync_ReturnsCategory_WhenCategoryExists()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category { Id = categoryId, Name = "Test Category" };
            var categoryViewModel = new CategoryAdminViewModel { Id = categoryId, Name = "Test Category" };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _mockMapper.Setup(mapper => mapper.Map<CategoryAdminViewModel>(category)).Returns(categoryViewModel);

            // Act
            var result = await _categoryService.GetCategoryByIdAsync(categoryId);

            // Assert
            Assert.Equal(categoryViewModel, result);
        }

        [Fact]
        public async Task GetCategoryByIdAsync_ReturnsNull_WhenCategoryDoesNotExist()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category)null);

            // Act
            var result = await _categoryService.GetCategoryByIdAsync(categoryId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddCategoryAsync_ReturnsCreatedCategory()
        {
            // Arrange
            var createCategoryRequest = new CreateCategoryRequest { Name = "New Category" };
            var category = new Category { Id = Guid.NewGuid(), Name = "New Category" };
            var categoryViewModel = new CategoryAdminViewModel { Id = category.Id, Name = "New Category" };

            _mockMapper.Setup(mapper => mapper.Map<Category>(createCategoryRequest)).Returns(category);
            _mockCategoryRepository.Setup(repo => repo.AddAsync(category)).ReturnsAsync(category);
            _mockMapper.Setup(mapper => mapper.Map<CategoryAdminViewModel>(category)).Returns(categoryViewModel);

            // Act
            var result = await _categoryService.AddCategoryAsync(createCategoryRequest);

            // Assert
            Assert.Equal(categoryViewModel, result);
        }

        [Fact]
        public async Task UpdateCategoryAsync_ReturnsUpdatedCategory()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var updateCategoryRequest = new UpdateCategoryRequest { Name = "Updated Category" };
            var category = new Category { Id = categoryId, Name = "Updated Category" };
            var categoryViewModel = new CategoryAdminViewModel { Id = categoryId, Name = "Updated Category" };

            _mockMapper.Setup(mapper => mapper.Map<Category>(updateCategoryRequest)).Returns(category);
            _mockCategoryRepository.Setup(repo => repo.UpdateAsync(category)).ReturnsAsync(category);
            _mockMapper.Setup(mapper => mapper.Map<CategoryAdminViewModel>(category)).Returns(categoryViewModel);

            // Act
            var result = await _categoryService.UpdateCategoryAsync(categoryId, updateCategoryRequest);

            // Assert
            Assert.Equal(categoryViewModel, result);
        }

        [Fact]
        public async Task DeleteCategoryAsync_CallsRepositoryDelete()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            _mockCategoryRepository.Setup(repo => repo.DeleteAsync(categoryId)).Returns(Task.CompletedTask);

            // Act
            await _categoryService.DeleteCategoryAsync(categoryId);

            // Assert
            _mockCategoryRepository.Verify(repo => repo.DeleteAsync(categoryId), Times.Once);
        }

        [Fact]
        public async Task IsCategoryExist_ReturnsTrue_WhenCategoryExists()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var category = new Category { Id = categoryId, Name = "Existing Category" };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);

            // Act
            var result = await _categoryService.IsCategoryExist(categoryId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsCategoryExist_ReturnsFalse_WhenCategoryDoesNotExist()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category)null);

            // Act
            var result = await _categoryService.IsCategoryExist(categoryId);

            // Assert
            Assert.False(result);
        }
    }
}
