using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTO;
using api.Enum;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReminderController : ControllerBase
    {
        private readonly IReminderService _reminderService;
        private readonly IJwtService _jwtService;

        public ReminderController(IReminderService reminderService, IJwtService jwtService)
        {
            _reminderService = reminderService;
            _jwtService = jwtService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetReminders([FromQuery] String? reminderId, [FromQuery] IsDone? isDone)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            if (IsDone.UNDONE.Equals(isDone))
            {
                return Ok(await _reminderService.GetAllUndoneReminders(userId));
            }

            if (IsDone.DONE.Equals(isDone))
            {
                return Ok(await _reminderService.GetAllDoneReminders(userId));
            }

            if (!String.IsNullOrEmpty(reminderId))
            {
                return Ok(await _reminderService.GetReminderById(reminderId, userId));
            }

            return Ok(await _reminderService.GetAllReminders(userId));
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddNewReminder([FromBody] ReminderRequest reminderRequest)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _reminderService.AddNewReminder(userId, reminderRequest);

            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> UpdateReminder([FromBody] UpdateReminderRequest updateReminderRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _reminderService.UpdateReminder(updateReminderRequest, userId);

            return Ok();
        }

        [HttpDelete]
        [Route("[action]/{reminderId}")]
        public async Task<IActionResult> DeleteReminder([FromRoute] String reminderId)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _reminderService.DeleteReminder(userId, reminderId);

            return Ok();
        }
    }
}