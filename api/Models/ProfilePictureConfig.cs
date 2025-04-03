using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class ProfilePictureConfig
    {
        public List<String> AllowedExtensions { get; set; } = new List<String>();
        public int MaxFileSizeInMB { get; set; }
    }
}