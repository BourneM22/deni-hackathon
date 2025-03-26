using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Soundboard
    {
        public String SoundId { get; set;} = String.Empty;
        public String UserId { get; set;} = String.Empty;
        public String FilterId { get; set;} = String.Empty;
        public String Name { get; set;} = String.Empty;
        public String Description { get; set;} = String.Empty;
        public String SoundPath { get; set;} = String.Empty;
    }
}