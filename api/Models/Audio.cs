using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Audio
    {
        public byte[] AudioBytes { get; set; } = [];
        public String FileType { get; set; } = String.Empty;
        public String FileName { get; set; } = String.Empty;
    }
}