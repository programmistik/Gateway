using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

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

            //Image img = Image.FromFile(path);
            //if (img.PropertyIdList.Contains(0x0112))
            //{
            //    PropertyItem propOrientation = img.GetPropertyItem(0x0112);
            //    short orientation = BitConverter.ToInt16(propOrientation.Value, 0);
            //    if (orientation == 6)
            //    {
            //        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //    }
            //    else if (orientation == 8)
            //    {
            //        img.RotateFlip(RotateFlipType.Rotate270FlipNone);
            //    }
            //    else
            //    {
            //        // Do nothing
            //    }
            //}


            return filename;
        }
    }
}
