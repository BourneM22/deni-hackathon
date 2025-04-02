using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTO
{
    public class ProfilePictureBytes
    {
        public byte[] ImgBytes { get; set; } = [];
        public String FileName { get; set; } = String.Empty;
    }
}