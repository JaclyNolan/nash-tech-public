using EF_Core_Assignment1.Application.DTOs.Account;
using EF_Core_Assignment1.Application.DTOs.Role;
using EF_Core_Assignment1.Application.Services;
using EF_Core_Assignment1.WebAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace EF_Core_Assignment1.WebAPI.Tests.Controllers
{
    public class AccountsControllerTests
    {
        private readonly Mock<IAccountService> _mockAccountService;
        private readonly AccountsController _controller;
        private readonly Guid _userId = Guid.NewGuid();

        public AccountsControllerTests()
        {
            _mockAccountService = new Mock<IAccountService>();
            _controller = new AccountsController(_mockAccountService.Object);

            // Set up a mock user with a NameIdentifier claim
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, _userId.ToString())
            }, "mock"));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task GetAccountInfo_WithValidUser_ReturnsOkResult()
        {
            // Arrange
            var accountInfo = new AccountInfoViewModel
            {
                Id = "string",
                Email = "string@string.com",
                Name = "string",
                Roles = new List<RoleViewModel>([
                    new RoleViewModel {
                        Name = "string",
                        Id = "string"
                    }
                ])
            };
            _mockAccountService.Setup(s => s.GetAccountInfoAsync(_userId.ToString()))
                .ReturnsAsync(accountInfo);

            // Act
            var result = await _controller.GetAccountInfo();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.Equal(accountInfo, okResult.Value);
        }

        [Fact]
        public async Task GetAccountInfo_WithInvalidUser_ReturnsUnauthorizedResult()
        {
            // Arrange
            _mockAccountService.Setup(s => s.GetAccountInfoAsync(It.IsAny<string>()))
                .ReturnsAsync((AccountInfoViewModel?) null);

            // Act
            var result = await _controller.GetAccountInfo();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
