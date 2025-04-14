using api.DTO;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/note-tags")]
    public class NoteTagController : ControllerBase
    {
        private readonly INoteTagService _noteTagService;
        private readonly IJwtService _jwtService;

        public NoteTagController(INoteTagService noteTagService, IJwtService jwtService)
        {
            _noteTagService = noteTagService;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNoteTags()
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            return Ok(await _noteTagService.GetAllNoteTags(userId));
        }

        [HttpPost]
        public async Task<IActionResult> AddNewNoteTag([FromBody] NoteTagRequest noteTagRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _noteTagService.AddNewNoteTag(userId, noteTagRequest);

            return Ok();
        }

        [HttpPut]
        [Route("{noteTagId}")]
        public async Task<IActionResult> UpdateNoteTag([FromBody] UpdateNoteTagRequest updateNoteTagRequest, [FromRoute] String noteTagId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (noteTagId != updateNoteTagRequest.TagId)
            {
                return BadRequest();
            }

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _noteTagService.UpdateNoteTag(userId, updateNoteTagRequest);

            return Ok();
        }

        [HttpDelete]
        [Route("{noteTagId}")]
        public async Task<IActionResult> DeleteNoteTag([FromRoute] String noteTagId)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _noteTagService.DeleteNoteTag(userId, noteTagId);

            return Ok();
        }
    }
}