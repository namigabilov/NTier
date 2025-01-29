using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Processing;

namespace Buisness.Extentions
{
    public static class FileExtention
    {
        public static bool CheckFileLength(this IFormFile file, short lenth)
        {
            return (file.Length / 1024) <= lenth;
        }
        public async static Task<string> CreateFileAsync(this IFormFile file, IWebHostEnvironment _webHostEnvironment, params string[] folders)
        {
            string fileName = $"{Guid.NewGuid()}-{file.FileName}";

            string filePath = Path.Combine(_webHostEnvironment.WebRootPath);

            foreach (string folder in folders)
            {
                filePath = Path.Combine(filePath, folder);
            }

            filePath = Path.Combine(filePath, fileName);

            bool check = file.ContentType.StartsWith("image/");

            if (check && !file.CheckFileLength(1024))
            {
                using (var image = await SixLabors.ImageSharp.Image.LoadAsync(file.OpenReadStream()))
                {
                    var options = new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new SixLabors.ImageSharp.Size { Width = 500 }
                    };
                    image.Mutate(x => x.Resize(options));
                    using (var resizedStream = new MemoryStream())
                    {
                        await image.SaveAsync(resizedStream, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
                        resizedStream.Seek(0, SeekOrigin.Begin);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await resizedStream.CopyToAsync(fileStream);
                        }
                    }
                }
            }
            else
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            return fileName;
        }
        public static void DeleteFile(string fileName, IWebHostEnvironment env, params string[] folders)
        {
            string filePath = Path.Combine(env.WebRootPath);

            foreach (string folder in folders)
            {
                filePath = Path.Combine(filePath, folder);
            }

            filePath = Path.Combine(filePath, fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
