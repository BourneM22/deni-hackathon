using api.DTO;
using api.Exceptions;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user")]
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
        public async Task<IActionResult> GetUserInfo()
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);
            return Ok(await _userService.GetUserInfo(userId));
        }

        [HttpGet]
        [Route("profile-picture")]
        public async Task<IActionResult> GetProfilePictureImg()
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            ProfilePictureBytes profilePictureBytes = await _userService.GetProfilePictureByte(userId);

            return new FileContentResult(profilePictureBytes.ImgBytes, "application/octet-stream")
            {
                FileDownloadName = profilePictureBytes.FileName
            };
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserInfo([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _userService.UpdateUserInfo(registerRequest, userId);

            return Ok();
        }

        [HttpPut]
        [Route("profile-picture")]
        public async Task<IActionResult> UpdateUserProfilePicture(IFormFile profilePicture)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            try
            {
                String imgPath = _fileService.StoreImage(profilePicture);
                await _userService.UpdateProfilePicturePath(imgPath, userId);

                return Ok();
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("profile-picture")]
        public async Task<IActionResult> DeleteUserProfilePicture()
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _userService.DeleteProfilePicture(userId);

            return Ok();
        }
    }
}