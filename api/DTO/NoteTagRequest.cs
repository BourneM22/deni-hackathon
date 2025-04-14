using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTO
{
    public class NoteTagRequest
    {
        [Required( ErrorMessage = "Name is required")]
        public String Name { get; set; } = String.Empty;
    }
}