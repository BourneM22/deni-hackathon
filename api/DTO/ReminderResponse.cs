using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Enum;

namespace api.DTO
{
    public class ReminderResponse
    {
        public String ReminderId { get; set; } = String.Empty;
        public int PriorityId { get; set; }
        public String PriorityName { get; set; } = String.Empty;
        public String Title { get; set; } = String.Empty;
        public DateOnly DeadlineDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public String Description { get; set; } = String.Empty;
        public IsDone? IsDone { get; set; }
        public ReminderType Type { get; set; }
    }
}