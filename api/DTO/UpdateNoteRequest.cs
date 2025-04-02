using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTO
{
    public class UpdateNoteRequest
    {
        [Required]
        public String NoteId { get; set; } = String.Empty;

        [Required]
        public String Title { get; set;} = String.Empty;

        [Required]
        public String Content { get; set;} = String.Empty;
    }
}