using EF_Core_Assignment1.Application.DTOs.BookBorrowingRequest;
using EF_Core_Assignment1.Application.DTOs.Common;
using EF_Core_Assignment1.Application.Exceptions;
using EF_Core_Assignment1.Application.Services;
using EF_Core_Assignment1.Domain.Entities;
using EF_Core_Assignment1.Persistance.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EF_Core_Assignment1.WebAPI.Controllers
{
    [Route("api/borrowing/requests")]
    [ApiController]
    public class BorrowingRequestsController : ControllerBase
    {
        private readonly int maxBooksPerRequest = ApplicationSettings.BorrowingSettings.MaxBooksPerRequest;
        private readonly int maxRequestsPerMonth = ApplicationSettings.BorrowingSettings.MaxRequestsPerMonth;
        private readonly IBorrowingRequestService _borrowingRequestService;
        public BorrowingRequestsController(IBorrowingRequestService borrowingRequestService)
        {
            _borrowingRequestService = borrowingRequestService;
        }

        [HttpGet]
        [Authorize(Roles = $"{nameof(UserRole.Admin)}")]
        public async Task<ActionResult<PaginatedResult<BookBorrowingRequestAdminViewModel>>> GetAllBorrowingRequest([FromQuery] GetAllBorrowingRequest request)
        {
            var (borrowingRequests, totalCount) = await _borrowingRequestService.GetAllBorrowingRequestAsync(request);

            var paginatedResult = new PaginatedResult<BookBorrowingRequestAdminViewModel>
            {
                Data = borrowingRequests,
                PageNumber = request.Page,
                PageSize = request.PerPage,
                TotalCount = totalCount
            };

            return Ok(paginatedResult);
        }

        [HttpPost("status/{id}")]
        [Authorize(Roles = $"{nameof(UserRole.Admin)}")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateBorrowingRequest request)
        {
            try
            {
                var actionerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _borrowingRequestService.UpdateRequestStatus(id, request.Status, new Guid(actionerId));
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("user")]
        [Authorize(Roles = nameof(UserRole.User))]
        public async Task<IActionResult> CreateUserRequest([FromBody] CreateBorrowingUserRequest request)
        {
            try
            {
                // validate request
                if (request.RequestDetails == null || request.RequestDetails.Count == 0)
                {
                    return BadRequest("Invalid request. Please provide borrowing details.");
                }

                if (request.RequestDetails.Count > maxBooksPerRequest)
                {
                    return BadRequest($"You can borrow up to {maxBooksPerRequest} books in one request.");
                }

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                List<BookBorrowingRequestUserViewModel> borrowingRequestThisMonth = await _borrowingRequestService.GetBorrowingRequestForUserThisMonth(userId);
                if (borrowingRequestThisMonth.Count > maxRequestsPerMonth)
                {
                    return BadRequest($"You have reached the monthly borrowing request limit of {maxRequestsPerMonth}.");
                }

                await _borrowingRequestService.CreateUserBookBorrowingRequest(userId, request);

                return Ok("Request's made!");
            }
            catch (BorrowingValidationException ex)
            {
                return BadRequest($"Validation error: {ex.Message}");
            }
        }


        [HttpGet("user/current-month")]
        [Authorize(Roles = $"{nameof(UserRole.User)}")]
        public async Task<ActionResult<List<BookBorrowingRequestUserViewModel>>> GetRequestForUserThisMonth()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(await _borrowingRequestService.GetBorrowingRequestForUserThisMonth(userId));
        }

        [HttpGet("user")]
        [Authorize(Roles = $"{nameof(UserRole.User)}")]
        public async Task<ActionResult<List<BookBorrowingRequestUserViewModel>>> GetRequestForUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(await _borrowingRequestService.GetBorrowingRequestForUser(userId));
        }
    }
}
