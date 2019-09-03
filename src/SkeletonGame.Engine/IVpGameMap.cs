using SkeletonGame.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;

namespace SkeletonGame.Engine
{
    public interface IVpGameMapper
    {
        IEnumerable<VpGameMap> GetMappings(string vp_game_map = @"C:\P-ROC\shared\vp_game_map.yaml");
        void SaveMappings(IEnumerable<VpGameMap> gameMaps, string vp_game_map = @"C:\P-ROC\shared\vp_game_map.yaml");
    }

    public class VpGameMapper : IVpGameMapper
    {
        public IEnumerable<VpGameMap> GetMappings(string vp_game_map = @"C:\P-ROC\shared\vp_game_map.yaml")
        {
            //Create serializer and load the mapping file
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

            //Convert the dict maps to list of VPGamemap
            var maps = new List<VpGameMap>();            
            foreach (var gameMap in gameMappingDict)
            {
                var valType = gameMap.Value as Dictionary<object, object>;
                var kls = valType["kls"].ToString();
                var path = valType["path"].ToString();
                var yaml = valType["yaml"].ToString();
                string table = string.Empty;
                if (valType.ContainsKey("table"))
                {
                    table = valType["table"] != null ? valType["table"].ToString() : string.Empty;
                }
                maps.Add(new VpGameMap(gameMap.Key.ToString(), path, kls, yaml, table));
            }

            return maps.OrderBy(x => x.Rom);
        }

        public void SaveMappings(IEnumerable<VpGameMap> gameMaps, string vp_game_map = "C:\\P-ROC\\shared\\vp_game_map_test.yaml")
        {
            Dictionary<object, Dictionary<string, object>> mappings = new Dictionary<object, Dictionary<string, object>>();
            var yamlSerializer = new Serializer();
            using (TextWriter writer = File.CreateText(vp_game_map))
            {
                //    Dictionary<object, object> dict = new Dictionary<object, object>();
                foreach (var mapping in gameMaps)
                {
                    var meh = new Dictionary<string, object>();
                    meh.Add("path", mapping.Path);
                    meh.Add("kls", mapping.Kls);
                    meh.Add("yaml", mapping.Yaml);
                    meh.Add("table", mapping.Table);
                    mappings.Add(mapping.Rom, meh);
                }
                yamlSerializer.Serialize(writer, mappings);
            }
        }
    }
}
