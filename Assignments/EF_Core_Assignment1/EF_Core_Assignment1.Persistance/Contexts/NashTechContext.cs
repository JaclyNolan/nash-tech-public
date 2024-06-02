using EF_Core_Assignment1.Domain.Common;
using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace EF_Core_Assignment1.Persistance.Contexts
{
    public class NashTechContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public NashTechContext(DbContextOptions<NashTechContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookBorrowingRequest> BookBorrowingRequests { get; set; }
        public DbSet<BookBorrowingRequestDetails> BookBorrowingRequestDetails { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.Entity<BookBorrowingRequest>()
                .HasOne(r => r.Requestor)
                .WithMany(u => u.RequestedBookBorrowingRequest)
                .HasForeignKey(r => r.RequestorId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);

            builder.Entity<BookBorrowingRequest>()
                .HasOne(r => r.Approver)
                .WithMany(u => u.ApprovedBookBorrowingRequest)
                .HasForeignKey(r => r.ApproverId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.Entity<BookBorrowingRequestDetails>()
                .HasOne(d => d.BookBorrowingRequest)
                .WithMany(r => r.BookBorrowingRequestDetails)
                .HasForeignKey(d => d.BookBorrowingRequestId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(true);

            builder.Entity<BookBorrowingRequestDetails>()
                .HasOne(d => d.Book)
                .WithMany(r => r.BookBorrowingRequestDetails)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimeStamp();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateTimeStamp();
            return base.SaveChanges();
        }

        private void UpdateTimeStamp()
        {
            var modifiedEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(e => e.Entity);

            foreach (var entity in modifiedEntities)
            {
                if (entity is BaseEntity baseEntity)
                {
                    if (baseEntity.DateCreated == default)
                    {
                        baseEntity.DateCreated = DateTime.UtcNow;
                    }
                    baseEntity.DateUpdated = DateTime.UtcNow;
                }
            }

        }
    }
}