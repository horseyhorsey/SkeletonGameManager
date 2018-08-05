using SkeletonGameManager.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonGameManager.Module.Services
{
    public class SkeletonGameRunner : IGameRunnner
    {
        public Task Run(string gameFolder, string gameEntryFile)
        {
            //Check python 27 installed first
            string getUserEnv = Environment.GetEnvironmentVariable("path", EnvironmentVariableTarget.User);
            if (!getUserEnv.Contains(@"C:\Python27"))
                throw new FileNotFoundException(@"C:\Python27 python not found in your enviroment");

            //Build args to run the game.py with python
            var startInfo = new ProcessStartInfo("python");
            startInfo.WorkingDirectory = gameFolder;
            startInfo.Arguments = $"{gameEntryFile}";

            //Redirect just the error output...
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.UseShellExecute = false;

            return Task.Run(() =>
            {
                var p = new Process { StartInfo = startInfo };

                p.Start();
                //p.BeginOutputReadLine();
                p.BeginErrorReadLine();

                p.ErrorDataReceived += P_OutputDataReceived1;
                p.Exited += P_Exited;
                p.Disposed += P_Disposed;
                p.WaitForExit();
            });
        }

        private void P_Disposed(object sender, EventArgs e)
        {
            
        }

        private void P_Exited(object sender, EventArgs e)
        {
            
        }

        private void P_OutputDataReceived1(object sender, DataReceivedEventArgs e)
        {
            
        }
    }
}
