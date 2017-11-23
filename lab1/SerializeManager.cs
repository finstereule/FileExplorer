using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace lab1
{
    internal interface ISerializable
    {
        string Filename { get; }
    }

    internal static class SerializeManager
    {
        //  private static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); //отримує шлях до папки
        // private static readonly string DirPath = Path.Combine(AppData, "lab1");
        private static readonly string DirPath = StaticResources.ClientDirPath;

        private static string CheckAndCreatePath(string filename)
        {
            if (!Directory.Exists(StaticResources.ClientDirPath))
                Directory.CreateDirectory(StaticResources.ClientDirPath);

            return Path.Combine(StaticResources.ClientDirPath, filename);
        }

        public static void Serialize<TObject>(TObject obj, string fileName)
        {
            try
            {
                var formatter = new BinaryFormatter();
                var filename = CheckAndCreatePath(fileName);

                using (var fs = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, obj);
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to serialize object", ex);
            }
        }
        public static TObject Deserialize<TObject>(string filename)
        {
            try
            {
                var formatter = new BinaryFormatter();
                using (var fs = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    return (TObject)formatter.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to deserialize object", ex);
                throw;
            }
        }
    }
}
