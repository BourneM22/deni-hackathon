using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

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

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(await _authService.GetAllUsers());
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult CheckPassword()
        {
            String hashed = _passwordHasher.hash("halo");
            return Ok(new { valid = _passwordHasher.verify(hashed, "halo"), HashRes = hashed });
        }
    }
}