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
        void CreateNewGameEntry(string gameName, string templateName, string procFolder, string skeletonGameBranchZip);
    }

    public class CreateSkeletonGame : ICreateSkeletonGame
    {
        /// <summary>
        /// Creates new game from a downloaded skeleton game framework.
        /// </summary>
        /// <param name="gameName"></param>
        /// <param name="procFolder"></param>
        /// <param name="skeletonGameBranchZip">The skeleton game branch zip.</param>
        public void CreateNewGameEntry(string gameName, string templateName, string procFolder, string skeletonGameBranchZip)
        {
            var fullPath = Path.Combine(procFolder, gameName);
            var gamePath = CreateDirectoriesForNewGame(fullPath);

            //Create the extra directory I created for VP help
            if (templateName == "EmptyGameVP")
                Directory.CreateDirectory(Path.Combine(fullPath, "VP"));

            using (ZipArchive archive = ZipFile.OpenRead(skeletonGameBranchZip))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    var fullname = entry.FullName;
                    if (fullname.Contains($"PyProcGameHD-SkeletonGame-dev/{templateName}"))                    
                        ExtractFile(gamePath, entry, fullname, $"PyProcGameHD-SkeletonGame-dev/{templateName}/");                    
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
        private string CreateDirectoriesForNewGame(string fullGamePath)
        {            
            Directory.CreateDirectory(fullGamePath);
            Directory.CreateDirectory(fullGamePath + "\\assets\\dmd");
            Directory.CreateDirectory(fullGamePath + "\\assets\\fonts");
            Directory.CreateDirectory(fullGamePath + "\\assets\\lampshows");
            Directory.CreateDirectory(fullGamePath + "\\assets\\sound\\music");
            Directory.CreateDirectory(fullGamePath + "\\assets\\sound\\voice");
            Directory.CreateDirectory(fullGamePath + "\\assets\\sound\\sfx");

            Directory.CreateDirectory(fullGamePath + "\\config");
            Directory.CreateDirectory(fullGamePath + "\\config\\profiles");
            Directory.CreateDirectory(fullGamePath + "\\config\\trophys");

            Directory.CreateDirectory(fullGamePath + "\\my_modes");

            Directory.CreateDirectory(fullGamePath + "\\procgame");
            Directory.CreateDirectory(fullGamePath + "\\procgame\\desktop");
            Directory.CreateDirectory(fullGamePath + "\\procgame\\dmd");
            Directory.CreateDirectory(fullGamePath + "\\procgame\\game");
            Directory.CreateDirectory(fullGamePath + "\\procgame\\highscore");
            Directory.CreateDirectory(fullGamePath + "\\procgame\\modes");
            Directory.CreateDirectory(fullGamePath + "\\procgame\\profiles");
            Directory.CreateDirectory(fullGamePath + "\\procgame\\tools\\mailbox");

            return fullGamePath;
        }
    }
}

