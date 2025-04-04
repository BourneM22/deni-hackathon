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
    [Route("api/soundboard-filters")]
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
        public async Task<IActionResult> GetAllSoundboardFilters()
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            return Ok(await _soundboardFilterService.GetAllSounboardFilters(userId));
        }

        [HttpPost]
        public async Task<IActionResult> AddNewSoundboardFilter([FromBody] SoundboardFilterRequest soundboardFilterRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _soundboardFilterService.AddNewSoundboardFilter(userId, soundboardFilterRequest);

            return Ok();
        }

        [HttpPut]
        [Route("{soundboardFilterId}")]
        public async Task<IActionResult> UpdateSoundboardFilter([FromBody] UpdateSoundboardFilterRequest updateSoundboardFilterRequest, [FromRoute] String soundboardFilterId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (soundboardFilterId != updateSoundboardFilterRequest.FilterId)
            {
                return BadRequest();
            }

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _soundboardFilterService.UpdateSounboardFilter(userId, updateSoundboardFilterRequest);

            return Ok();
        }

        [HttpDelete]
        [Route("{soundboardFilterId}")]
        public async Task<IActionResult> DeleteSoundboardFilter([FromRoute] String soundboardFilterId)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _soundboardFilterService.DeleteSoundboardFilter(userId, soundboardFilterId);

            return Ok();
        }
    }
}