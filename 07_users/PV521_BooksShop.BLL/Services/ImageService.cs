using Microsoft.AspNetCore.Http;

namespace PV521_BooksShop.BLL.Services
{
    public class ImageService
    {
        public async Task<string?> SaveAsync(IFormFile file, string path, CancellationToken ct = default)
        {
            try
            {
                var types = file.ContentType.Split("/");

                if (types.Length != 2 || types[0] != "image")
                {
                    return null;
                }

                var ext = Path.GetExtension(file.FileName);
                var imageName = $"{Guid.NewGuid()}{ext}";
                var imagePath = Path.Combine(path, imageName);

                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream, ct);
                }

                return imageName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void Remove(string path)
        {
            if(File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
