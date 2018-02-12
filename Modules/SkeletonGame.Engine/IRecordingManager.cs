using SkeletonGame.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace SkeletonGame.Engine
{
    public static class RecordingManager
    {
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
                PlayBackItem p = new PlayBackItem();

                while ((line = sr.ReadLine()) != null)
                {
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
                            p.Event = Convert.ToDecimal(vals[0]);
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

        public static List<string> PlayBackFiles { get; set; } = new List<string>();

        /// <summary>
        /// Populates the playback files. <see cref="PlayBackFiles"/>
        /// </summary>
        /// <param name="recordingsFolder">The recordings folder.</param>
        public static void GetPlaybackFiles(string recordingsFolder)
        {
            PlayBackFiles.Clear();

            PlayBackFiles.AddRange(Directory.EnumerateFiles(recordingsFolder, "*.txt"));
        }
    }
}
