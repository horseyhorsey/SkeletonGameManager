using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models
{
    public class Audio
    {
        [YamlMember(Alias = "Music", ApplyNamingConventions = false)]
        public List<Music> Music { get; set; }

        [YamlMember(Alias = "Effects", ApplyNamingConventions = false)]
        public List<Effect> Effects { get; set; }

        [YamlMember(Alias = "Voice", ApplyNamingConventions = false)]
        public List<Voice> Voice { get; set; }
    }

    public class Music : AssetKeyFile
    {
        [YamlMember(Alias = "volume", ApplyNamingConventions = false)]
        public decimal Volume { get; set; } 
    }

    public class Effect : Music
    {

    }

    public class Voice : Music
    {
    }
}
