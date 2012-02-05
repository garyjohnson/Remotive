using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace MediaControl.Host
{
    internal static class LogUtility
    {
        private static readonly object syncObject = new object();
        private static string logPath = string.Empty;
        private static void CreateLogFile()
        {
            // not thread safe, just for test
            if (string.IsNullOrEmpty(logPath))
            {
                // Create a log file
                DirectoryInfo pathInfo = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar + "MediaControl");
                if (!pathInfo.Exists)
                {
                    pathInfo.Create();
                }

                FileInfo logFile = new FileInfo(pathInfo.FullName + Path.DirectorySeparatorChar + "logfile.txt");
                if (!logFile.Exists)
                {
                    logFile.Create();
                }

                logPath = logFile.FullName;
            }
        }

        public static void LogException(Exception ex)
        {
            CreateLogFile();

            if (!string.IsNullOrEmpty(logPath))
            {
                using (StreamWriter writer = new StreamWriter(logPath, true))
                {
                    writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    writer.WriteLine(ex.Message);
                    writer.WriteLine(ex.Source);
                    writer.WriteLine(ex.TargetSite);
                    writer.WriteLine(ex.StackTrace);

                    StackTrace trace = new StackTrace(ex);
                    if (trace.FrameCount > 0)
                    {
                        StackFrame frame = trace.GetFrame(0);
                        if (frame != null)
                        {
                            MethodBase method = frame.GetMethod();
                            writer.WriteLine(string.Format("{0}, Line {1}", method.Name, frame.GetFileLineNumber()));
                        }
                    }

                    writer.WriteLine();
                }

                if (ex.InnerException != null)
                {
                    LogException(ex.InnerException);
                }
            }
        }

        public static void LogMessage(string message)
        {
            CreateLogFile();

            if (!string.IsNullOrEmpty(logPath))
            {
                using (StreamWriter writer = new StreamWriter(logPath, true))
                {
                    writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    writer.WriteLine("");
                }
            }
        }
    }
}
