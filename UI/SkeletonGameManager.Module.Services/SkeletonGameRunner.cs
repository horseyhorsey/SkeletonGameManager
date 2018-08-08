using SkeletonGameManager.Base;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SkeletonGameManager.Module.Services
{
    public class SkeletonGameRunner : IGameRunnner
    {
        public Task Run(string gameFolder, string gameEntryFile)
        {
            //Build args to run the game.py with python
            var startInfo = new ProcessStartInfo("tools\\sg_runner.exe");
            startInfo.WorkingDirectory = Environment.CurrentDirectory;

            string gamePath = $"\"{gameFolder}\"";
            string gameFile = $"\"{gameEntryFile}\"";

            startInfo.Arguments = $"{gamePath} {gameFile}";

            //Redirect just the error output...
            startInfo.CreateNoWindow = false;

            startInfo.RedirectStandardOutput = false;
            startInfo.RedirectStandardError = false;
            startInfo.RedirectStandardInput = false;
            startInfo.UseShellExecute = false;

            return Task.Run(() =>
            {
                var p = new Process { StartInfo = startInfo };

                //p.OutputDataReceived += P_OutputDataReceived1;
                //p.ErrorDataReceived += P_OutputDataReceived;
                //p.Exited += P_Exited;
                //p.Disposed += P_Disposed;
                
                p.Start();
                p.WaitForExit();
            });
        }
    }
}
