using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Enum;

namespace api.DTO
{
    public class UpdateReminderRequest
    {
        [Required( ErrorMessage = "Reminder ID is required" )]
        public String ReminderId { get; set; } = String.Empty;

        [Required( ErrorMessage = "Priority ID is required" )]
        public int PriorityId { get; set; }

        [Required( ErrorMessage = "Title is required" )]
        public String Title { get; set; } = String.Empty;

        [Required( ErrorMessage = "Deadline date is required" )]
        public DateOnly DeadlineDate { get; set; }

        [Required( ErrorMessage = "Start time is required" )]
        public String StartTime { get; set; } = String.Empty;

        public String? EndTime { get; set; }

        [Required( ErrorMessage = "Description is required" )]
        public String Description { get; set; } = String.Empty;

        public IsDone? IsDone { get; set; }

        [Required( ErrorMessage = "Type is required")]
        public ReminderType Type { get; set; }
    }
}