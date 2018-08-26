using Newtonsoft.Json;
using SkeletonGame.Models.Export;
using SkeletonGame.Models.Machine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SkeletonGame.Engine
{
    public interface ISkeletonGameExport
    {
        /// <summary>
        /// Exports the lamps to lampshow UI ready to be used there without manually creating all the lamps.
        /// </summary>
        /// <param name="lamps">The lamps.</param>
        void ExportLampsToLampshowUI(IEnumerable<PRLamp> lamps, string gameName, string exportFolder);

        /// <summary>
        /// Exports to pyprocgame pthon coded switch hits.
        /// </summary>
        /// <param name="switches">The switches.</param>
        /// <param name="exportFolder">The export folder. (the game root folder)</param>
        void ExportToPyprocgameSwitchHits(IEnumerable<PRSwitch> switches, string exportFolder);
    }

    public class SkeletonGameExport : ISkeletonGameExport
    {
        /// <summary>
        /// Exports the lamps to lampshow UI ready to be used there without manually creating all the lamps.
        /// </summary>
        /// <param name="lamps">The lamps.</param>
        /// <param name="gameName"></param>
        /// <param name="exportFolder"></param>
        /// <exception cref="NullReferenceException">Lamp collection cannot be null</exception>
        public void ExportLampsToLampshowUI(IEnumerable<PRLamp> lamps, string gameName, string exportFolder)
        {
            if (lamps == null) throw new NullReferenceException("Lamp collection cannot be null");

            ExportLampsToUi(lamps, gameName, exportFolder);
        }

        public void ExportToPyprocgameSwitchHits(IEnumerable<PRSwitch> switches, string exportFolder)
        {
            List<string> exportLines = new List<string>();

            foreach (var prSwitch in switches)
            {
                if (!prSwitch.Number.Contains("SD"))
                if (!prSwitch.Number.Contains("SF"))
                if (!prSwitch.Name.Contains("trough"))
                {
                    exportLines.Add($"\tdef sw_{prSwitch.Name}_active(self, sw):");
                    exportLines.Add($"\t\treturn procgame.game.SwitchContinue");
                }
            }

            using (var sw = File.CreateText(exportFolder + "\\pyprocgame_switch_hits.py"))
            {
                foreach (var line in exportLines)
                {
                    sw.WriteLine(line);
                }
            }
        }

        #region Private Methods

        /// <summary>
        /// Exports the lamps to Lampshow UI.
        /// </summary>
        /// <param name="lamps">The lamps.</param>
        /// <param name="exportFolder">The export folder. The game folder</param>
        private void ExportLampsToUi(IEnumerable<PRLamp> lamps, string gameName, string exportFolder)
        {
            var lampUiDir = Path.Combine(exportFolder, "LampShowUI", gameName);
            //TODO: Message like this should be in resource for localization.
            if (Directory.Exists(lampUiDir))
                throw new Exception("Cannot overwrite existing lampshow UI configurations. If this is what you want, delete the LampshowUI directory from the game folder.");

            var lampshowExportModel = CreatePrExportFromPrLamps(lamps);
            var json = JsonConvert.SerializeObject(lampshowExportModel);

            //Create the folder and config files for a lampshow UI
            Directory.CreateDirectory(lampUiDir);
            Directory.CreateDirectory(lampUiDir + "\\LedShows");

            //Create the LED config
            using (var sw = File.CreateText(Path.Combine(lampUiDir, $"{gameName}.json")))
            {
                sw.Write(json);
            }

            //Create a readme
            using (var sw = File.CreateText(lampUiDir + "\\readme.txt"))
            {
                sw.Write("Copy this folder to the lampshow UI Directory");
            }
        }

        private PRLampExport CreatePrExportFromPrLamps(IEnumerable<PRLamp> lamps)
        {
            PRLampExport export = new PRLampExport();
            export.PlayfieldImage = "pf.png";
            export.Groups = new List<Group>();
            export.Leds = new List<LED>();

            //Create some groups, with the needed unassigned leds
            export.Groups.Add(new Group
            {
                Leds = new List<int>(),
                Name = "Unassigned Leds"
            });
            export.Groups.Add(new Group
            {
                Leds = new List<int>(),
                Name = "Flashers"
            });

            //The current lamp id
            int id = 2;

            //Create a led list from the the lamps available
            foreach (var prLamp in lamps)
            {
                export.Leds.Add(new LED
                {
                    Id = id,
                    Name = prLamp.Name
                });

                export.Groups[0].Leds.Add(id);

                id++;
            }

            return export;
        }

        #endregion
    }
}
