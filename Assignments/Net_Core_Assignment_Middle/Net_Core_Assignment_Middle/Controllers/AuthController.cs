using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Net_Core_Assignment_Day_Middleware.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("login")]
        public IActionResult Login()
        {
            Log.Information("Accessed url /auth/login");
            return Ok();
        }
    }
}
