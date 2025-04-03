using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTO
{
    public class UpdateSoundboardRequest
    {
        [Required( ErrorMessage = "Sound ID is required")]
        public String SoundId { get; set;} = String.Empty;

        public String? FilterId { get; set;}

        [Required( ErrorMessage = "Name is required")]
        public String Name { get; set;} = String.Empty;

        [Required( ErrorMessage = "Description is required")]
        public String Description { get; set;} = String.Empty;
    }
}