using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EF_Core_Assignment1.Persistance.Repositories
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        Task<IEnumerable<Book>> SearchBooksAsync(string searchString);
        Task<(IEnumerable<Book> Books, int TotalCount)> GetBooksAsync(int pageNumber, int pageSize, string sortField, string sortOrder, string search);
    }

    public class BookRepository : IBookRepository
    {
        private readonly NashTechContext _context;

        public BookRepository(NashTechContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Book> Books, int TotalCount)> GetBooksAsync(int pageNumber, int pageSize, string sortField, string sortOrder, string search)
        {
            var query = _context.Books.AsQueryable();

            // Apply search filter only for name
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => b.Title.Contains(search));
            }

            // Apply sorting
            switch (sortField.ToLower())
            {
                case "title":
                    query = sortOrder.Equals("desc", StringComparison.CurrentCultureIgnoreCase)
                        ? query.OrderByDescending(b => b.Title)
                        : query.OrderBy(b => b.Title);
                    break;
                case "datecreated":
                    query = sortOrder.Equals("desc", StringComparison.CurrentCultureIgnoreCase)
                        ? query.OrderByDescending(b => b.DateCreated)
                        : query.OrderBy(b => b.DateCreated);
                    break;
                default:
                    // Default sorting by Id if invalid sortField is provided
                    query = sortOrder.Equals("desc", StringComparison.CurrentCultureIgnoreCase)
                        ? query.OrderByDescending(b => b.Id)
                        : query.OrderBy(b => b.Id);
                    break;
            }

            var totalCount = await query.CountAsync();
            var books = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (books, totalCount);
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string searchString)
        {
            return await _context.Books
                .Where(b => b.Title.Contains(searchString))
                .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(Guid id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<Book> AddAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            var existingEntity = await _context.Books.FindAsync(book.Id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task DeleteAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
    }
}
