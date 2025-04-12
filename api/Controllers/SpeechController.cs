using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Vosk;

namespace api.Controllers
{
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

            // Check if the uploaded file is PCM (raw audio)
            var validMimeTypes = new[] { "audio/raw", "application/octet-stream" };

            if (!validMimeTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest("Invalid file format. Please upload a PCM file.");
            }

            // Validate file size or other constraints if needed
            if (file.Length > 10 * 1024 * 1024)  // Max size 10MB for example
            {
                return BadRequest("File size is too large. Maximum allowed size is 10MB.");
            }

            try
            {
                // Process PCM file
                var result = await _speechToTextService.ProcessRawAudioStream(file.OpenReadStream());
                return Ok(result);  // Return the transcription result
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error processing file: {ex.Message}");
            }
        }
    }
}