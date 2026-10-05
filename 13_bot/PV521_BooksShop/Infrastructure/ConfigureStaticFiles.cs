using Microsoft.Extensions.FileProviders;

namespace PV521_BooksShop.Infrastructure
{
    public static class ConfigureStaticFiles
    {
        public static void AddStaticFiles(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            string root = environment.ContentRootPath;
            string storagePath = Path.Combine(root, "Storage");
            string imagesPath = Path.Combine(storagePath, "Images");

            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/images",
                FileProvider = new PhysicalFileProvider(imagesPath)
            });
        }
    }
}
