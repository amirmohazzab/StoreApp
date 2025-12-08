using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using StoreApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data.Repositories
{
    public class CloudinaryImageStorage : IImageStorage
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryImageStorage(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<string> UploadAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream)
            };

            var result = await cloudinary.UploadAsync(uploadParams);
            return result.Url.ToString();
        }
    }
}
