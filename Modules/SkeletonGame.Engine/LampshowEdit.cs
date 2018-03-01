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
        void ReverseLampshowFile(string file, string reversedFile);
    }

    public class LampshowEdit : ILampshowEdit
    {
        /// <summary>
        /// Reverses a lampshow file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="reversedFile">The reversed file to save</param>
        public void ReverseLampshowFile(string file, string reversedFile)
        {
            using (var sr = File.OpenText(file))
            using (var sw = File.CreateText(reversedFile))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("lamp") || line.StartsWith("coil"))
                    {
                        var split = line.Split('|');
                        var splitreversed = new string(split[1].Reverse().ToArray()).Insert(0, " ");
                        line = split[0] + "|" + splitreversed.Remove(splitreversed.Length - 1);
                    }

                    sw.WriteLine(line);
                }
            }
        }
    }
}
