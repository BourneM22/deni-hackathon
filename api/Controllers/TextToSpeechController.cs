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
    [ApiController]
    [Route("api/tts")]
    public class TextToSpeechController : ControllerBase
    {
        private readonly ITextToSpeechService _textToSpeechService;
        private readonly IJwtService _jwtService;

        public TextToSpeechController(ITextToSpeechService textToSpeechService, IJwtService jwtService)
        {
            _textToSpeechService = textToSpeechService;
            _jwtService = jwtService;
        }

        [HttpPost]
        [Route("audio")]
        public IActionResult SendTextToSpeech([FromBody] TextToSpeechRequest textToSpeechRequest)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            Audio audio = _textToSpeechService.TextToSpeech(textToSpeechRequest.Text);

            return new FileContentResult(audio.AudioBytes, audio.FileType)
            {
                FileDownloadName = audio.FileName
            };
        }
    }
}