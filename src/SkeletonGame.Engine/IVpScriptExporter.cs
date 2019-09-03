﻿using SkeletonGame.Models;
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

        #region Public Methods
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

            if (config.PRLamps.Where(x => x.Name.ToUpper() != "NOT USED").Count() > 0)
                templateScript = templateScript.Replace("{LAMP_SECTION}", GetLampScriptSection());
            else
            {
                templateScript = templateScript.Replace("{LAMP_SECTION}", string.Empty);
            }

            if (config.PRGame.MachineType.ToUpper().Contains("WPC"))
                templateScript = templateScript.Replace("{GI_SECTION}", GetGIScriptSection());
            else
                templateScript = templateScript.Replace("{GI_SECTION}", string.Empty);

            if (config.PRGame.MachineType.ToUpper().Contains("PDB"))
                templateScript = templateScript.Replace("{LED_SECTION}", GetLedScriptSection());
            else
                templateScript = templateScript.Replace("{LED_SECTION}", string.Empty);

            return templateScript;
        }

        /// <summary>
        /// Exports the machine values to script from the given type. Creates Sub routines for Visual Basic Script <para />
        /// </summary>
        /// <remarks>
        /// Switch exports > export Hit and UnHit Routines - Coils exports a list of SolCallbacks and empty sub routines to match.
        /// VPSwitchTypes set to None won't be exported. This is used on things like the coin door, coins, service menu because the PDB.vbs assigns these to VP.
        /// </remarks>
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
                        if (_switch.VpSwitchType == VpSwitchType.None) continue;

                        //Don't export trough switches, flippers and dedicated
                        if (_switch.Name.ToUpper() != "NOT USED")
                            if (!_switch.Name.Contains("trough"))
                                if (!_switch.Name.Contains("flipper"))
                                    if (machineConfig.GetMachineType() == MachineType.PDB)
                                    {
                                        exportString += GetVpSwitchMethod(_switch, machineConfig.GetMachineType());
                                    }
                                    else if (!_switch.Number.Contains("SD"))
                                    {
                                        exportString += GetVpSwitchMethod(_switch, machineConfig.GetMachineType());
                                    }
                    }

                    break;
                case VpScriptExportType.Coil:

                    exportString += $"' ** VP SOL CALLBACKS **  {newLine}";
                    var solCallBackSubs = new List<string>();
                    var machineType = machineConfig.GetMachineType();

                    foreach (var _coil in machineConfig.PRCoils)
                    {
                        if (_coil.Name.ToUpper() != "NOT USED")
                        {
                            if (machineType != MachineType.PDB && _coil.Name.Contains("flipper"))
                            {
                                continue;
                            }

                            // Don't add Main flippers just the hold coil
                            if (_coil.Name.Contains("LMain") || _coil.Name.Contains("RMain"))
                                continue;

                            var solName = "Sol" + _coil.Name;
                            if (_coil.Name.Contains("trough"))
                            {
                                exportString += GetVpSolCallback(_coil, machineConfig.GetMachineType(), "bstrough.SolOut");
                            }
                            else
                            {
                                //SolCallback entry
                                exportString += GetVpSolCallback(_coil, machineConfig.GetMachineType());

                                //Create a callback method
                                var str = $"Sub {solName}(Enabled){newLine}If Enabled Then{newLine}'{newLine}Else{newLine}'{newLine}End If{newLine}End Sub{newLine}{newLine}";
                                solCallBackSubs.Add(str);
                            }                            
                        }

                    }

                    //Add all the sub routine for the solcallback
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
        #endregion

        #region Private methods
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
                    ballStackScript += $"Set {dimName}=New cvpmBallStack : With {dimName}" + Environment.NewLine;
                    ballStackScript += $".initKicker sw{vpSwitchNumber}, {vpSwitchNumber}, 80, 5" + Environment.NewLine;
                    //ballStackScript += ".initSounds " + "\"" + "Solenoid" + "\"" + ", " + "\"" + "Solenoid" + "\"" + ", " + "\"" + "Solenoid" + "\"" + Environment.NewLine;
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
                    ballStackScript += $"Set {dimName}=New cvpmBallStack  : With {dimName}" + Environment.NewLine;
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
            //troughScript += $"{dimName}.InitEntrySnd "Drain", "Solenoid"
            //bsTrough.InitExitSnd "popper_ball", "Solenoid"

            return troughScript;
        }

        private string GetLampScriptSection()
        {
            string exportString = string.Empty;

            exportString += $"Const UseLamps = True ' This enables the Core.vbs to update {Environment.NewLine}";            
            var quote = "\"";
            exportString += $"Set LampCallback = GetRef({quote}UpdateMultipleLamps{quote}) ' This empty callback uses Core.vbs do the lamp updates";
            exportString += $"Sub UpdateMultipleLamps : End Sub ' Use this to do other things when lamps changed";

            return exportString;
        }

        private string GetLedScriptSection()
        {
            string exportString = string.Empty;
            exportString += $"Const UsePdLeds = True ' This enables the Core.vbs to update Leds {Environment.NewLine}";
            var quote = "\"";
            exportString += $"Set PDLedCallback = GetRef({quote}UpdateLeds{quote}) ' This empty callback uses Core.vbs do the lamp updates";
            exportString += $"Sub UpdateLeds : End Sub ' Use this to do other things when leds changed";
            return exportString;
        }

        private string GetGIScriptSection()
        {
            string exportString = string.Empty;
            var quote = "\"";
            exportString += $"Dim GIon, lamp ' Williams has GI you can use from proc{Environment.NewLine}";
            exportString += $"Set GICallback = GetRef({quote}UpdateGI{quote}) ' This empty callback uses Core.vbs do the lamp updates";
            exportString += $"Sub UpdateGI(giNo, stat)";
            exportString += $"  Select Case giNo{Environment.NewLine}    Case 0 GI_low(abs(stat)){Environment.NewLine}";
            exportString += $"    Case 1 GI_mid(abs(stat)){Environment.NewLine}    Case 2 GI_hi(abs(stat)){Environment.NewLine}  End Select";
            exportString += $"End Sub{Environment.NewLine}{Environment.NewLine}";

            exportString += $"Sub GI_low(enabled) : End Sub{Environment.NewLine}";
            exportString += $"Sub GI_mid(enabled) : End Sub{Environment.NewLine}";
            exportString += $"Sub GI_hi(enabled) : End Sub{Environment.NewLine}";


            return exportString;
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
                    return "PDB.vbs";
            }

            return null;
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

        private string GetVpSolCallback(PRCoil prCoil, MachineType procMachineType, string solString = null)
        {
            ///var exportString = $"' {pRSwitch.Name} hit {Environment.NewLine}";

            //Create a string for vbscript
            var solName = "Sol" + prCoil.Name;
            var solStr = "\"" + solName + "\"";
            string solNumber = null;

            if (procMachineType != MachineType.PDB)
            {
                solNumber = prCoil.Number.Replace("C0", string.Empty);
                solNumber = solNumber.Replace("C", string.Empty);
            }
            else
            {
                //This only good if exporting from the UI
                solNumber = prCoil.Number.Split(':')[0];
                if (solNumber.StartsWith("0"))
                    solNumber = solNumber.Remove(0, 1);
            }

            solStr = string.IsNullOrWhiteSpace(solString) ? solStr : "\"" + solString + "\""; 

            return $"SolCallback({solNumber}) = {solStr}{Environment.NewLine}";
        }

        /// <summary>
        /// Gets a VP scripted switch method.
        /// </summary>
        /// <param name="pRSwitch">The Machines switch</param>
        /// <param name="procMachineType">Type of the PROC Board.</param>
        /// <returns>String that contains a Hit() and UnHit() method invoking the Controller.Switch(num). If setting VP Switch type the controllers invocation will be different.</returns>
        private string GetVpSwitchMethod(PRSwitch pRSwitch, MachineType procMachineType)
        {
            var exportString = $"' {pRSwitch.Name} hit {Environment.NewLine}";
            string swNumber = null;
            string swName = pRSwitch.Name;

            //Get switch number with out any chars. Depending on machine type. Different with a PDB
            if (procMachineType != MachineType.PDB)
            {
                swNumber = pRSwitch.Number.Replace("S", string.Empty);
            }
            else
            {
                swNumber = pRSwitch.Number.Replace("SD", string.Empty);
                if (swNumber.StartsWith("0"))
                    swNumber = swNumber.Remove(0, 1);
            }

            // VP Pulse Switches
            if (pRSwitch.VpSwitchType == VpSwitchType.PulseSwitch)
            {
                if (procMachineType != MachineType.PDB)
                {
                    exportString += $"Sub sw{swNumber}_Hit():vpmTimer.PulseSw {swNumber}:End Sub{Environment.NewLine}";
                }
            }
            // VP Pulse Spinners
            else if (pRSwitch.VpSwitchType == VpSwitchType.Spinner)
            {
                if (procMachineType != MachineType.PDB)
                {
                    exportString += $"Sub sw{swNumber}_Spin():vpmTimer.PulseSw {swNumber}:End Sub{Environment.NewLine}";
                }
            }
            // Anything but BallStacks / Saucers
            else if (pRSwitch.VpSwitchType != VpSwitchType.Saucer || pRSwitch.VpSwitchType != VpSwitchType.Vuk || pRSwitch.VpSwitchType != VpSwitchType.Scoop)
            {
                exportString += $"Sub sw{swNumber}_Hit():Controller.Switch({swNumber}) = 1 :End Sub{Environment.NewLine}";
                exportString += $"Sub sw{swNumber}_UnHit():Controller.Switch({swNumber}) = 0 :End Sub{Environment.NewLine}";
            }
            return exportString;
        } 
        #endregion
    }
}
