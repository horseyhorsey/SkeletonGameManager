using SkeletonGame.Models;
using SkeletonGame.Models.Machine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SkeletonGame.Engine
{
    public interface IVpScriptExporter
    {
        /// <summary>
        /// Exports the machine values to script from the given type. Creates Sub routines for Visual Basic Script <para/>
        /// Switch exports export Hit and UnHit Routines - Coils exports a list of SolCallbacks and empty sub routines to match
        /// </summary>
        /// <param name="machineConfig">The machine configuration.</param>
        /// <param name="exportType">Type of the export.</param>
        /// <returns></returns>
        string ExportMachineValuesToScript(MachineConfig machineConfig, VpScriptExportType exportType);

        /// <summary>
        /// Creates the visual pinball script.
        /// </summary>
        /// <param name="romName">Name of the rom which is set to the cgamename in the script. Usually the game folders name</param>
        string CreateVisualPinballScript(MachineConfig config, string romName);
    }

    public class VpScriptExporter : IVpScriptExporter
    {
        private string _ballStackVariables = string.Empty;

        /// <summary>
        /// Creates a visual pinball script for the game
        /// </summary>
        /// <param name="romName">Name of the rom which is set to the cgamename in the script. Usually the game folders name</param>
        /// <exception cref="NotImplementedException"></exception>
        public string CreateVisualPinballScript(MachineConfig config, string romName)
        {
            var templateScript = GetTemplateScript();            

            string switchOptions = ExportMachineValuesToScript(config, VpScriptExportType.Switch);
            string coilOptions = ExportMachineValuesToScript(config, VpScriptExportType.Coil);

            string ballStackVars = string.Empty;
            List<string> ballStackScripts = new List<string>();

            var saucers = config.PRSwitches.Where(s => s.VpSwitchType == VpSwitchType.Saucer);
            var scoops = config.PRSwitches.Where(s => s.VpSwitchType == VpSwitchType.Scoop);
            var vuks = config.PRSwitches.Where(s => s.VpSwitchType == VpSwitchType.Vuk);

            ballStackVars += $"Dim bsTrough\r\n";
            ballStackScripts.Add(CreateTroughScript(config, "bsTrough"));

            foreach (var saucer in saucers)
            {
                ballStackVars += $"Dim bs{saucer.Name}\r\n";
                ballStackScripts.Add(CreateBallStackScript(saucer));
            }

            foreach (var scoop in scoops)
            {
                ballStackVars += $"Dim bs{scoop.Name}\r\n";
                ballStackScripts.Add(CreateBallStackScript(scoop));
            }

            foreach (var vuk in vuks)
            {
                ballStackVars += $"Dim bs{vuk.Name}\r\n";
                ballStackScripts.Add(CreateBallStackScript(vuk));
            }

            //Replace romname
            templateScript = templateScript.Replace("{ROMNAME}", $"{romName}");

            //Replace Dims for stacks
            templateScript = templateScript.Replace("{BALL_STACKS_VARS}", $"{_ballStackVariables}");

            //Replace vbs script
            templateScript = templateScript.Replace("{MACHINE_SCRIPT_TYPE}", $"{GetVpMachineScriptType(config.PRGame.MachineType)}");

            var vpInitTableStacks = string.Empty;
            foreach (var stack in ballStackScripts)
            {
                vpInitTableStacks += stack + Environment.NewLine;
            }

            //Add stacks to table init
            templateScript = templateScript.Replace("{TABLE_INIT_SECTION}", $"{vpInitTableStacks}");

            //Switch section
            templateScript = templateScript.Replace("{SWITCH_SECTION}", $"{ExportMachineValuesToScript(config, VpScriptExportType.Switch)}");

            //Solenoid section
            templateScript = templateScript.Replace("{SOLENOID_SECTION}", $"{ExportMachineValuesToScript(config, VpScriptExportType.Coil)}");

            return templateScript;

        }

        private string CreateTroughScript(MachineConfig config, string dimName)
        {
            var troughScript = string.Empty;

            var troughSwitches = config.PRSwitches.Where(x => x.Name.ToUpper().Contains("TROUGH")).Reverse();
            _ballStackVariables += $"Dim bsTrough\r\n";
            troughScript += $"Set {dimName} = New cvpmBallStack" + Environment.NewLine;

            troughScript += $"{dimName}.InitSw ";
            var troughSwitchList = "0";
            var cnt = 1;
            foreach (var trough in troughSwitches)
            {
                troughSwitchList += $", {trough.Number.Replace("S", "")}";
                cnt++;
            }

            var zeroFill = 8 - cnt;
            for (int i = 0; i < zeroFill; i++)
            {
                troughSwitchList += ", 0";
            }

            troughScript += troughSwitchList + Environment.NewLine;
            troughScript += $"{dimName}.InitKick BallRelease, 90, 8" + Environment.NewLine;
            troughScript += $"{dimName}.Balls = {config.PRGame.NumBalls}" + Environment.NewLine;
            var dimQuoted = "\"" + dimName + "\"";
            troughScript += $"{dimName}.CreateEvents {dimQuoted},  Drain" + Environment.NewLine;

            return troughScript;
        }

        private string GetTemplateScript()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "SkeletonGame.Engine.VpScript.script_template.vbs";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private string GetVpMachineScriptType(string machineType)
        {
            var procMachineType = Enum.Parse(typeof(MachineType), machineType.ToUpper());
            switch (procMachineType)
            {
                case MachineType.WPC95:
                case MachineType.WPC:
                    return "WPC.vbs";                
                case MachineType.WPCALPHANUMERIC:
                    return "S11.vbs";
                case MachineType.STERNSAM:
                    return "SAM.vbs";
                case MachineType.STERNWHITESTAR:
                    return "de.vbs";
                case MachineType.PDB:
                    return null;
            }

            return null;
        }

        /// <summary>
        /// Exports the machine values to script from the given type. Creates Sub routines for Visual Basic Script <para />
        /// Switch exports export Hit and UnHit Routines - Coils exports a list of SolCallbacks and empty sub routines to match
        /// </summary>
        /// <param name="machineConfig">The machine configuration.</param>
        /// <param name="exportType">Type of the export.</param>
        /// <returns></returns>
        public string ExportMachineValuesToScript(MachineConfig machineConfig, VpScriptExportType exportType)
        {
            string exportString = string.Empty;
            string newLine = Environment.NewLine;

            switch (exportType)
            {
                case VpScriptExportType.Switch:

                    exportString += $"' ** VP SWITCH EVENTS **  {newLine}";

                    foreach (var _switch in machineConfig.PRSwitches)
                    {
                        //Don't export trough switches, flippers and dedicated
                        if (!_switch.Name.Contains("trough"))
                            if (!_switch.Name.Contains("flipper"))
                                if (!_switch.Number.Contains("SD"))
                                {
                                    if (_switch.VpSwitchType == VpSwitchType.PulseSwitch)
                                    {
                                        exportString += $"' {_switch.Name} hit {newLine}";
                                        exportString += $"Sub {_switch.Number.Replace("S", "sw")}_Hit():vpmTimer.PulseSw {_switch.Number.Remove(0, 1)}:End Sub{newLine}";
                                    }                                    
                                    else if (_switch.VpSwitchType != VpSwitchType.Saucer || _switch.VpSwitchType != VpSwitchType.Vuk || _switch.VpSwitchType != VpSwitchType.Scoop)
                                    {
                                        exportString += $"' {_switch.Name} hit {newLine}";
                                        exportString += $"Sub {_switch.Number.Replace("S", "sw")}_Hit():Controller.Switch({_switch.Number.Remove(0, 1)}) = 1 :End Sub{newLine}";
                                        exportString += $"Sub {_switch.Number.Replace("S", "sw")}_UnHit():Controller.Switch({_switch.Number.Remove(0, 1)}) = 0 :End Sub{newLine}";
                                    }
                                }
                    }
                    break;
                case VpScriptExportType.Coil:

                    exportString += $"' ** VP SOL CALLBACKS **  {newLine}";

                    var solCallBackSubs = new List<string>();

                    foreach (var _coil in machineConfig.PRCoils)
                    {
                        if (!_coil.Name.Contains("flipper"))
                        {
                            //Create a string for vbscript
                            var solName = "Sol" + _coil.Name;
                            var solStr = "\"" + solName + "\"";

                            var solNumber = _coil.Number.Replace("C0", string.Empty);
                            solNumber = solNumber.Replace("C", string.Empty);

                            exportString += $"SolCallback({solNumber}) = {solStr}{newLine}";

                            var str = $"Sub {solName}(Enabled){newLine}If Enabled Then{newLine}'{newLine}Else{newLine}'{newLine}End If{newLine}End Sub{newLine}{newLine}";
                            solCallBackSubs.Add(str);
                        }

                    }

                    //Add a sub routine for the solcallback
                    exportString += $"{newLine}{newLine}";
                    foreach (var sub in solCallBackSubs)
                    {
                        exportString += sub;
                    }

                    break;
                default:
                    break;
            }

            return exportString;
        }

        /// <summary>
        /// Creates a ball stack script for use in the VBS constructor
        /// </summary>
        /// <returns></returns>
        private string CreateBallStackScript(PRSwitch prSwitch)
        {
            string ballStackScript = string.Empty;
            var vpSwitchNumber = prSwitch.Number.Replace("S", "");

            //Add to the Dim variables for top of script
            var dimName = $"bs{prSwitch.Name}";
            _ballStackVariables += $"Dim {dimName}\r\n";

            switch (prSwitch.VpSwitchType)
            {
                case VpSwitchType.Saucer:                                   
                    ballStackScript += $"Set {dimName}=New cvpmSaucer : With {dimName}" + Environment.NewLine;
                    ballStackScript += $".initKicker sw{vpSwitchNumber}, {vpSwitchNumber}, 80, 5" + Environment.NewLine;
                    ballStackScript += ".initSounds " + "\"" + "Solenoid" + "\"" + ", " + "\"" + "Solenoid" + "\"" + ", " + "\"" + "Solenoid" + "\"" + Environment.NewLine;
                    ballStackScript += ".CreateEvents " + "\"" + $"{dimName}" + "\"" + $", sw{vpSwitchNumber}" + Environment.NewLine;
                    ballStackScript += "End With" + Environment.NewLine;
                    break;
                case VpSwitchType.Scoop:
                    ballStackScript += $"Set {dimName}=New cvpmBallStack : With {dimName}" + Environment.NewLine;
                    ballStackScript += $".InitSw 0, {vpSwitchNumber}, 0, 0, 0, 0, 0, 0" + Environment.NewLine;
                    ballStackScript += $".InitKick sw{vpSwitchNumber}, 200, 20" + Environment.NewLine;
                    ballStackScript += $".KickZ = 0.3" + Environment.NewLine;
                    ballStackScript += $".KickForceVar = 1.5" + Environment.NewLine;
                    ballStackScript += ".InitExitSnd " + "\"" + "Solenoid" + "\"" + ", " + "\"" + "Solenoid" + "\"" + Environment.NewLine;
                    ballStackScript += ".CreateEvents " + "\"" + $"{dimName}" + "\"" + $", sw{vpSwitchNumber}" + Environment.NewLine;
                    ballStackScript += "End With" + Environment.NewLine;
                    break;
                case VpSwitchType.Vuk:
                    ballStackScript += $"Set {dimName}=New cvpmSaucer : With {dimName}" + Environment.NewLine;
                    ballStackScript += $".initKicker sw{vpSwitchNumber}, {vpSwitchNumber}, 80, 5" + Environment.NewLine;
                    ballStackScript += ".initSounds " + "\"" + "Solenoid" + "\"" + ", " + "\"" + "Solenoid" + "\"" + ", " + "\"" + "Solenoid" + "\"" + Environment.NewLine;
                    ballStackScript += ".CreateEvents " + "\"" + $"{dimName}" + "\"" + $", sw{vpSwitchNumber}" + Environment.NewLine;
                    ballStackScript += "End With" + Environment.NewLine;
                    break;
                default:
                    break;
            }
        
            return ballStackScript;
        }
    }
}
