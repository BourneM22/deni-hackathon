using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Vosk;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/speech")]
    public class SpeechController : ControllerBase
    {
        private readonly ISpeechToTextService _speechToTextService;

        public SpeechController(ISpeechToTextService speechToTextService)
        {
            _speechToTextService = speechToTextService;
        }

        [HttpPost]
        [Route("transcribe")]
        public async Task<IActionResult> TranscribeAudio(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided.");
            }

            // Optional size check (10MB)
            if (file.Length > 10 * 1024 * 1024)
            {
                return BadRequest("File size is too large. Maximum allowed size is 10MB.");
            }

            // Extract file extension
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            try
            {
                SpeechToTextResponse result;

                if (extension == ".pcm")
                {
                    // Direct raw PCM stream
                    result = await _speechToTextService.ProcessRawAudioStream(file.OpenReadStream());
                }
                else if (extension == ".wav" || extension == ".mp3" || extension == ".m4a")
                {
                    // Use FFmpeg pipeline
                    result = await _speechToTextService.ProcessAudioFile(file);
                }
                else
                {
                    return BadRequest("Unsupported file format. Only .pcm, .wav, .mp3, or .m4a are allowed.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error processing file: {ex.Message}");
            }
        }
    }
}