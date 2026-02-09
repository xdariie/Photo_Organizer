using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Formats.Tar;

namespace Photo_Organization
{
    class TarExtractor
    {
        private string tempPath = "./temp/extracted";
        public TarExtractor()
        {
            Directory.CreateDirectory(tempPath);
        }

        public void Extract(string tarFilePath)
        {
            if (!File.Exists(tarFilePath))
            {
                Console.WriteLine($"Tar file {tarFilePath} does not exist.");
                return;
            }
            if (!tarFilePath.EndsWith(".tar"))
            {
                return;
            }

            string extractFolder = Path.Combine(tempPath, Path.GetFileNameWithoutExtension(tarFilePath));

            if (Directory.Exists(extractFolder)) {
                Directory.Delete(extractFolder, true);
            }

            Directory.CreateDirectory(extractFolder);

            try {
            TarFile.ExtractToDirectory(tarFilePath, extractFolder, false);
                Console.WriteLine($"Extracted TAR: {tarFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting TAR: {ex.Message}");
            }
        }   
    }
}
