using SkeletonGame.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SkeletonGame.Engine
{
    public interface ISkeletonGameFiles
    {
        Task<IEnumerable<string>> GetFilesAsync(string path, AssetTypes assetType);
        
    }

    public class SkeletonGameFiles : ISkeletonGameFiles
    {
        public async Task<IEnumerable<string>> GetFilesAsync(string path,  AssetTypes assetType)
        {
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException($"Asset path not found {path} for type {assetType.ToString()}");

            return await Task.Run(() =>
            {
                switch (assetType)
                {
                    case AssetTypes.Lampshows:
                        return Directory.EnumerateFiles(path, @"*.lampshow");
                    case AssetTypes.HdFonts:
                        return Directory.EnumerateFiles(path, @"*.ttf");
                    case AssetTypes.Music:
                    case AssetTypes.Voice:
                    case AssetTypes.Sfx:
                        var audioFileFilters = new List<string> { "*.wav", "*.mp3", "*.ogg" };
                        return GetFiles(path, audioFileFilters);
                    case AssetTypes.Animations:
                        var animFileFilters = new List<string> { "*.png", "*.jpg", "*.mp4", "*.avi" };
                        return GetFiles(path, animFileFilters);
                    default:
                        return null;
                }
            });
            
        }

        /// <summary>
        /// Gets all the files in a directory & sub dirs by extension filters.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="filters">The filters.</param>
        /// <returns></returns>
        private IEnumerable<string> GetFiles(string path, IEnumerable<string> filters)
        {
            var files = new List<string>();
            foreach (var filter in filters)
            {
                files.AddRange(Directory.EnumerateFiles(path, filter, SearchOption.AllDirectories));                
            }

            return files;
        }
    }
}
