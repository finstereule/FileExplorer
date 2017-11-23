using System;
using System.Text;
using System.IO;

namespace lab1
{
    public static class Logger
    {
        private static readonly string Filepath = Path.Combine(StaticResources.ClientLogDirPath,
            "App" + DateTime.Now.ToString("YYYY_MM_DD") + ".txt");

        private static void CheckAndCreateFile()
        {
            if (!Directory.Exists(StaticResources.ClientLogDirPath))
            {
                Directory.CreateDirectory(StaticResources.ClientLogDirPath);
            }
            if (!File.Exists(Filepath))
            {
                File.Create(Filepath).Close();
            }
        }

        public static void Log(string message)
        {
            StreamWriter writer = null;
            FileStream file = null;
            try
            {
                CheckAndCreateFile();
                file = new FileStream(Filepath, FileMode.Append);
                writer = new StreamWriter(file);
                writer.WriteLine(DateTime.Now.ToString("HH:mm:ss.ms") + " " + message);
            }
            catch
            {
            }
            finally
            {
                writer?.Close();
                file?.Close();
                writer = null;
                file = null;
            }
        }
        public static void Log(string message, Exception ex)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(message);
            while (ex != null)
            {
                stringBuilder.AppendLine(ex.Message);
                stringBuilder.AppendLine(ex.StackTrace);
                ex = ex.InnerException;
            }
            Log(stringBuilder.ToString());
        }

        public static void Log(Exception ex)
        {
            var stringBuilder = new StringBuilder();
            while (ex != null)
            {
                stringBuilder.AppendLine(ex.Message);
                stringBuilder.AppendLine(ex.StackTrace);
                ex = ex.InnerException;
            }
            Log(stringBuilder.ToString());
        }
    }
}
