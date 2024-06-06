using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EF_Core_Assignment1.Persistance.Repositories
{
    public interface IAccountRepository
    {
        Task<ApplicationUser?> GetAccountByIdAsync(string userId);
    }
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountRepository(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ApplicationUser?> GetAccountByIdAsync(string userId)
        {
            return await _userManager.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
