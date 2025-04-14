using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Enum;

namespace api.DTO
{
    public class ParsedReminderRequest
    {
        public int PriorityId { get; set; }
        public String Title { get; set; } = String.Empty;
        public DateOnly DeadlineDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public String Description { get; set; } = String.Empty;
        public ReminderType Type { get; set; }
    }
}