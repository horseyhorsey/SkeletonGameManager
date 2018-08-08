using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonGame.ConsoleApp
{
    class Program
    {
        private static string _gameFolder = string.Empty;
        private static IList<string> LogData { get; set; } = new List<string>();

        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                    throw new ArgumentNullException();

                //Run game with python
                Run(args[0], args[1]);

                LogToFile(Path.Combine(_gameFolder, "logs"));
            }
            catch (Exception)
            {
                PrintHelp();
                Console.ReadLine();
            }
        }       
        
        static void PrintHelp()
        {
            Console.WriteLine("".PadLeft(20, '*'));
            Console.WriteLine("Instructions \n\r".PadLeft(5, '*').PadRight(5, '*'));
            Console.WriteLine("sg_runner 'Game Path' 'GameEntryFile'");
            Console.WriteLine("".PadLeft(20, '*'));
        }

        static void Run(string gameFolder, string gameEntryFile)
        {
            _gameFolder = gameFolder;

            //Check python 27 installed first
            string getUserEnv = Environment.GetEnvironmentVariable("path", EnvironmentVariableTarget.User);
            if (!getUserEnv.Contains(@"C:\Python27"))
                throw new FileNotFoundException(@"C:\Python27 python not found in your enviroment");

            //Build args to run the game.py with python
            var startInfo = new ProcessStartInfo("python");
            startInfo.WorkingDirectory = gameFolder;
            startInfo.Arguments = $"{gameEntryFile}";

            //Redirect just the error output...
            startInfo.CreateNoWindow = false;

            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = false;
            startInfo.UseShellExecute = false;

            var p = new Process { StartInfo = startInfo };

            p.OutputDataReceived += P_OutputDataReceived1;
            p.ErrorDataReceived += P_OutputDataReceived1;
            p.Exited += P_Exited;
            p.Disposed += P_Disposed;

            p.Start();
            //Console.WriteLine(p.StandardOutput.ReadToEnd());
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();

            p.WaitForExit();
        }

        static void P_OutputDataReceived1(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
            LogData.Add(e.Data);
            //_gameFolder
        }

        static void P_Disposed(object sender, EventArgs e)
        {

        }

        static void P_Exited(object sender, EventArgs e)
        {

        }

        static void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
            LogData.Add(e.Data);
        }
        

        /// <summary>
        /// Logs the held data in the LogData to file.
        /// </summary>
        /// <param name="logPath">The log path.</param>
        static void LogToFile(string logPath)
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
