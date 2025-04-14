using api.DTO;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/notes")]
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
        public async Task<IActionResult> GetAllNotes([FromQuery] String? tagId, [FromQuery] String? search)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            if (!String.IsNullOrEmpty(search))
            {
                return Ok(await _noteService.SearchNoteByKeyword(userId, search));
            }

            if (!String.IsNullOrEmpty(tagId))
            {
                return Ok(await _noteService.GetAllNotesByTagId(userId, tagId));
            }

            return Ok(await _noteService.GetAllNotes(userId));
        }

        [HttpGet]
        [Route("{noteId}")]
        public async Task<IActionResult> GetNoteById([FromRoute] String noteId)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            return Ok(await _noteService.GetNoteById(userId, noteId));
        }

        [HttpPost]
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
        [Route("{noteId}")]
        public async Task<IActionResult> UpdateNote([FromBody] UpdateNoteRequest updateNoteRequest, [FromRoute] String noteId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (noteId != updateNoteRequest.NoteId)
            {
                return BadRequest();
            }

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _noteService.UpdateNote(updateNoteRequest, userId);
            
            return Ok();
        }

        [HttpDelete]
        [Route("{noteId}")]
        public async Task<IActionResult> DeleteNote([FromRoute] String noteId)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _noteService.DeleteNote(noteId, userId);
            
            return Ok();
        }
    }
}