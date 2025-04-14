using System.Speech.Synthesis;
using api.DTO;
using api.Enum;
using api.Exceptions;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/auth")]
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
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                LoginResponse response = await _authService.Authenticate(request);

                return Ok(response);
            }
            catch
            {
                return BadRequest("Email and password doesn't match!");
            }
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _authService.Register(registerRequest);

                return Ok();

            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("validate/email")]
        public async Task<IActionResult> CheckExistingEmail([FromBody] Email email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _authService.CheckEmailAlreadyExist(email.EmailAddress));
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] Email email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _authService.ResetPassword(email.EmailAddress);

            return Ok(new {Message = $"New password has been send into {email.EmailAddress}"});
        }
    }
}