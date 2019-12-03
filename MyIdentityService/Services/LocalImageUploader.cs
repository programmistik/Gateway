using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdentityService.Services
{
    public class LocalImageUploader : IImageUploader
    {
        public async Task<string> Upload(IFormFile file)
        {
            var filename = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var path = $"wwwroot/uploads/{filename}";

            using (var fs = new FileStream(path, FileMode.CreateNew))
            {
                await file.CopyToAsync(fs);
            }

            return filename;
        }
    }
}
