using AutoMapper;
using EF_Core_Assignment1.Application.DTOs.Account;
using EF_Core_Assignment1.Persistance.Repositories;

namespace EF_Core_Assignment1.Application.Services
{
    public interface IAccountService
    {
        Task<AccountInfoViewModel?> GetAccountInfoAsync(string userId);
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<AccountInfoViewModel?> GetAccountInfoAsync(string userId)
        {
            var user = await _accountRepository.GetAccountByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            return _mapper.Map<AccountInfoViewModel>(user);
        }
    }

}
