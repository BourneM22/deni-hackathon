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
    [Route("api/soundboards")]
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
        public async Task<IActionResult> GetAllSoundboards([FromQuery] String? filterId)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            if (!String.IsNullOrEmpty(filterId))
            {
                return Ok(await _soundbardService.GetAllSoundboardByFilterId(userId, filterId));
            }

            return Ok(await _soundbardService.GetAllSoundboard(userId));
        }

        [HttpGet]
        [Route("{soundboardId}")]
        public async Task<IActionResult> GetSoundboardById([FromRoute] String soundboardId)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            return Ok(await _soundbardService.GetSoundboardById(userId, soundboardId));
        }

        [HttpGet]
        [Route("{soundboardId}/audio")]
        public async Task<IActionResult> GetSoundboardAudioById([FromRoute] String soundboardId)
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
        [Route("{soundboardId}")]
        public async Task<IActionResult> UpdateSoundboard([FromBody] UpdateSoundboardRequest updateSoundboardRequest, [FromRoute] String soundboardId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (soundboardId != updateSoundboardRequest.SoundId)
            {
                return BadRequest();
            }

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _soundbardService.UpdateSoundboard(userId, updateSoundboardRequest);

            return Ok();
        }

        [HttpDelete]
        [Route("{soundboardId}")]
        public async Task<IActionResult> DeleteSoundboard([FromRoute] String soundboardId)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _soundbardService.DeleteSoundboard(userId, soundboardId);

            return Ok();
        }
    }
}