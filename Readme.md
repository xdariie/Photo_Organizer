This application is intended to run on Windows. It relies on System.
Drawing for EXIF metadata processing, which is only fully supported on Windows.

FileScanner.cs performs an initial recursive scan of the monitored 
directory and detects files that already exist before folder monitoring 
starts.

FolderWatcher.cs uses the built-in FileSystemWatcher class to monitor  
a selected directory and its subdirectories. It detects newly created  
files and notifies the application so they can be processed further.

ZipExtractor.cs handles ZIP archives by extracting their contents into 
a temporary directory so that the contained image files can be processed 
further.

TarExtractor.cs handles TAR archives by extracting their contents into
a temporary directory so that the contained image files can be processed
further.

The ExifReader.cs module is responsible for reading EXIF metadata from image 
files in order to obtain the photo capture date and GPS coordinates. EXIF 
metadata is a standard format used by digital cameras to store additional 
information inside photos.
The capture date is read from the EXIF tag DateTimeOriginal. If this tag 
is present, its value is converted into a DateTime object; otherwise, the 
method returns null, which allows the program to handle images without 
EXIF data.
GPS coordinates are stored in EXIF as three rational values representing 
degrees, minutes, and seconds, along with separate reference tags that 
specify the direction (north/south and east/west). These values are 
converted into decimal latitude and longitude. If GPS information is 
missing, the location is treated as unknown.

NominatimClient.cs converts GPS coordinates into country and city names 
using the Nominatim reverse geocoding service. It sends an 
asynchronous HTTP request, parses the JSON response, and safely returns 
location data or "Unknown" if the information is unavailable.

FolderBuilder.cs creates the directory structure based on the photo 
capture date and location using the format year/month/day/country/city.

FileMover.cs moves image files into the target directory created by the 
program and prevents overwriting existing files by generating unique filenames.

Program.cs contains the main logic of the application. The program sets the path 
to the input_photos directory and starts by scanning all files that already 
exist in this folder using FileScanner.
After that, a FolderWatcher is launched to monitor the same directory for new 
files added while the program is running. Both existing and newly detected 
files are passed to the same ProcessFile method.
The ProcessFile method handles ZIP and TAR archives, reads EXIF metadata from 
image files, determines the photo date and location (using Nominatim if GPS 
data is available), creates the required folder structure, and moves the file 
to its final destination.
The program runs until the user presses ENTER, then stops monitoring and exits.