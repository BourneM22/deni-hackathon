using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTO;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SoundboardController : ControllerBase
    {
        private readonly ISoundbardService _soundbardService;
        private readonly IJwtService _jwtService;
        private readonly ITextToSpeechService _textToSpeechService;

        public SoundboardController(ISoundbardService soundbardService, IJwtService jwtService, ITextToSpeechService textToSpeechService)
        {
            _soundbardService = soundbardService;
            _jwtService = jwtService;
            _textToSpeechService = textToSpeechService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetSoundboards([FromQuery] String? soundboardId)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            if (soundboardId != null)
            {
                return Ok(await _soundbardService.GetSoundboardById(userId, soundboardId));
            }

            return Ok(await _soundbardService.GetAllSoundboard(userId));
        }

        [HttpGet]
        [Route("[action]/{filterId}")]
        public async Task<IActionResult> GetAllSoundboardsByFilterID([FromRoute] String filterId)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            return Ok(await _soundbardService.GetAllSoundboardByFilterId(userId, filterId));
        }

        [HttpGet]
        [Route("[action]/{soundboardId}")]
        public async Task<IActionResult> GetSoundboardSoundById([FromRoute] String soundboardId)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            SoundboardResponse soundboard = await _soundbardService.GetSoundboardById(userId, soundboardId);

            Audio audio = _textToSpeechService.TextToSpeech(soundboard.Description);

            return new FileContentResult(audio.AudioBytes, audio.FileType)
            {
                FileDownloadName = audio.FileName
            };
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddNewSoundboard([FromBody] SoundboardRequest soundboardRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _soundbardService.AddNewSoundBoard(userId, soundboardRequest);

            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> UpdateSoundboard([FromBody] UpdateSoundboardRequest updateSoundboardRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _soundbardService.UpdateSoundboard(userId, updateSoundboardRequest);

            return Ok();
        }

        [HttpDelete]
        [Route("[action]/{soundboardId}")]
        public async Task<IActionResult> DeleteSoundboard([FromRoute] String soundboardId)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _soundbardService.DeleteSoundboard(userId, soundboardId);

            return Ok();
        }
    }
}