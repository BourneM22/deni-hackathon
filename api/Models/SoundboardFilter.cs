using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class SoundboardFilter
    {
        public String FilterId { get; set;} = String.Empty;
        public String UserId { get; set;} = String.Empty;
        public String Name { get; set;} = String.Empty;
    }
}