using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class FileConfig
    {
        public String DestinationPath { get; set; } = String.Empty;
        public String ProfilePicturePath { get; set; } = String.Empty;
        public String SoundBoardPath { get; set; } = String.Empty;

        public String GetProfilePictureFolderPath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), DestinationPath, ProfilePicturePath);
        }

        public String GetSoundBoardFolderPath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), DestinationPath, SoundBoardPath);
        }
    }
}