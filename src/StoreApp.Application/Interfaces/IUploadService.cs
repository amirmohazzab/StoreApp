using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Interfaces
{
    public interface IUploadService
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task<bool> DeleteImageAsync(string url);
        Task<bool> DeleteFileAsync(string filePath);
    }
}
