using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PriorityController : ControllerBase
    {
        private readonly IPriorityService _priorityService;
        private readonly IJwtService _jwtService;

        public PriorityController(IPriorityService priorityService, IJwtService jwtService)
        {
            _priorityService = priorityService;
            _jwtService = jwtService;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllPriorities()
        {
            return Ok(_priorityService.GetAllPriorities());
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> UpdatePriority([FromBody] Priority updatedPriority)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            try
            {
                await _priorityService.UpdatePriority(updatedPriority, userId);

                return Ok();
            } catch
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddNewPriority([FromBody] Priority newPriority)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            try
            {
                await _priorityService.AddNewPriority(newPriority, userId);

                return Ok();
            } catch
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        [Route("[action]/{priorityId}")]
        public async Task<IActionResult> DeletePriority([FromRoute] int priorityId)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            try
            {
                await _priorityService.DeletePriority(priorityId, userId);

                return Ok();
            } catch
            {
                return Unauthorized();
            }
        }
    }
}