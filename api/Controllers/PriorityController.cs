using api.DTO;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/priorities")]
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
        public IActionResult GetAllPriorities()
        {
            return Ok(_priorityService.GetAllPriorities());
        }

        [HttpPut]
        [Route("{priorityId}")]
        public async Task<IActionResult> UpdatePriority([FromBody] UpdatePriorityRequest updatePriorityRequest, [FromRoute] int priorityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (priorityId != updatePriorityRequest.PriorityId)
            {
                return BadRequest();
            }

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            try
            {
                await _priorityService.UpdatePriority(updatePriorityRequest, userId);

                return Ok();
            } catch
            {
                return Unauthorized();
            }
        }

        [HttpPost]
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
        [Route("{priorityId}")]
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