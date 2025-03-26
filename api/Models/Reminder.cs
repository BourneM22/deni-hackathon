using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Reminder
    {
        public String ReminderId { get; set; } = String.Empty;
        public String UserId { get; set; } = String.Empty;
        public String PriorityId { get; set; } = String.Empty;
        public String Title { get; set; } = String.Empty;
        public DateOnly DeadlineDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public String Description { get; set; } = String.Empty;
    }
}