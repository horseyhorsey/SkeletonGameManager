using System.IO;
using System.IO.Compression;

namespace SkeletonGame.Engine
{
    public interface ICreateSkeletonGame
    {
        /// <summary>
        /// Creates new game from a downloaded skeleton game framework
        /// </summary>
        /// <param name="skeletonGameBranchZip">The skeleton game branch zip.</param>
        void CreateNewGameEntry(string gameName, string procFolder, string skeletonGameBranchZip);
    }

    public class CreateSkeletonGame : ICreateSkeletonGame
    {
        /// <summary>
        /// Creates new game from a downloaded skeleton game framework.
        /// </summary>
        /// <param name="gameName"></param>
        /// <param name="procFolder"></param>
        /// <param name="skeletonGameBranchZip">The skeleton game branch zip.</param>
        public void CreateNewGameEntry(string gameName, string procFolder, string skeletonGameBranchZip)
        {
            var gamePath = CreateDirectoriesForNewGame(procFolder, gameName);

            using (ZipArchive archive = ZipFile.OpenRead(skeletonGameBranchZip))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    var fullname = entry.FullName;
                    if (fullname.Contains("PyProcGameHD-SkeletonGame-dev/EmptyGame/"))                    
                        ExtractFile(gamePath, entry, fullname, "PyProcGameHD-SkeletonGame-dev/EmptyGame/");                    
                    else if (fullname.Contains("PyProcGameHD-SkeletonGame-dev/procgame/"))
                        ExtractFile(gamePath, entry, fullname, "PyProcGameHD-SkeletonGame-dev/");
                }
            }
        }

        private void ExtractFile(string gamePath, ZipArchiveEntry entry, string fullname, string replaceString)
        {
            var ext = Path.HasExtension(fullname);
            if (ext)
            {
                var fileNameAndDir = fullname
                    .Replace($"{replaceString}", string.Empty);

                var extractPath = Path.Combine(gamePath, fileNameAndDir);

                if (!File.Exists(extractPath))
                {
                    entry.ExtractToFile(extractPath);
                }
                    
            }
        }

        /// <summary>
        /// Creates the directories for new game.
        /// </summary>
        /// <param name="procFolder">The proc folder.</param>
        /// <param name="gameName">Name of the game.</param>
        /// <returns>The full path to the game</returns>
        private string CreateDirectoriesForNewGame(string procFolder, string gameName)
        {
            var fullPath = Path.Combine(procFolder, gameName);
            Directory.CreateDirectory(fullPath);
            Directory.CreateDirectory(fullPath + "\\assets\\dmd");
            Directory.CreateDirectory(fullPath + "\\assets\\fonts");
            Directory.CreateDirectory(fullPath + "\\assets\\lampshows");
            Directory.CreateDirectory(fullPath + "\\assets\\sound\\music");
            Directory.CreateDirectory(fullPath + "\\assets\\sound\\voice");
            Directory.CreateDirectory(fullPath + "\\assets\\sound\\sfx");

            Directory.CreateDirectory(fullPath + "\\config");
            Directory.CreateDirectory(fullPath + "\\config\\profiles");
            Directory.CreateDirectory(fullPath + "\\config\\trophys");

            Directory.CreateDirectory(fullPath + "\\my_modes");

            Directory.CreateDirectory(fullPath + "\\procgame");
            Directory.CreateDirectory(fullPath + "\\procgame\\desktop");
            Directory.CreateDirectory(fullPath + "\\procgame\\dmd");
            Directory.CreateDirectory(fullPath + "\\procgame\\game");
            Directory.CreateDirectory(fullPath + "\\procgame\\highscore");
            Directory.CreateDirectory(fullPath + "\\procgame\\modes");
            Directory.CreateDirectory(fullPath + "\\procgame\\profiles");
            Directory.CreateDirectory(fullPath + "\\procgame\\tools\\mailbox");

            return fullPath;
        }
    }
}

