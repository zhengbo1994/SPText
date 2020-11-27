using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPTextCommon
{
    public class ImageHelper
    {
        private static ImageCodecInfo GetEncoderInfo(string mime_type)
        {
            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i <= imageEncoders.Length; i++)
            {
                if (imageEncoders[i].MimeType == mime_type)
                {
                    return imageEncoders[i];
                }
            }
            return null;
        }

        public static void SaveJpg(Image image, string file_name, int level)
        {
            try
            {
                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)level);
                ImageCodecInfo encoderInfo = ImageHelper.GetEncoderInfo("image/jpeg");


                File.Delete(file_name);
                image.Save(file_name, encoderInfo, encoderParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static MemoryStream SaveJpgToStream(Image image, int level)
        {
            try
            {
                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)level);
                ImageCodecInfo encoderInfo = ImageHelper.GetEncoderInfo("image/jpeg");
                MemoryStream strem = new MemoryStream();
                image.Save(strem, encoderInfo, encoderParameters);
                return strem;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int SaveJpgAtFileSize(Image image, string file_name, long max_size)
        {
            int result;
            try
            {
                for (int i = 100; i > 5; i -= 5)
                {
                    ImageHelper.SaveJpg(image, file_name, i);
                    if (ImageHelper.GetFileSize(file_name) <= max_size)
                    {
                        result = i;
                        return result;
                    }
                }
                result = 5;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static long GetFileSize(string file_name)
        {
            long length;
            try
            {
                length = new FileInfo(file_name).Length;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return length;
        }

        public static Bitmap ShrinkageImg(Image iSource, int maxWidth = 800, int maxHeight = 0)
        {
            int sW = iSource.Width, sH = iSource.Height;
            if (maxWidth != 0 && maxWidth < sW)
            {
                sW = maxWidth;
                sH = iSource.Height * maxWidth / iSource.Width;
            }
            else if (maxHeight != 0 && maxHeight < sH)
            {
                sH = maxHeight;
                sW = iSource.Width * maxHeight / iSource.Height;
            }
            Bitmap ob = new Bitmap(sW, sH);
            Graphics g = Graphics.FromImage(ob);
            try
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(iSource, new Rectangle(0, 0, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
                return ob;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                g.Dispose();
                iSource.Dispose();
            }

        }

        public static Bitmap LoadBitmap(string file_name)
        {
            Bitmap result;
            try
            {
                Bitmap bitmap2;
                using (Bitmap bitmap = new Bitmap(file_name))
                {
                    bitmap2 = new Bitmap(bitmap.Width, bitmap.Height);
                    using (Graphics graphics = Graphics.FromImage(bitmap2))
                    {
                        Rectangle rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                        graphics.DrawImage(bitmap, rectangle, rectangle, GraphicsUnit.Pixel);
                    }
                }
                result = bitmap2;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static byte[] ImageToBytes(Image image)
        {
            byte[] result;
            try
            {
                ImageFormat rawFormat = image.RawFormat;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    if (rawFormat.Equals(ImageFormat.Jpeg))
                    {
                        image.Save(memoryStream, ImageFormat.Jpeg);
                    }
                    else if (rawFormat.Equals(ImageFormat.Png))
                    {
                        image.Save(memoryStream, ImageFormat.Png);
                    }
                    else if (rawFormat.Equals(ImageFormat.Bmp))
                    {
                        image.Save(memoryStream, ImageFormat.Bmp);
                    }
                    else if (rawFormat.Equals(ImageFormat.Gif))
                    {
                        image.Save(memoryStream, ImageFormat.Gif);
                    }
                    else if (rawFormat.Equals(ImageFormat.Icon))
                    {
                        image.Save(memoryStream, ImageFormat.Icon);
                    }
                    byte[] array = new byte[memoryStream.Length];
                    memoryStream.Seek(0L, SeekOrigin.Begin);
                    memoryStream.Read(array, 0, array.Length);
                    result = array;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static byte[] ImageToBytes(string FilePath)
        {
            byte[] result;
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (Image image = Image.FromFile(FilePath))
                    {
                        using (Bitmap bitmap = new Bitmap(image))
                        {
                            bitmap.Save(memoryStream, image.RawFormat);
                        }
                    }
                    result = memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static Image BytesToImage(byte[] buffer, int maxWidth = 800, int maxHeight = 0)
        {
            Image result;
            try
            {
                MemoryStream stream = new MemoryStream(buffer);
                Image image = Image.FromStream(stream);
                result = ShrinkageImg(image, maxWidth, maxHeight);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public static Bitmap BytesToBitmap(byte[] Bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(Bytes);
                return new Bitmap((Image)new Bitmap(stream));
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
            }
        }

    }
}
