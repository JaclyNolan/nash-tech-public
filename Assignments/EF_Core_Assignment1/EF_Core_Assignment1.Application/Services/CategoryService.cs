using AutoMapper;
using EF_Core_Assignment1.Application.DTOs.Category;
using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Application.Services
{
    public interface ICategoryService
    {
        Task<(IEnumerable<CategoryViewModel>, int totalCount)> GetAllCategoriesAsync(GetAllCategoryRequest request);
        Task<CategoryViewModel?> GetCategoryByIdAsync(Guid id);
        Task<CategoryViewModel> AddCategoryAsync(CreateCategoryRequest createCategoryRequest);
        Task<CategoryViewModel> UpdateCategoryAsync(Guid id, UpdateCategoryRequest updateCategoryRequest);
        Task DeleteCategoryAsync(Guid id);
        Task<bool> IsCategoryExist(Guid id);
    }
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<CategoryViewModel>, int totalCount)> GetAllCategoriesAsync(GetAllCategoryRequest request)
        {
            var (categories, totalCount) = await _categoryRepository.GetCategoriesAsync(
                request.PageNumber,
                request.PageSize,
                request.SortField.ToString(),
                request.SortOrder.ToString(),
                request.Search);
            var categoryViewModels = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);
            return (categoryViewModels, totalCount);
        }

        public async Task<CategoryViewModel?> GetCategoryByIdAsync(Guid id)
        {
            return _mapper.Map<CategoryViewModel>(await _categoryRepository.GetByIdAsync(id));
        }

        public async Task<CategoryViewModel> AddCategoryAsync(CreateCategoryRequest createCategoryRequest)
        {
            var category = _mapper.Map<Category>(createCategoryRequest);
            return _mapper.Map<CategoryViewModel>(await _categoryRepository.AddAsync(category));
        }

        public async Task<CategoryViewModel> UpdateCategoryAsync(Guid id, UpdateCategoryRequest updateCategoryRequest)
        {
            var category = _mapper.Map<Category>(updateCategoryRequest);
            category.Id = id;
            return _mapper.Map<CategoryViewModel>(await _categoryRepository.UpdateAsync(category));
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            await _categoryRepository.DeleteAsync(id);
        }

        public async Task<bool> IsCategoryExist(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category != null;
        }
    }

}
