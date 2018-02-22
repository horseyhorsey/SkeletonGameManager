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
    }

    public class SkeletonGameExport : ISkeletonGameExport
    {       
        public void ExportLampsToLampshowUI(IEnumerable<PRLamp> lamps, string gameName, string exportFolder)
        {
            if (lamps == null) throw new NullReferenceException("Lamp collection cannot be null");

            ExportLampsToUi(lamps, gameName, exportFolder);
        }

        #region Private Methods

        /// <summary>
        /// Exports the lamps to Lampshow UI.
        /// </summary>
        /// <param name="lamps">The lamps.</param>
        /// <param name="exportFolder">The export folder. The game folder</param>
        private void ExportLampsToUi(IEnumerable<PRLamp> lamps, string gameName, string exportFolder)
        {
            var lampshowExportModel = CreatePrExportFromPrLamps(lamps);
            var json = JsonConvert.SerializeObject(lampshowExportModel);

            //Create the folder and config files for a lampshow UI
            var lampUiDir = Path.Combine(exportFolder, "LampShowUI", gameName);
            Directory.CreateDirectory(lampUiDir);

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
