using AutoMapper;
using EF_Core_Assignment1.Application.DTOs.Account;
using EF_Core_Assignment1.Application.Services;
using EF_Core_Assignment1.Persistance.Identity;
using EF_Core_Assignment1.Persistance.Repositories;
using Moq;

namespace EF_Core_Assignment1.Application.Tests.Services
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockMapper = new Mock<IMapper>();
            _accountService = new AccountService(_mockAccountRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAccountInfoAsync_UserExists_ReturnsAccountInfoViewModel()
        {
            // Arrange
            var userId = "123";
            var account = new ApplicationUser { Id = userId, Name = "Test User" };
            var accountInfoViewModel = new AccountInfoViewModel { Id = userId, Name = "Test User" };

            _mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(userId))
                .ReturnsAsync(account);
            _mockMapper.Setup(mapper => mapper.Map<AccountInfoViewModel>(account))
                .Returns(accountInfoViewModel);

            // Act
            var result = await _accountService.GetAccountInfoAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("Test User", result.Name);
        }

        [Fact]
        public async Task GetAccountInfoAsync_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            var userId = "123";

            _mockAccountRepository.Setup(repo => repo.GetAccountByIdAsync(userId))
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            var result = await _accountService.GetAccountInfoAsync(userId);

            // Assert
            Assert.Null(result);
        }
    }
}