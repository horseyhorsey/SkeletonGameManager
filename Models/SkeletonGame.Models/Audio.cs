using System.Collections.Generic;
using System.Collections.ObjectModel;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models
{
    public class Audio
    {
        [YamlMember(Alias = "Music", ApplyNamingConventions = false)]
        public ObservableCollection<Music> Music { get; set; }

        [YamlMember(Alias = "Effects", ApplyNamingConventions = false)]
        public ObservableCollection<Music> Effects { get; set; }

        [YamlMember(Alias = "Voice", ApplyNamingConventions = false)]
        public ObservableCollection<Music> Voice { get; set; }

        [YamlIgnore]
        public ObservableCollection<Music> AllAudio { get; set; } = new ObservableCollection<Models.Music>();
    }

    public class Music : AssetKeyFile
    {
        [YamlMember(Alias = "volume", ApplyNamingConventions = false)]
        public decimal Volume { get; set; } 
    }
}
