using SkeletonGame.Models.Transition;
using System.Collections.ObjectModel;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class GroupLayer : TransitionBaseLayer
    {
        [YamlMember(Alias = "Name", ApplyNamingConventions = false)]
        public string Name { get; set; }

        [YamlMember(Alias = "width", ApplyNamingConventions = false)]
        public string Width { get; set; }

        [YamlMember(Alias = "height", ApplyNamingConventions = false)]
        public string Height { get; set; }

        [YamlMember(Alias = "opaque", ApplyNamingConventions = false)]
        public string Opaque { get; set; } = null;

        [YamlMember(Alias = "fill_color", ApplyNamingConventions = false)]
        public byte[] FillColor { get; set; } = null;

        [YamlMember(Alias = "contents", ApplyNamingConventions = false)]
        public ObservableCollection<Content> Contents { get; set; } = new ObservableCollection<Content>();

        public GroupLayer()
        {
            this.SeqType = SequenceType.GroupLayer;
            this.duration = 5.0m;
        }

        public GroupLayer(SequenceBase sequenceBase) : base()
        {

        }
    }
}
