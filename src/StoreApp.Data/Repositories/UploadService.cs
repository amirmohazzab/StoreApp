using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using StoreApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data.Repositories
{
    public class UploadService : IUploadService
    {
        private readonly IWebHostEnvironment env;

        public UploadService(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var folder = Path.Combine(env.WebRootPath, "images");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            return $"/images/{fileName}";
        }

        public async Task<bool> DeleteImageAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            var fullPath = Path.Combine(env.WebRootPath, url.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }

            return false;
        }

        public Task<bool> DeleteFileAsync(string filePath)
        {
            try
            {
                var fullPath = Path.Combine(env.WebRootPath, filePath.TrimStart('/'));

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(true);
            }
        }
    }
}
