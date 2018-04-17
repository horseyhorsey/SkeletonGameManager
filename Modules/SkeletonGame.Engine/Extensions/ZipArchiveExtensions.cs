using System.IO;
using System.IO.Compression;

namespace SkeletonGame.Engine.Extensions
{
    public static class ZipArchiveExtensions
    {
        /// <summary>
        /// Extracts all files/folder to directory.
        /// </summary>
        /// <param name="archive">The archive.</param>
        /// <param name="destinationDirectoryName">Name of the destination directory.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        public static void ExtractToDirectory(this ZipArchive archive, string destinationDirectoryName, bool overwrite)
        {
            if (!overwrite)
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                return;
            }
            foreach (ZipArchiveEntry file in archive.Entries)
            {
                string completeFileName = Path.Combine(destinationDirectoryName, file.FullName);
                if (file.Name == "")
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                    continue;
                }
                file.ExtractToFile(completeFileName, true);
            }
        }
    }
}
