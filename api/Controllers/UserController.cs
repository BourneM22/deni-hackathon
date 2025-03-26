using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public UserController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(await _userService.GetAllUsers());
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CheckId()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            return Ok(_jwtService.getUserIdFromToken(token!));
        }
    }
}