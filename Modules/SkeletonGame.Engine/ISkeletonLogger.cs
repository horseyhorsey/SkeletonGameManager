using System;
using System.Collections.Generic;
using System.IO;

namespace SkeletonGame.Engine
{
    public interface ISkeletonLogger
    {
        IList<string> LogData { get; set; }

        void LogToFile(string logPath);
    }

    public class SkeletonLogger : ISkeletonLogger
    {
        public IList<string> LogData { get; set; } = new List<string>();

        /// <summary>
        /// Logs the held data in the LogData to file.
        /// </summary>
        /// <param name="logPath">The log path.</param>
        public void LogToFile(string logPath)
        {
            Directory.CreateDirectory(logPath);

            using (var sw = File.CreateText(Path.Combine(logPath, DateTime.Now.ToFileTime() + ".log")))
            {
                foreach (var logLine in LogData)
                {
                    sw.WriteLine(logLine);
                }
            }
        }
    }
}
