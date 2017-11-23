using System;
using System.IO;

namespace lab1
{
    static class StaticResources //logs are in "C:\Users\*\AppData\Roaming\FileExplorer\Logs"
    {
        private static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        internal static readonly string ClientDirPath = Path.Combine(AppData, "FileExplorer");
        internal static readonly string ClientLogDirPath = Path.Combine(ClientDirPath, "Logs");
       
    }
}
