using EF_Core_Assignment1.Application.DTOs.Role;

namespace EF_Core_Assignment1.Application.DTOs.Account
{
    public class AccountInfoViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}
