using System.Collections.Generic;
using System.Collections.ObjectModel;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class AssetsFile
    {
        [YamlMember(Alias = "UserInterface", ApplyNamingConventions = false)]
        public UserInterface UserInterface { get; set; }

        [YamlMember(Alias = "LampShows", ApplyNamingConventions = false)]
        public ObservableCollection<LampShow> LampShows { get; set; } = new ObservableCollection<LampShow>();

        [YamlMember(Alias = "RGBShows", ApplyNamingConventions = false)]
        public ObservableCollection<LampShow> RGBShows { get; set; } = new ObservableCollection<LampShow>();

        [YamlMember(Alias = "Animations", ApplyNamingConventions = false)]
        public ObservableCollection<Animation> Animations { get; set; } = new ObservableCollection<Animation>();

        [YamlMember(Alias = "Fonts", ApplyNamingConventions = false)]
        public Font Fonts { get; set; }

        [YamlMember(Alias = "Audio", ApplyNamingConventions = false)]
        public Audio Audio { get; set; }
    }
}
