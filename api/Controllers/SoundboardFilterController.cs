using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTO;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SoundboardFilterController : ControllerBase
    {
        private readonly ISoundboardFilterService _soundboardFilterService;
        private readonly IJwtService _jwtService;

        public SoundboardFilterController(ISoundboardFilterService soundboardFilterService, IJwtService jwtService)
        {
            _soundboardFilterService = soundboardFilterService;
            _jwtService = jwtService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllSoundboardFilters()
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            return Ok(await _soundboardFilterService.GetAllSounboardFilters(userId));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddNewSoundboardFilter([FromBody] SoundboardFilterRequest soundboardFilterRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _soundboardFilterService.AddnewSoundboardFilter(userId, soundboardFilterRequest);

            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> UpdateNewSoundboardFilter([FromBody] UpdateSoundboardFilterRequest updateSoundboardFilterRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _soundboardFilterService.UpdateSounboardFilter(userId, updateSoundboardFilterRequest);

            return Ok();
        }

        [HttpDelete]
        [Route("[action]/{soundboardFilterId}")]
        public async Task<IActionResult> DeleteNewSoundboardFilter([FromRoute] String soundboardFilterId)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _soundboardFilterService.DeleteSoundboardFilter(userId, soundboardFilterId);

            return Ok();
        }
    }
}