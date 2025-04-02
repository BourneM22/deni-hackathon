using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTO;
using api.Enum;
using api.Models;
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
            List<ReminderResponse> reminders = new List<ReminderResponse>();

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            if (!String.IsNullOrEmpty(reminderId))
            {
                return Ok(await _reminderService.GetReminderById(reminderId, userId));
            }

            reminders = await _reminderService.GetAllReminders(userId);

            if (IsDone.UNDONE.Equals(isDone))
            {
                reminders = reminders.Where(r => r.IsDone == IsDone.UNDONE).ToList();
            }

            if (IsDone.DONE.Equals(isDone))
            {
                reminders = reminders.Where(r => r.IsDone == IsDone.DONE).ToList();
            }

            return Ok(reminders);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetRemindersByDate([FromQuery] String? date, [FromQuery] IsDone? isDone)
        {
            DateTime parsedDate = DateTime.Now;
            List<ReminderResponse> reminders = new List<ReminderResponse>();

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            if (date == null)
            {
                reminders = await _reminderService.GetSpecificDateReminders(userId, DateOnly.FromDateTime(DateTime.Today));
            } else
            {
                try
                {
                    parsedDate = DateTime.Parse(date);

                    reminders = await _reminderService.GetSpecificDateReminders(userId,  DateOnly.FromDateTime(parsedDate));
                } catch
                {
                    return BadRequest("Date format must be yyyy-mm-dd");
                }
            }

            if (IsDone.UNDONE.Equals(isDone))
            {
                reminders = reminders.Where(r => r.IsDone == IsDone.UNDONE).ToList();
            }

            if (IsDone.DONE.Equals(isDone))
            {
                reminders = reminders.Where(r => r.IsDone == IsDone.DONE).ToList();
            }

            return Ok(reminders);
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