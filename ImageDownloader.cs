using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.IO;
using System.Drawing;
using System.Diagnostics;

namespace Images
{
    class ImageDownloader
    {
        private byte[] imageBytes;
        private Stopwatch stopwatch = new Stopwatch();
        public async Task ImageDownloaderAsync(string url)
        {
            stopwatch.Start();
            //Console.WriteLine("Starting image Download...");
            HttpClient httpClient = new HttpClient();
            this.imageBytes =await httpClient.GetByteArrayAsync(url);
            //Console.WriteLine("Succesfully Dolwnloaded image");
            stopwatch.Stop();
            Console.WriteLine($"This image took {stopwatch.ElapsedMilliseconds} milliseconds to download");
        }
       
        public void SaveImage(string filePath)
        {
            //Console.WriteLine("Saveing image...");
            stopwatch.Restart();
            using (Stream fr = new FileStream(filePath, FileMode.Create))
            {
                Bitmap bitmap = new Bitmap(RotateImage(this.imageBytes));
                //fr.Write(this.imageBytes, 0, imageBytes.Length);
                bitmap.Save(fr, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            //Console.WriteLine("Saved image...");
            stopwatch.Stop();
            Console.WriteLine($"This image took {stopwatch.ElapsedMilliseconds} milliseconds to saved");
        }
        private  Bitmap ConvertFromBytes(Byte[] imagebytes)
        {
           
            return new Bitmap(new MemoryStream(imagebytes));
        }
        private  Bitmap RotateImage(byte[] bytes)
        {
            stopwatch.Restart();  
            Bitmap bitmap = ConvertFromBytes(bytes);
            
           bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
            stopwatch.Stop();
            Console.WriteLine($"This image took { stopwatch.ElapsedMilliseconds} milliseconds to rotate");
           return bitmap;
        }
    }
}
