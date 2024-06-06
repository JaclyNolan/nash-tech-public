using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EF_Core_Assignment1.Persistance.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<IEnumerable<Category>> SearchCategoriesAsync(string searchString);
        Task<(IEnumerable<Category> Categories, int TotalCount)> GetCategoriesAsync(int pageNumber, int pageSize, string sortField, string sortOrder, string search);
    }

    public class CategoryRepository : ICategoryRepository
    {
        private readonly NashTechContext _context;

        public CategoryRepository(NashTechContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Category> Categories, int TotalCount)> GetCategoriesAsync(int pageNumber, int pageSize, string sortField, string sortOrder, string search)
        {
            var query = _context.Categories.AsQueryable();

            // Apply search filter only for name
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Name.Contains(search));
            }

            Expression<Func<Category, object>> expressionOrder;

            // Apply sorting
            switch (sortField.ToLower())
            {
                case "name":
                    expressionOrder = e => e.Name;
                    break;
                case "datecreated":
                    expressionOrder = e => e.DateCreated;
                    break;
                default:
                    // Default sorting by Id if invalid sortField is provided
                    expressionOrder = e => e.Id;
                    break;
            }

            if (sortOrder.Equals("desc", StringComparison.CurrentCultureIgnoreCase))
            {
                query = query.OrderByDescending(expressionOrder);
            }
            else
            {
                query = query.OrderBy(expressionOrder);

            }

            var totalCount = await query.CountAsync();
            var categories = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (categories, totalCount);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Category>> SearchCategoriesAsync(string searchString)
        {
            return await _context.Categories
                .Where(c => c.Name.Contains(searchString))
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            var existingEntity = await _context.Categories.FindAsync(category.Id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
