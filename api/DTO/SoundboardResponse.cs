using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTO
{
    public class SoundboardResponse
    {
        public String SoundId { get; set;} = String.Empty;
        public String? FilterId { get; set;}
        public String Name { get; set;} = String.Empty;
        public String Description { get; set;} = String.Empty;
    }
}