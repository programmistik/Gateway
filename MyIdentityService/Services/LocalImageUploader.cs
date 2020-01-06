using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using ImageMagick;
using IdentityModel.Client;
using System.Text.RegularExpressions;

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

            var fullPath = Path.GetFullPath(path);

            var rotate = getRotationValue(fullPath);

            if (rotate > 0.00)
            {

                using (MagickImage myImg = new MagickImage(fullPath))
                {
                    myImg.Rotate(rotate);
                    myImg.Write(fullPath);
                }

            }

            // Add small size
            using (MagickImage image = new MagickImage(fullPath))
            {
                // MagickGeometry size = new MagickGeometry(100, 100);
                // This will resize the image to a fixed size without maintaining the aspect ratio.
                // Normally an image will be resized to fit inside the specified size.
                var size = new Percentage(50);                 

                image.Resize(size);

                // Save the result
                image.Write("wwwroot/uploads/min." + filename);
            }

            return filename;
        }

        public async Task<string> UploadAva(string file, string IdentityId)
        //    public async Task<string> UploadAva(IFormFile file, string IdentityId)
        {
            
            var filename = $"{IdentityId}.png";
            var path = $"wwwroot/uploads/profiles/{filename}";

            DataImage StrImage = DataImage.TryParse(file);
            

            Image img = StrImage?.Image;

            img.Save(path);

            //using (var fs = new FileStream(path, FileMode.CreateNew))
            //{
            //     await file.CopyToAsync(fs);
            //}

            var fullPath = Path.GetFullPath(path);

            var rotate = getRotationValue(fullPath);

            if (rotate > 0.00)
            {

                using (MagickImage myImg = new MagickImage(fullPath))
                {
                    myImg.Rotate(rotate);
                    myImg.Write(fullPath);
                }

            }

            // Add small size
            using (MagickImage image = new MagickImage(fullPath))
            {
                MagickGeometry size = new MagickGeometry(100, 100);
                // This will resize the image to a fixed size without maintaining the aspect ratio.
                // Normally an image will be resized to fit inside the specified size.
                size.IgnoreAspectRatio = true;

                image.Resize(size);

                // Save the result
                image.Write("wwwroot/uploads/profiles/100x100." + filename);
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

    public sealed class DataImage
    {
        private static readonly Regex DataUriPattern = new Regex(@"^data\:(?<type>image\/(png|tiff|jpg|gif));base64,(?<data>[A-Z0-9\+\/\=]+)$", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);

        private DataImage(string mimeType, byte[] rawData)
        {
            MimeType = mimeType;
            RawData = rawData;
        }

        public string MimeType { get; }
        public byte[] RawData { get; }

        public Image Image => Image.FromStream(new MemoryStream(RawData));

        public static DataImage TryParse(string dataUri)
        {
            if (string.IsNullOrWhiteSpace(dataUri)) return null;

            Match match = DataUriPattern.Match(dataUri);
            if (!match.Success) return null;

            string mimeType = match.Groups["type"].Value;
            string base64Data = match.Groups["data"].Value;

            try
            {
                byte[] rawData = Convert.FromBase64String(base64Data);
                return rawData.Length == 0 ? null : new DataImage(mimeType, rawData);
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}
