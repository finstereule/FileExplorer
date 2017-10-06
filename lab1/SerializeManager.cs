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
        private static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static readonly string DirPath = Path.Combine(AppData, "lab1");

        private static string CreateAndGetPath(string filename)
        {
            if (!Directory.Exists(DirPath))
                Directory.CreateDirectory(DirPath);

            return Path.Combine(DirPath, filename);
        }

        public static void Serialize<TObject>(TObject obj) where TObject : ISerializable
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                var filename = CreateAndGetPath(obj.Filename);

                using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, obj);
                }
            }
            catch (Exception e)
            {
                // TODO Add logging
                throw;

            }
        }

        public static TObject Deserialize<TObject>(string filename) where TObject : ISerializable
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    return (TObject)formatter.Deserialize(fs);
                }
            }
            catch (Exception e)
            {
                // TODO add logging
                throw;
            }
        }
    }
}
