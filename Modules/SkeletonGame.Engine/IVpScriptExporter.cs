using SkeletonGame.Models;
using SkeletonGame.Models.Machine;
using System;
using System.Collections.Generic;

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
    }

    public class VpScriptExporter : IVpScriptExporter
    {
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
                            if(!_switch.Name.Contains("flipper"))
                                if (!_switch.Number.Contains("SD"))
                                    {
                                        exportString += $"' {_switch.Name} hit {newLine}";
                                        exportString += $"Sub {_switch.Number.Replace("S", "sw")}_Hit(){newLine}";
                                        exportString += $"    Controller.Switch({_switch.Number.Remove(0, 1)}) = 1{newLine}";
                                        exportString += $"End Sub{newLine}";

                                        exportString += $"{newLine}";

                                        exportString += $"' {_switch.Name} unhit {newLine}";
                                        exportString += $"Sub {_switch.Number.Replace("S", "sw")}_UnHit(){newLine}";
                                        exportString += $"    Controller.Switch({_switch.Number.Remove(0, 1)}) = 0{newLine}";
                                        exportString += $"End Sub{newLine}";

                                        exportString += $"{newLine}";
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
    }
}
