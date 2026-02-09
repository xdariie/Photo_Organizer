using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Photo_Organization
{
    class FileScanner
    {
        private string rootPath;

        public event Action<string>? OnFileDetected;
        public FileScanner(string path)
        {
            rootPath = path;

        }

        public void Scan()
        {
            if (!Directory.Exists(rootPath))
            {
                Console.WriteLine($"Directory {rootPath} does not exist.");
                return;
            }

            ScanDirectory(rootPath);
        }

        private void ScanDirectory(string path)
        {
            try
            {
                foreach(string file in Directory.GetFiles(path))
                {
                    Console.WriteLine("Found file: " + file);
                    OnFileDetected?.Invoke(file);
                }

                foreach(string dir in Directory.GetDirectories(path))
                {
                    ScanDirectory(dir);
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error while scanning: {ex.Message}");
            }
        }
    }
}
