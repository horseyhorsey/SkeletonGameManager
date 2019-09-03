using System.Collections.Generic;
using System.Collections.ObjectModel;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models
{
    public class Audio
    {
        [YamlMember(Alias = "Music", ApplyNamingConventions = false)]
        public ObservableCollection<AudioFile> Music { get; set; }

        [YamlMember(Alias = "Effects", ApplyNamingConventions = false)]
        public ObservableCollection<AudioFile> Effects { get; set; }

        [YamlMember(Alias = "Voice", ApplyNamingConventions = false)]
        public ObservableCollection<AudioFile> Voice { get; set; }

        [YamlIgnore]
        public ObservableCollection<AudioFile> AllAudio { get; set; } = new ObservableCollection<Models.AudioFile>();
    }

    public class AudioFile : AssetKeyFile
    {
        [YamlMember(Alias = "volume", ApplyNamingConventions = false)]
        public decimal Volume { get; set; }

        [YamlMember(Alias = "streaming_load", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public string StreamingLoad { get; set; } = "True";
    }
}
