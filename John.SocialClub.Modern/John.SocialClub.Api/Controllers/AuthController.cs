using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace John.SocialClub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private const string DemoUser = "demo";
        private const string DemoPassword = "demo123";

        public record LoginRequest([Required] string Username, [Required] string Password);
        public record LoginResponse(string Username, string Token);

        /// <summary>
        /// Demo login with hardcoded credentials (demo/demo123). Returns a simple opaque token.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        [ProducesResponseType(401)]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            if (!string.Equals(request.Username, DemoUser, StringComparison.OrdinalIgnoreCase) || request.Password != DemoPassword)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{request.Username}:{DateTime.UtcNow:O}"));
            return Ok(new LoginResponse(request.Username, token));
        }
    }
}