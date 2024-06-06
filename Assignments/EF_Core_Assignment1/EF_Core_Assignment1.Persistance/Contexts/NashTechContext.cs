using EF_Core_Assignment1.Domain.Common;
using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EF_Core_Assignment1.Persistance.Contexts
{
    public class NashTechContext
        : IdentityDbContext<
        ApplicationUser, ApplicationRole, string,
        IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public NashTechContext(DbContextOptions<NashTechContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookBorrowingRequest> BookBorrowingRequests { get; set; }
        public DbSet<BookBorrowingRequestDetails> BookBorrowingRequestDetails { get; set; }
        public DbSet<Category> Categories { get; set; }

        private void ConfigureIdentityRelationship(ModelBuilder b)
        {
            b.Entity<ApplicationUser>(b =>
            {
                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
                b.HasMany(e => e.Roles)
                    .WithMany(e => e.Users)
                    .UsingEntity<ApplicationUserRole>(
                        l => l.HasOne(e => e.Role).WithMany(e => e.UserRoles).HasForeignKey(e => e.RoleId),
                        r => r.HasOne(e => e.User).WithMany(e => e.UserRoles).HasForeignKey(e => e.UserId));
            });

            b.Entity<ApplicationRole>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });
        }

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
                .HasOne(r => r.Actioner)
                .WithMany(u => u.ActionedBookBorrowingRequest)
                .HasForeignKey(r => r.ActionerId)
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

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.UserRoles)
                .WithOne()
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            ConfigureIdentityRelationship(builder);
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