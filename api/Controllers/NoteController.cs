using api.DTO;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;
        private readonly IJwtService _jwtService;

        public NoteController(INoteService noteService, IJwtService jwtService)
        {
            _noteService = noteService;
            _jwtService = jwtService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetNotes([FromQuery] String? noteId)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            if (!String.IsNullOrEmpty(noteId))
            {
                return Ok(await _noteService.GetNoteById(userId, noteId));
            }

            return Ok(await _noteService.GetAllNotes(userId));
        }

        [HttpGet]
        [Route("[action]/{noteTagId}")]
        public async Task<IActionResult> GetAllNotesByTagId([FromRoute] String noteTagId)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            return Ok(await _noteService.GetAllNotesByTagId(userId, noteTagId));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddNewNote([FromBody] NoteRequest newNoteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _noteService.AddNewNote(newNoteRequest, userId);

            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> UpdateNote([FromBody] UpdateNoteRequest updateNoteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _noteService.UpdateNote(updateNoteRequest, userId);
            
            return Ok();
        }

        [HttpDelete]
        [Route("[action]/{noteId}")]
        public async Task<IActionResult> DeleteNote([FromRoute] String noteId)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _noteService.DeleteNote(noteId, userId);
            
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> SearchNotesByKeyword([FromQuery] String search)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            return Ok(await _noteService.SearchNoteByKeyword(userId, search));
        }
    }
}