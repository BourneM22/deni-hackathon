using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.DTO
{
    public class NoteRequest
    {
        [Required(ErrorMessage = "Title is required")]
        public String Title { get; set;} = String.Empty;

        [Required(ErrorMessage = "Content is required")]
        public String Content { get; set;} = String.Empty;
    }
}