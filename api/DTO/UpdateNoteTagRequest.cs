using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTO
{
    public class UpdateNoteTagRequest
    {
        [Required( ErrorMessage = "Tag ID is required")]
        public String TagId { get; set; } = String.Empty;

        [Required( ErrorMessage = "Name is required")]
        public String Name { get; set; } = String.Empty;
    }
}