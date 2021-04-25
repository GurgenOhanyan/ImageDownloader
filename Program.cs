using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
namespace Images
{
    class Program
    {
        private static string directoryPath = @"C:\Users\User\OneDrive\Desktop\images";
        private const string Default_File_Name = "image.jpg";
        private static string imagesSourceName = @"C:\Users\User\source\repos\Images\sourceUrls.txt";
        static async Task Main(string[] args)
        {
            var urls = GetImageUrls(imagesSourceName);
            await MultipleDownloadAsync(urls);
            Console.ReadKey();
        }

        static string ExtractImageName(string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                return Default_File_Name;
            }

            int lastIndex = url.LastIndexOf('/');
            return url.Substring(lastIndex + 1);
        }
        static async Task StartDownload(string url)
        {
            ImageDownloader imageDownloader = new ImageDownloader();
            await imageDownloader.ImageDownloaderAsync(url);
            var di = Directory.CreateDirectory(directoryPath);

            
            string imageName = ExtractImageName(url);

            string filePath = Path.Combine(di.FullName, imageName);
     
            imageDownloader.SaveImage(filePath);
        }
        static IEnumerable<string> GetImageUrls(string fileName)
        {
            List<string> source = new List<string>();
            using (StreamReader sr = new StreamReader(fileName))
            {
                while (!sr.EndOfStream)
                {
                    source.Add(sr.ReadLine());
                }
            }

            return source;
        }

        private static async Task MultipleDownloadAsync(IEnumerable<string> urls)
        {
            ImageDownloader imageDownloader = new ImageDownloader();
            List<Task> allTasks = new List<Task>();
            foreach (string url in urls)
            {
                Task task = StartDownload(url);
                allTasks.Add(task);
            }
            await Task.WhenAll(allTasks);
        }
       
    }
}
