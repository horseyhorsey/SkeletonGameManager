using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models
{
    public class GameData
    {
        [YamlMember(Alias = "Audits", ApplyNamingConventions = false)]
        public Dictionary<string, string> Audits { get; set; }
    }
}
