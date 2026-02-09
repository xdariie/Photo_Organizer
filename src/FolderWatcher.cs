using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Photo_Organization
{
    internal class FolderWatcher
    {
        private FileSystemWatcher watcher;

        public event Action<string>? OnFileDetected;
        public FolderWatcher(string path)
        {
            watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = false;

            watcher.Created += OnFileCreated;
        }

        public void Start()
        {
            watcher.EnableRaisingEvents = true;
            Console.WriteLine("Folder watcher started.");
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            Console.WriteLine("Folder watcher stopped.");
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"New file detected: {e.FullPath}");
            OnFileDetected?.Invoke(e.FullPath);

        }
    }
}
