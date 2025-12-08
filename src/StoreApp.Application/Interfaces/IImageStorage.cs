using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Interfaces
{
    public interface IImageStorage
    {
        Task<string> UploadAsync(Stream fileStream, string fileName);
    }
}
