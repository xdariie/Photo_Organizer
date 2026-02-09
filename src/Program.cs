using Photo_Organization;
using System;
namespace Photo_Organization
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../src/data/input_photos/"));
            Console.WriteLine("INPUT PATH = " + inputPath);
            Console.WriteLine("Photo Organizer started.");

            FileScanner scanner = new FileScanner(inputPath);
            scanner.OnFileDetected += path => _ = ProcessFile(path);
            scanner.Scan();

            FolderWatcher watcher = new FolderWatcher(inputPath);
            watcher.OnFileDetected += path => _ = ProcessFile(path);
            watcher.Start();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();

            watcher.Stop();
        }

        static async Task ProcessFile(string filePath)
        {
            Console.WriteLine("Processing: " + filePath);

            

            ZipExtractor zipExtractor = new ZipExtractor();
            TarExtractor tarExtractor = new TarExtractor();
            ExifReader exifReader = new ExifReader();
            NominatimClient nominatim = new NominatimClient();
            FolderBuilder folderBuilder = new FolderBuilder();
            FileMover fileMover = new FileMover();

            if (filePath.EndsWith(".zip"))
            {
                zipExtractor.Extract(filePath);
                return;
            }

            if (filePath.EndsWith(".tar"))
            {
                tarExtractor.Extract(filePath);
                return;
            }

            DateTime date = exifReader.GetDateTaken(filePath) ?? File.GetCreationTime(filePath);

            var gps = exifReader.GetGpsCoordinates(filePath);

            string country = "unknown";
            string city = "unknown";

            if (gps != null)
            {
                var location = await nominatim.GetLocationAsync(gps.Value.Latitude, gps.Value.Longitude);

                country = location.Country;
                city = location.City;
            }

            string targetPath = folderBuilder.BuildPath(date, country, city);
            fileMover.MoveFile(filePath, targetPath);
        }


    }
}
