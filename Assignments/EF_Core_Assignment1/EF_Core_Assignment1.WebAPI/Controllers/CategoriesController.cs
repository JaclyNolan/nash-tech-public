using EF_Core_Assignment1.Application.DTOs.Category;
using EF_Core_Assignment1.Application.DTOs.Common;
using EF_Core_Assignment1.Application.Services;
using EF_Core_Assignment1.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EF_Core_Assignment1.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(UserRole.Admin)}")]
        public async Task<ActionResult<PaginatedResult<CategoryAdminViewModel>>> GetAllCategories([FromQuery] GetAllCategoryRequest request)
        {
            var (categories, totalCount) = await _categoryService.GetAllCategoriesAsync(request);

            var paginatedResult = new PaginatedResult<CategoryAdminViewModel>
            {
                Data = categories,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return Ok(paginatedResult);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{nameof(UserRole.Admin)}")]
        public async Task<ActionResult<CategoryAdminViewModel>> GetCategoryById(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = $"{nameof(UserRole.Admin)}")]
        public async Task<ActionResult<CategoryAdminViewModel>> AddCategory([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            var createdCategory = await _categoryService.AddCategoryAsync(createCategoryRequest);
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{nameof(UserRole.Admin)}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryRequest updateCategoryRequest)
        {
            if (!await _categoryService.IsCategoryExist(id))
            {
                return BadRequest();
            }

            await _categoryService.UpdateCategoryAsync(id, updateCategoryRequest);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{nameof(UserRole.Admin)}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
