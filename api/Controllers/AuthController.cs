using api.DTO;
using api.Exceptions;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthController(IAuthService authService, IPasswordHasher passwordHasher)
        {
            _authService = authService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                LoginResponse response = await _authService.Authenticate(request);

                return Ok(response);
            }
            catch (Exception e)
            {
                if (e is EmailNotFoundException)
                {
                    return BadRequest("Email doesn't exist!");
                } else
                {
                    return Unauthorized("Email and password doesn't match!");
                }
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _authService.Register(registerRequest);

            return Ok();
        }
    }
}