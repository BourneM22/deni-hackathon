using api.DTO;
using api.Enum;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/reminders")]
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
        public async Task<IActionResult> GetAllReminders([FromQuery] IsDone? isDone, [FromQuery] String? date, [FromQuery] String? monthYear)
        {
            List<ReminderResponse> reminders = new List<ReminderResponse>();

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            if (!String.IsNullOrEmpty(date))
            {
                try
                {
                    DateTime parsedDate = DateTime.Parse(date);

                    reminders = await _reminderService.GetSpecificDateReminders(userId,  DateOnly.FromDateTime(parsedDate));
                } catch
                {
                    return BadRequest("Date format must be yyyy-mm-dd");
                }
            } else if (!String.IsNullOrEmpty(monthYear))
            {
                try
                {
                    monthYear = $"{monthYear}-01";
                    DateTime parsedMonthYear = DateTime.Parse(monthYear);

                    reminders = await _reminderService.GetSpecificMonthYearReminders(userId,  DateOnly.FromDateTime(parsedMonthYear));
                } catch
                {
                    return BadRequest("Date format must be yyyy-mm");
                }
            } else
            {
                reminders = await _reminderService.GetAllReminders(userId);
            }

            if (isDone.Equals(IsDone.UNDONE))
            {
                reminders = reminders.Where(r => r.IsDone.Equals(IsDone.UNDONE)).ToList();
            }

            if (isDone.Equals(IsDone.DONE))
            {
                reminders = reminders.Where(r => r.IsDone.Equals(IsDone.DONE)).ToList();
            }

            return Ok(reminders);
        }

        [HttpGet]
        [Route("{reminderId}")]
        public async Task<IActionResult> GetAllReminders([FromRoute] String reminderId)
        {
            List<ReminderResponse> reminders = new List<ReminderResponse>();

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            return Ok(await _reminderService.GetReminderById(reminderId, userId));
        }

        [HttpPost]
        public async Task<IActionResult> AddNewReminder([FromBody] ReminderRequest reminderRequest)
        {
            DateTime? startTime, endTime = null;

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            try
            {
                startTime = DateTime.Parse(reminderRequest.DeadlineDate.ToString("yyyy-MM-dd") + $" {reminderRequest.StartTime}:00");

                if (reminderRequest.EndTime != null)
                {
                    endTime = DateTime.Parse(reminderRequest.DeadlineDate.ToString("yyyy-MM-dd") + $" {reminderRequest.EndTime}:00");
                }
            } catch
            {
                return BadRequest("Start time and end time must be in format hh:mm! (24 hour format)");
            }

            ParsedReminderRequest parsedReminderRequest = new ParsedReminderRequest()
            {
                DeadlineDate = reminderRequest.DeadlineDate,
                Description = reminderRequest.Description,
                EndTime = endTime,
                StartTime = (DateTime)startTime,
                PriorityId = reminderRequest.PriorityId,
                Title = reminderRequest.Title,
                Type = reminderRequest.Type
            };

            await _reminderService.AddNewReminder(userId, parsedReminderRequest);

            return Ok();
        }

        [HttpPut]
        [Route("{reminderId}")]
        public async Task<IActionResult> UpdateReminder([FromBody] UpdateReminderRequest updateReminderRequest, [FromRoute] String reminderId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reminderId != updateReminderRequest.ReminderId)
            {
                return BadRequest();
            }

            DateTime? startTime, endTime = null;

            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            try
            {
                startTime = DateTime.Parse(updateReminderRequest.DeadlineDate.ToString("yyyy-MM-dd") + $" {updateReminderRequest.StartTime}:00");
                
                if (updateReminderRequest.EndTime != null)
                {
                    endTime = DateTime.Parse(updateReminderRequest.DeadlineDate.ToString("yyyy-MM-dd") + $" {updateReminderRequest.EndTime}:00");
                }
            } catch
            {
                return BadRequest("Start time and end time must be in format hh:mm! (24 hour format)");
            }

            ParsedUpdateReminderRequest parsedUpdateReminderRequest = new ParsedUpdateReminderRequest()
            {
                ReminderId = updateReminderRequest.ReminderId,
                IsDone = updateReminderRequest.IsDone,
                DeadlineDate = updateReminderRequest.DeadlineDate,
                Description = updateReminderRequest.Description,
                EndTime = endTime,
                StartTime = (DateTime)startTime,
                PriorityId = updateReminderRequest.PriorityId,
                Title = updateReminderRequest.Title,
                Type = updateReminderRequest.Type
            };

            await _reminderService.UpdateReminder(parsedUpdateReminderRequest, userId);

            return Ok();
        }

        [HttpDelete]
        [Route("{reminderId}")]
        public async Task<IActionResult> DeleteReminder([FromRoute] String reminderId)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _reminderService.DeleteReminder(userId, reminderId);

            return Ok();
        }

        [HttpPut]
        [Route("{reminderId}/done")]
        public async Task<IActionResult> UpdateDoneStatus([FromRoute] String reminderId)
        {
            String token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            String userId = _jwtService.GetUserIdFromToken(token);

            await _reminderService.UpdateToDoneStatus(userId, reminderId);

            return Ok();
        }
    }
}