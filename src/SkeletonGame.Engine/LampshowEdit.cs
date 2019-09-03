using System.IO;
using System.Linq;

namespace SkeletonGame.Engine
{
    public interface ILampshowEdit
    {
        /// <summary>
        /// Reverses a lampshow file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="reversedFile">The reversed file to save</param>
        string ReverseLampshowFile(string file, string reversedFile);
    }

    public class LampshowEdit : ILampshowEdit
    {
        /// <summary>
        /// Reverses a lampshow or rgb file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="reversedFile">The reversed file to save</param>
        /// <returns>The reversed lampshow path</returns>
        public string ReverseLampshowFile(string file, string reversedFile)
        {
            using (var sr = File.OpenText(file))
            using (var sw = File.CreateText(reversedFile))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("lamp") || line.StartsWith("coil") || line.StartsWith("rgb"))
                    {
                        line = SplitAndReversePipedTrack(line);
                    }
                    else if (line.Contains("|"))
                    {
                        if (!line.StartsWith("#"))
                        {
                            line = SplitAndReversePipedTrack(line);
                        }
                    }

                    sw.WriteLine(line);
                }

                return Path.GetFileName(reversedFile);
            }
        }

        private string SplitAndReversePipedTrack(string line)
        {
            var split = line.Split('|');
            var splitreversed = new string(split[1].Reverse().ToArray()).Insert(0, " ");
            line = split[0] + "|" + splitreversed.Remove(splitreversed.Length - 1);
            return line;
        }
    }
}
