﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RoboHash.Net.Interfaces;
using RoboHash.Net.Internals;

namespace RoboHash.Net
{
    public class RoboHash : RoboHashBase<ImageSource>
    {
        private static readonly IRoboHashImageFileProvider _imageFileProvider;
        private static readonly IRoboHashDigestGenerator _digestGenerator;

        static RoboHash()
        {
            _imageFileProvider = new DefaultImageFileProvider();
            _digestGenerator = new DefaultDigestGenerator();
        }

        /// <summary>
        /// Creates a robohash from the given text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static RoboHash Create(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            return Create(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Creates a robohash from the given byte array.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static RoboHash Create(byte[] bytes, int offset, int length)
        {
            using (var memory = new MemoryStream(bytes, offset, length))
                return Create(memory);
        }

        /// <summary>
        /// Creates a robohash from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static RoboHash Create(Stream stream)
        {
            var hexDigest = _digestGenerator.GenerateHexDigest(stream);
            return new RoboHash(hexDigest, _imageFileProvider);
        }

        public RoboHash(string hexDigest, IRoboHashImageFileProvider imageFileProvider)
            : base(hexDigest, imageFileProvider) { }


        /*
        protected override Image RenderFiles(IEnumerable<string> srcFiles, int srcWidth, int srcHeight, int destWidth, int destHeight)
        {
            var retval = new Bitmap(srcWidth, srcHeight);
            using (var canvas = Graphics.FromImage(retval))
            {
                canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                foreach (var imageFile in srcFiles)
                {
                    using (var image = Image.FromFile(imageFile))
                        canvas.DrawImage(image, new Rectangle(0, 0, srcWidth, srcHeight),
                            new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                }
                canvas.Save();
            }

            if (srcWidth != destWidth || srcHeight != destHeight)
            {
                var resizedImage = new Bitmap(retval, destWidth, destHeight);
                retval.Dispose();

                retval = resizedImage;
            }

            return retval;
        }*/
        protected override ImageSource RenderFiles(IEnumerable<string> srcFiles, string backgroundColor, int srcWidth, int srcHeight, int destWidth, int destHeight, Options options)
        {
            var retval = new WriteableBitmap(srcWidth, srcHeight);
            if (backgroundColor != null)
            {
                var color = RoboHelper.ConvertHexColor(backgroundColor);
                retval.FillRectangle(0, 0, srcHeight, srcHeight, color);
            }

            foreach (var imageFile in srcFiles)
            {
                var image = new BitmapImage();
                using (var fs = new FileStream(imageFile, FileMode.Open))
                    image.SetSource(fs);
                retval.Blit(new Rect(0, 0, srcWidth, srcHeight), new WriteableBitmap(image), new Rect(0, 0, image.PixelWidth, image.PixelHeight));

                //TOOD implement options
            }
            return retval;
        }
    }
}
