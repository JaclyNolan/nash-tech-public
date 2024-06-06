using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EF_Core_Assignment1.Persistance.Repositories
{
    public interface IBorrowingRequestRepository : IBaseRepository<BookBorrowingRequest>
    {
        Task<(List<BookBorrowingRequest> bookBorrowingRequests, int TotalCount)> GetBorrowingRequestsAsync(int page, int pageSize, string sortField, string sortOrder);
        Task<bool> IsBorrowingRequestExist(Guid id);
        Task<List<BookBorrowingRequest>> GetBorrowingRequestForUserBetween(string userId, DateTime startDate, DateTime endDate);
        Task<List<BookBorrowingRequest>> GetBorrowingRequestForUser(string userId);
    }

    public class BorrowingRequestRepository : IBorrowingRequestRepository
    {
        private readonly NashTechContext _context;

        public BorrowingRequestRepository(NashTechContext context)
        {
            _context = context;
        }

        public async Task<BookBorrowingRequest> AddAsync(BookBorrowingRequest entity)
        {
            await _context.BookBorrowingRequests.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.BookBorrowingRequests.FindAsync(id);
            if (entity != null)
            {
                _context.BookBorrowingRequests.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<BookBorrowingRequest>> GetAllAsync()
        {
            return await _context.BookBorrowingRequests
                .Include(b => b.Requestor)
                .Include(b => b.Actioner)
                .Include(b => b.BookBorrowingRequestDetails)
                    .ThenInclude(d => d.Book)
                        .ThenInclude(b => b.Category)
                .ToListAsync();
        }

        public async Task<(List<BookBorrowingRequest> bookBorrowingRequests, int TotalCount)> GetBorrowingRequestsAsync(int page, int pageSize, string sortField, string sortOrder)
        {
            var query = _context.BookBorrowingRequests
                .Include(b => b.Requestor)
                .Include(b => b.Actioner)
                .Include(b => b.BookBorrowingRequestDetails)
                    .ThenInclude(d => d.Book)
                        .ThenInclude(b => b.Category)
                .AsQueryable();

            Expression<Func<BookBorrowingRequest, object>> expressionOrder;

            // Apply sorting
            switch (sortField.ToLower())
            {
                case "requestorname":
                    expressionOrder = e => e.Requestor.Name;
                    break;
                case "actionername":
                    expressionOrder = e => e.Actioner.Name;
                    break;
                case "status":
                    expressionOrder = e => e.Status;
                    break;
                default:
                    expressionOrder = e => e.RequestDate;
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
            var borrowingRequests = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (borrowingRequests, totalCount);
        }

        public async Task<BookBorrowingRequest?> GetByIdAsync(Guid id)
        {
            return await _context.BookBorrowingRequests
                .Include(b => b.Requestor)
                .Include(b => b.Actioner)
                .Include(b => b.BookBorrowingRequestDetails)
                    .ThenInclude(d => d.Book)
                        .ThenInclude(b => b.Category)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<BookBorrowingRequest>> GetBorrowingRequestForUserBetween(string userId, DateTime startDate, DateTime endDate)
        {
            return await _context.BookBorrowingRequests
                .Include(r => r.BookBorrowingRequestDetails)
                    .ThenInclude(d => d.Book)
                        .ThenInclude(b => b.Category)
                .Where(b => b.RequestorId == userId && b.RequestDate >= startDate && b.RequestDate <= endDate)
                .ToListAsync();
        }
        public async Task<List<BookBorrowingRequest>> GetBorrowingRequestForUser(string userId)
        {
            return await _context.BookBorrowingRequests
                .Include(r => r.BookBorrowingRequestDetails)
                    .ThenInclude(d => d.Book)
                        .ThenInclude(b => b.Category)
                .Where(b => b.RequestorId == userId)
                .ToListAsync();
        }

        public async Task UpdateRequestStatusAsync(Guid id, BookRequestStatus status)
        {
            var request = await _context.BookBorrowingRequests.FindAsync(id);
            if (request != null)
            {
                request.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<BookBorrowingRequest> UpdateAsync(BookBorrowingRequest request)
        {
            var existingEntity = await _context.BookBorrowingRequests.FindAsync(request.Id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<bool> IsBorrowingRequestExist(Guid id)
        {
            return await _context.BookBorrowingRequests.AnyAsync(r => r.Id == id);
        }
    }
}
