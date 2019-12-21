using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using ImageMagick;

namespace MyIdentityService.Services
{
    public class LocalImageUploader : IImageUploader
    {
        public async Task<string> Upload(IFormFile file)
        //public async Task<string> Upload(IFormFileCollection files)
        {
            //var filename = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filename = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var path = $"wwwroot/uploads/{filename}";

            //Image image = Image.FromFile(file.);
            //var height = image.Height;
            //var width = image.Width;

            using (var fs = new FileStream(path, FileMode.CreateNew))
            {
                //await file.CopyToAsync(fs);
                await file.CopyToAsync(fs);
            }
            //Image image = Image.FromFile(path);
            //var height = image.Height;
            //var width = image.Width;

            var p = Path.GetFullPath(path);

            var rotate = getRotationValue(p);

            if (rotate == 90.00)
            {

                using (MagickImage myImg = new MagickImage(p))
                {
                    myImg.Rotate(90.00);
                    myImg.Write(p);
                }

            }


            return filename;
        }

        private double getRotationValue (string path)
        {
            using (Image img = new Bitmap(path))
            {
                if (img.PropertyIdList.Contains(0x0112))
                {
                    PropertyItem propOrientation = img.GetPropertyItem(0x0112);
                    short orientation = BitConverter.ToInt16(propOrientation.Value, 0);
                    if (orientation == 6)
                    {

                        return 90.00;
                       
                    }
                    else if (orientation == 8)
                    {
                        return 270.00;
                    }
                    else
                    {
                        return 0.00;
                    }
                }
            }
            return 0.00;
        }
    }
}
