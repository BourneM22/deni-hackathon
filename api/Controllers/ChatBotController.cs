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
    [Route("api/chatbot")]
    public class ChatBotController : ControllerBase
    {
        private readonly IChatBotService _chatBotService;
        private readonly IJwtService _jwtService;

        public ChatBotController(IChatBotService chatBotService, IJwtService jwtService)
        {
            _chatBotService = chatBotService;
            _jwtService = jwtService;
        }

        [HttpPost]
        public async Task<IActionResult> SendPrompt([FromBody] ChatBotRequest chatBotRequest)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            return Ok(await _chatBotService.AskChatBot(chatBotRequest, token, userId));
        }
    }
}