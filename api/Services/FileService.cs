using api.Exceptions;
using api.Models;
using Microsoft.Extensions.Options;

namespace api.Services
{
    public interface IFileService
    {
        String StoreImage(IFormFile profilePicture);
        byte[] GetImageByte(String imgFileName);
    }

    public class FileService : IFileService
    {
        private readonly FileConfig _fileConfig;
        private readonly ProfilePictureConfig _profilePictureConfig;

        public FileService(IOptions<FileConfig> fileConfig, IOptions<ProfilePictureConfig> profilePictureConfig)
        {
            _fileConfig = fileConfig.Value;
            _profilePictureConfig = profilePictureConfig.Value;
        }

        public String StoreImage(IFormFile profilePicture)
        {
            if (profilePicture == null || profilePicture.Length == 0)
            {
                throw new ArgumentException("Image file is empty.");
            }

            // extensions
            List<String> extensions = _profilePictureConfig.AllowedExtensions ;
            String imgExtension = Path.GetExtension(profilePicture.FileName);

            if (!extensions.Contains(imgExtension))
            {
                throw new ImageExtensionException("Extension must be " + String.Join(", ", extensions));
            }

            // size
            long size = profilePicture.Length;
            if (size > (_profilePictureConfig.MaxFileSizeInMB * 1024 * 1024))
            {
                throw new FileSizeExceedException($"File size must be under {_profilePictureConfig.MaxFileSizeInMB} MB");
            }

            // name changing
            String newFileName = Guid.NewGuid().ToString() + imgExtension;
            String path = _fileConfig.GetProfilePictureFolderPath();

            // Check if directory exists, create it if not
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using FileStream stream = new FileStream(Path.Combine(path, newFileName), FileMode.Create);
            profilePicture.CopyTo(stream);

            return newFileName;
        }

        public byte[] GetImageByte(String imgFileName)
        {
            if (String.IsNullOrEmpty(imgFileName))
            {
                throw new EmptyFileNameException();
            }

            String fullPath = Path.Combine(_fileConfig.GetProfilePictureFolderPath(), imgFileName);

            Console.WriteLine(fullPath);

            if (!System.IO.File.Exists(fullPath))
            {
                throw new Exceptions.FileNotFoundException();
            }

            var fileBytes = System.IO.File.ReadAllBytes(fullPath);

            return fileBytes;
        }
    }
}