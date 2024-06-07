using EF_Core_Assignment1.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EF_Core_Assignment1.WebAPI.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("info")]
        [Authorize]
        public async Task<IActionResult> GetAccountInfo()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // Get the corresponding applicationUser from the request with the JWT token
            var accountInfo = await _accountService.GetAccountInfoAsync(userId);
            if (accountInfo == null)
            {
                return NotFound();
            }

            return Ok(accountInfo);
        }
    }
}