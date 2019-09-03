using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonGameManager.Base
{
    public interface IGameRunnner
    {
        /// <summary>
        /// Runs the specified game entry file.
        /// </summary>
        /// <param name="gameEntryFile">The game entry file. Game.py in most cases</param>
        /// <returns></returns>
        Task Run(string gameFolder, string gameEntryFile);
    }
}
