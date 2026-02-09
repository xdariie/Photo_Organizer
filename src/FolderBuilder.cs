using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Photo_Organization
{
    class FolderBuilder
    {
        private string baseOutputPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "organized"));

        public FolderBuilder()
        {
            Directory.CreateDirectory(baseOutputPath);
        }

        public string BuildPath(DateTime date, string country, string city)
        {
            string year = date.Year.ToString();
            string month = date.Month.ToString("D2");
            string day = date.Day.ToString("D2");

            if (string.IsNullOrWhiteSpace(country))
            {
                country = "unknown";
            }

            if (string.IsNullOrWhiteSpace(city))
            {
                city = "unknown";
            }

            string finalPath = Path.Combine(baseOutputPath, year, month, day, country, city);

            Directory.CreateDirectory(finalPath);

            return finalPath;
        }

    }
}
