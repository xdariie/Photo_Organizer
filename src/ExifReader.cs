using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Imaging;
using System.Drawing;
using System.Globalization;

namespace Photo_Organization
{
    class ExifReader
    {
        public DateTime? GetDateTaken(string imagePath)
        {
            try
            {
                using Image image = Image.FromFile(imagePath);

                const int DateTakenId = 0x9003; //date photo was taken

                if (!image.PropertyIdList.Contains(DateTakenId)) return null;

                PropertyItem item = image.GetPropertyItem(DateTakenId);
                if (item == null) return null;
                string dateString = System.Text.Encoding.ASCII.GetString(item.Value!).Trim('\0');

                return DateTime.ParseExact(dateString, "yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture);

            }
            catch
            {
                return null;
            }
        }

        public (double Latitude, double Longitude)? GetGpsCoordinates(string imagePath)
        {
            try
            {
                using Image image = Image.FromFile(imagePath);

                const int GpsLatId = 0x0002; //means N or S
                const int GpsLatRefId = 0x0001; //means degrees/minutes/seconds
                const int GpsLonId = 0x0004; //means E or W
                const int GpsLonRefId = 0x0003; //means degrees/minutes/seconds

                if (!image.PropertyIdList.Contains(GpsLatId) || !image.PropertyIdList.Contains(GpsLonId)) return null;

                double latitude = ConvertToDegrees(image.GetPropertyItem(GpsLatRefId));
                double longitude = ConvertToDegrees(image.GetPropertyItem(GpsLonRefId));

                string latRef = GetAscii(image.GetPropertyItem(GpsLatRefId));
                string lonRef = GetAscii(image.GetPropertyItem(GpsLonRefId));
                if(latRef == "S") latitude = -latitude;
                if (lonRef == "W") longitude = -longitude;
                return (latitude, longitude);

            }
            catch
            {
                return null; 
            }
        }



        private double ConvertToDegrees(PropertyItem item)
        { 
            if (item.Value == null)
            {
                throw new ArgumentException("Invalid GPS EXIF data");
            }
            double degrees = ToRational(item.Value, 0);
            double minutes = ToRational(item.Value, 8);
            double seconds = ToRational(item.Value, 16);

            return degrees + minutes / 60.0 + seconds / 3600.0;
        }

        private double ToRational(byte[] bytes, int start)
        {
            if (bytes.Length < start + 8)
            {
                throw new ArgumentException("Invalid rational data");
            }
            uint numerator = BitConverter.ToUInt32(bytes, start);
            uint denominator = BitConverter.ToUInt32(bytes, start + 4);

            if (denominator == 0) return 0;

            return (double)numerator / denominator;
        }


        private string GetAscii(PropertyItem item)
        {
            if (item == null) return null;
            return System.Text.Encoding.ASCII.GetString(item.Value).Trim('\0');
        }
    }
}
