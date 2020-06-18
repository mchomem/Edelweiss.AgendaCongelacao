using System;
using System.Configuration;
using System.IO;

namespace Edelweiss.Utils
{
    public class Log
    {
        #region Properties

        private static String DefaultPath
        {
            get { return @"C:"; }
        }

        #endregion

        #region Methods

        public static void Create(String value)
        {
            WriteFile(value);
        }

        public static void Create(Exception e)
        {
            String message = String.Format("[Hostname]: {0}\r\n[Source]: {1}\r\n[Exception]: {2}\n\r[InnerException]: {3}\r\n[Stacktrace]: {4}"
                , Environment.MachineName
                , e.Source
                , e.Message
                , e.InnerException
                , e.StackTrace);

            WriteFile(message);
        }

        #endregion

        #region Auxiliary methods

        private static String GetPath()
        {
            String path = String.Empty;
            String file = "App.log";

            if (ConfigurationManager.AppSettings["APP_LOG_FILE_PATH"] != null)
            {
                path = ConfigurationManager.AppSettings["APP_LOG_FILE_PATH"];
            }
            else
            {
                path = DefaultPath;
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path += "\\" + file;
        }

        private static void WriteFile(String message)
        {
            using (StreamWriter sw = new StreamWriter(GetPath(), true))
            {
                sw.WriteLine(String.Empty.PadRight(200, '-'));
                sw.WriteLine(String.Format("[Date/time]: {0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
                sw.WriteLine(message);
                sw.WriteLine(String.Empty.PadRight(200, '-'));
                sw.WriteLine();
            }
        }

        #endregion
    }
}
