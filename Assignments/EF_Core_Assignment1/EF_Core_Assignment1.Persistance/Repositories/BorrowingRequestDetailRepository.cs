using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace EF_Core_Assignment1.Persistance.Repositories
{
    public interface IBorrowingRequestDetailRepository : IBaseRepository<BookBorrowingRequestDetails>
    {
        Task<bool> IsBorrowingRequestDetailExist(Guid id);
        Task<List<BookBorrowingRequestDetails>> AddManyAsync(List<BookBorrowingRequestDetails> entites);
    }
    public class BorrowingRequestDetailRepository : IBorrowingRequestDetailRepository
    {
        private readonly NashTechContext _context;

        public BorrowingRequestDetailRepository(NashTechContext context)
        {
            _context = context;
        }

        public async Task<BookBorrowingRequestDetails> AddAsync(BookBorrowingRequestDetails entity)
        {
            await _context.BookBorrowingRequestDetails.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<BookBorrowingRequestDetails>> AddManyAsync(List<BookBorrowingRequestDetails> entities)
        {
            await _context.BookBorrowingRequestDetails.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.BookBorrowingRequestDetails.FindAsync(id);
            if (entity != null)
            {
                _context.BookBorrowingRequestDetails.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<BookBorrowingRequestDetails>> GetAllAsync()
        {
            return await _context.BookBorrowingRequestDetails.ToListAsync();
        }

        public async Task<BookBorrowingRequestDetails?> GetByIdAsync(Guid id)
        {
            return await _context.BookBorrowingRequestDetails.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<BookBorrowingRequestDetails> UpdateAsync(BookBorrowingRequestDetails entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> IsBorrowingRequestDetailExist(Guid id)
        {
            return await _context.BookBorrowingRequestDetails.AnyAsync(r => r.Id == id);
        }
    }
}
