using SkeletonGame.Models;
using System.Collections.Generic;
using System.IO;

namespace SkeletonGame.Engine
{
    public interface IVpGameMapper
    {
        IEnumerable<VpGameMap> GetMappings(string vp_game_map = @"C:\P-ROC\shared\vp_game_map.yaml");
    }

    public class VpGameMapper : IVpGameMapper
    {
        public IEnumerable<VpGameMap> GetMappings(string vp_game_map = @"C:\P-ROC\shared\vp_game_map.yaml")
        {
            var builder = new YamlDotNet.Serialization.DeserializerBuilder()
                .Build();

            var mapping = builder.Deserialize(File.OpenText(vp_game_map));

            //Get mapping as dict and remove rundir
            var gameMappingDict = mapping as Dictionary<object, object>;
            try
            {
                gameMappingDict.Remove("rundir");
            }
            catch { }

            var maps = new List<VpGameMap>();
            //Convert the dict maps to class
            foreach (var gameMap in gameMappingDict)
            {
                var valType = gameMap.Value as Dictionary<object, object>;

                var kls = valType["kls"].ToString();
                var path = valType["path"].ToString();
                var yaml = valType["yaml"].ToString();
                maps.Add(new VpGameMap(path, kls, yaml));                
            }

            return maps;
        }
    }
}
