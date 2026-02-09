using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace Photo_Organization
{
    class ZipExtractor
    {
        private string tempPath = "./temp/extracted";
        public ZipExtractor()
        {
            Directory.CreateDirectory(tempPath);
        }

        public void Extract(string zipFilePath)
        {
            if (!File.Exists(zipFilePath))
            {
                Console.WriteLine($"Zip file {zipFilePath} does not exist.");
                return;

            }

            if (!zipFilePath.EndsWith(".zip"))
            {
                return;
            }

            string extractFolder = Path.Combine(tempPath, Path.GetFileNameWithoutExtension(zipFilePath));

            try
            {
                ZipFile.ExtractToDirectory(zipFilePath, extractFolder);
                Console.WriteLine($"Extracted ZIP: {zipFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting ZIP: {ex.Message}");
            }
        }
    }
}
