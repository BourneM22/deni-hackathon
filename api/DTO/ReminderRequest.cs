using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Enum;

namespace api.DTO
{
    public class ReminderRequest
    {
        [Required( ErrorMessage = "Priority is required" )]
        public int PriorityId { get; set; }

        [Required( ErrorMessage = "Title is required" )]
        public String Title { get; set; } = String.Empty;

        [Required( ErrorMessage = "Deadline date is required" )]
        public DateOnly DeadlineDate { get; set; }

        [Required( ErrorMessage = "Start time is required" )]
        public DateTime StartTime { get; set; }

        [Required( ErrorMessage = "End time is required" )]
        public DateTime EndTime { get; set; }

        [Required( ErrorMessage = "Description is required" )]
        public String Description { get; set; } = String.Empty;

        [Required( ErrorMessage = "Type is required")]
        public ReminderType Type { get; set; }
    }
}