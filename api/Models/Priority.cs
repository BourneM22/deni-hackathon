using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Priority
    {
        [Required(ErrorMessage = "PriorityId is required")]
        public int PriorityId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public String Name { get; set; } = String.Empty;
    }
}