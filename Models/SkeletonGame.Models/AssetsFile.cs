using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class AssetsFile
    {
        [YamlMember(Alias = "UserInterface", ApplyNamingConventions = false)]
        public UserInterface UserInterface { get; set; }

        [YamlMember(Alias = "LampShows", ApplyNamingConventions = false)]
        public List<LampShow> LampShows { get; set; }

        [YamlMember(Alias = "Animations", ApplyNamingConventions = false)]
        public List<Animation> Animations { get; set; }

        [YamlMember(Alias = "Fonts", ApplyNamingConventions = false)]
        public Font Fonts { get; set; }

        [YamlMember(Alias = "Audio", ApplyNamingConventions = false)]
        public Audio Audio { get; set; }
    }
}
