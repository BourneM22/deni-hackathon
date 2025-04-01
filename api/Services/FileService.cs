using api.Exceptions;

namespace api.Services
{
    public interface IFileService
    {
        String StoreImage(IFormFile profilePicture);
    }

    public class FileService : IFileService
    {
        public String StoreImage(IFormFile profilePicture)
        {
            // extensions
            List<String> extensions = new List<string>() { ".jpg", ".jpeg", ".png"} ;
            String imgExtension = Path.GetExtension(profilePicture.FileName);

            if (!extensions.Contains(imgExtension))
            {
                throw new ImageExtensionException("Extension must be " + String.Join(", ", extensions));
            }

            // size
            long size = profilePicture.Length;
            if (size > (5 * 1024 * 1024))
            {
                throw new FileSizeExceedException("File size must be under 5 MB");
            }

            // name changing
            String newFileName = Guid.NewGuid().ToString() + imgExtension;
            String path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            // Check if directory exists, create it if not
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            FileStream stream = new FileStream(Path.Combine(path, newFileName), FileMode.Create);
            profilePicture.CopyTo(stream);
            stream.Close();

            return newFileName;
        }
    }
}