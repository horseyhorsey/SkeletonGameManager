using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SkeletonGame.Engine
{
    public interface IVisualPinball
    {
        void LoadTable(string tableFile, bool play = true);
        string VpExecutable { get; set; }
    }

    public class VpLaunch : IVisualPinball
    {
        public string VpExecutable { get; set; }

        public VpLaunch(string vpExecutable)
        {
            VpExecutable = vpExecutable;
        }

        public void LoadTable(string tableFile, bool play = true)
        {
            Process vpExe = new Process();
            var tableWrap = "\"" + tableFile + "\"";
            var args = play == true ? $@"-play {tableWrap}" : $"{tableWrap}";
            ProcessStartInfo si = new ProcessStartInfo(VpExecutable, args);
            vpExe.StartInfo = si;
            vpExe.Start();
        }
    }
}
