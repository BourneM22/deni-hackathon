using api.DTO;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IFileService _fileService;

        public UserController(IUserService userService, IJwtService jwtService, IFileService fileService)
        {
            _userService = userService;
            _jwtService = jwtService;
            _fileService = fileService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetUserInfo()
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);
            return Ok(await _userService.GetUserInfo(userId));
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] RegisterRequest registerRequest)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _userService.UpdateUserInfo(registerRequest, userId);

            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> UpdateUserProfilePicture(IFormFile profilePicture)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            String imgPath = _fileService.StoreImage(profilePicture);
            await _userService.UpdateProfilePicturePath(imgPath, userId);

            return Ok();
        }
    }
}