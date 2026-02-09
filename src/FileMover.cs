using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Photo_Organization
{
    internal class FileMover
    {
        public void MoveFile(string sourceFilePath, string targetDirectory)
        {
            if (!File.Exists(sourceFilePath))
            {
                Console.WriteLine($"File does not exist: {sourceFilePath}");
                return;
            }

            if (!Directory.Exists(targetDirectory))
            {
                Console.WriteLine($"Directory does not exist: {targetDirectory}");
                return;
            }

            string filename = Path.GetFileName(sourceFilePath);
            string targetFilePath = Path.Combine(targetDirectory, filename);

            try
            {
                if (File.Exists(targetFilePath))
                {
                    targetFilePath = GetUniqueFilePath(targetDirectory, filename);
                }

                File.Move(sourceFilePath, targetFilePath);
                Console.WriteLine($"Moved file to: {targetFilePath}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error moving file: {ex.Message}");
            }
        }



        private string GetUniqueFilePath(string directory, string fileName)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            int counter = 1;

            string newPath;

            do
            {
                string newFileName = $"{name}_{counter}{extension}";
                newPath = Path.Combine(directory, newFileName);
                counter++;
            } while (File.Exists(newPath));

            return newPath;
        }
    }
}
