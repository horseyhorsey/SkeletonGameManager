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
                    default:
                        return null;
                }
            });
            
        }
    }
}
