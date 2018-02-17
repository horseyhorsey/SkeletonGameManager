using SkeletonGame.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace SkeletonGame.Engine
{
    public static class RecordingManager
    {
        #region Consts
        /// <summary>
        /// The basicgame class. Default class for SkeletonGame
        /// </summary>
        const string BASICGAME_CLASS = @"class SkeletonGame(BasicGame):";

        /// <summary>
        /// The basicrecordable game class name for recording with SkeletonGame
        /// </summary>
        const string BASICRECORDABLE_GAME_CLASS = @"class SkeletonGame(BasicRecordableGame):";

        /// <summary>
        /// The fakepinproc playback class for the config.yaml setting
        /// </summary>
        const string FAKEPINPROC_PLAYBACK_CLASS = @"pinproc_class: procgame.fakepinproc.FakePinPROCPlayback";

        /// <summary>
        /// The fakepinproc class for the config.yaml setting
        /// </summary>
        const string FAKEPINPROC_CLASS = @"pinproc_class: procgame.fakepinproc.FakePinPROC";

        /// <summary>
        /// The playback filename the framework uses
        /// </summary>
        const string PLAYBACK_FILE = @"playback.txt";
        #endregion

        #region Properties
        public static List<string> PlayBackFiles { get; set; } = new List<string>();
        #endregion

        #region Public Methods
        /// <summary>
        /// Copies the play back file to the game root directory to be picked up by FakePinProcPlayback
        /// </summary>
        /// <param name="gameFolder">The game folder.</param>
        /// <param name="playbackFile">The playback file.</param>
        public static void CopyPlayBackFileToGameRoot(string gameFolder, string playbackFile)
        {
            var gameRootPlaybackFile = Path.Combine(gameFolder, PLAYBACK_FILE);
            File.Copy(playbackFile, gameRootPlaybackFile, true);
        }

        /// <summary>
        /// Populates the playback files. <see cref="PlayBackFiles"/>
        /// </summary>
        /// <param name="recordingsFolder">The recordings folder.</param>
        public static void GetPlaybackFiles(string recordingsFolder)
        {
            PlayBackFiles.Clear();

            PlayBackFiles.AddRange(Directory.EnumerateFiles(recordingsFolder, "*.txt"));
        }

        /// <summary>
        /// Parses a playback file into PlayBackItems from a pyprocgame recorded game.
        /// </summary>
        /// <param name="playbackFile">The playback file.</param>
        /// <returns></returns>
        public static IList<PlayBackItem> ParsePlaybackFile(string playbackFile)
        {
            using (var sr = new StreamReader(playbackFile))
            {
                IList<PlayBackItem> playbackLines = new List<PlayBackItem>();
                var line = "";

                while ((line = sr.ReadLine()) != null)
                {
                    PlayBackItem p = new PlayBackItem();

                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var vals = line.Split('|');
                        if (vals.Length == 2)
                        {
                            //p.Event = Convert.ToDecimal(vals[0]);
                            //p.value = Convert.ToByte(vals[1]);
                        }
                        else if (vals.Length >= 4)
                        {
                            p.Event = Convert.ToDouble(vals[0]);
                            p.Type = Convert.ToString(vals[1]);
                            p.value = Convert.ToByte(vals[2]);
                            p.SwName = Convert.ToString(vals[3]);
                        }

                        playbackLines.Add(p);
                    }
                }

                return playbackLines;
            }
        }

        /// <summary>
        /// Sets the fake pin proc playback in the config.yaml. pinproc_class
        /// </summary>
        /// <param name="gameFolder">The game folder.</param>
        /// <param name="playbackEnabled">if set to <c>true</c> [playback enabled].</param>
        public static void SetFakePinProcPlayback(string gameFolder, bool playbackEnabled = false)
        {

            var configFile = gameFolder + "\\config.yaml";
            var configContents = File.ReadAllLines(configFile);

            var lineToFind = FAKEPINPROC_CLASS;
            var lineReplace = FAKEPINPROC_PLAYBACK_CLASS;

            if (!playbackEnabled)
            {
                lineToFind = FAKEPINPROC_PLAYBACK_CLASS;
                lineReplace = FAKEPINPROC_CLASS;
            }

            //Find the line in file and replace.
            bool resultFound = false;
            for (int i = 0; i < configContents.Length; i++)
            {
                if (configContents[i] == lineToFind)
                {
                    resultFound = true;
                    configContents[i] = lineReplace;
                    break;
                }
            }

            // Write the file if result was found and replaced.
            if (resultFound)
                File.WriteAllLines(configFile, configContents);
        }

        /// <summary>
        /// Asjusts the skeletongame.py to set the skeleton games base class to allow inheriting from the BasicRecordableGame
        /// </summary>
        /// <param name="gameFile">The skeletongame.py file path</param>
        /// <param name="recordable">If recordable the base class is changed to BasicRecordableGame</param>
        public static void SetSkeletonGameBaseClass(string gameFile, bool recordable)
        {
            //Setup the match and replace.
            string lineToFind = BASICGAME_CLASS;
            string lineReplace = BASICRECORDABLE_GAME_CLASS;

            //Read all contents from file
            var skeletonGameContents = File.ReadAllLines(gameFile);

            //If recording then switch the basic and recordable game around
            if (!recordable)
            {
                lineToFind = BASICRECORDABLE_GAME_CLASS;
                lineReplace = BASICGAME_CLASS;
            }

            //Find the line in file and replace.
            bool resultFound = false;
            for (int i = 0; i < skeletonGameContents.Length; i++)
            {
                if (skeletonGameContents[i] == lineToFind)
                {
                    resultFound = true;
                    skeletonGameContents[i] = lineReplace;
                    break;
                }
            }

            // Write the file if result was found and replaced.
            if (resultFound)
                File.WriteAllLines(gameFile, skeletonGameContents);

        } 
        #endregion
    }
}
