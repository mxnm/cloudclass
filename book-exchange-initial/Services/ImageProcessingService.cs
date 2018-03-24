using book_exchange_initial.Entities;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;

namespace book_exchange_initial.Services
{
    public class ImageProcessingService
    {
        private string imagesPath;

        public ImageProcessingService(string imagesPath)
        {
            this.imagesPath = imagesPath;
        }

        public void ProcessImage(Stream imageStream, BookListing listing)
        {
            // Resize images
            var thumbnailBytes = this.ResizeImageForThumbnail(imageStream);
            imageStream.Position = 0;
            var newImageBytes = this.ResizeImageForFullImage(imageStream);

            var imageName = Guid.NewGuid().ToString();
            var imagePath = this.imagesPath + imageName + ".jpg";
            var thumbnailPath = this.imagesPath + imageName + "_thumb.jpg";

            // Write images to disk
            Directory.CreateDirectory(this.imagesPath);
            File.WriteAllBytes(imagePath, newImageBytes);
            File.WriteAllBytes(thumbnailPath, thumbnailBytes);

            // Update BookListing with image name
            listing.SetImage(imageName);
        }

        public byte[] ResizeImageForThumbnail(Stream imageStream)
        {
            var resizedStream = new MemoryStream();
            using (Image<Rgba32> image = Image.Load(imageStream))
            {
                var options = new ResizeOptions();
                options.Mode = ResizeMode.Max;
                options.Size = new SixLabors.Primitives.Size(500, 300);
                image.Mutate(x => x.Resize(options));
                image.SaveAsJpeg(resizedStream);
            }

            return resizedStream.ToArray();
        }

        public byte[] ResizeImageForFullImage(Stream imageStream)
        {
            var resizedStream = new MemoryStream();
            using (Image<Rgba32> image = Image.Load(imageStream))
            {
                var options = new ResizeOptions();
                options.Mode = ResizeMode.Max;
                options.Size = new SixLabors.Primitives.Size(1200, 800);
                image.Mutate(x => x.Resize(options));
                image.SaveAsJpeg(resizedStream);
            }

            return resizedStream.ToArray();
        }
    }
}
