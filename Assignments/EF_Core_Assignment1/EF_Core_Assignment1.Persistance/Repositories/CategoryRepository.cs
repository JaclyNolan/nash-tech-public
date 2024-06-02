using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;

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

            // Apply sorting
            switch (sortField.ToLower())
            {
                case "name":
                    query = sortOrder.Equals("desc", StringComparison.CurrentCultureIgnoreCase)
                        ? query.OrderByDescending(c => c.Name)
                        : query.OrderBy(c => c.Name);
                    break;
                case "datecreated":
                    query = sortOrder.Equals("desc", StringComparison.CurrentCultureIgnoreCase)
                        ? query.OrderByDescending(c => c.DateCreated)
                        : query.OrderBy(c => c.DateCreated);
                    break;
                default:
                    // Default sorting by Id if invalid sortField is provided
                    query = sortOrder.Equals("desc", StringComparison.CurrentCultureIgnoreCase)
                        ? query.OrderByDescending(c => c.Id)
                        : query.OrderBy(c => c.Id);
                    break;
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
