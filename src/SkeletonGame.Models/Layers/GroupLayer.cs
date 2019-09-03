using SkeletonGame.Models.Transition;
using System;
using System.Collections.ObjectModel;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
    [Serializable]
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

        [YamlMember(Alias = "zoom_layer", ApplyNamingConventions = false)]
        public ZoomLayer ZoomLayer { get; set; } = new ZoomLayer();

        [YamlMember(Alias = "rotate_layer", ApplyNamingConventions = false)]
        public RotateLayer RotateLayer { get; set; } = new RotateLayer();

        [YamlMember(Alias = "move_layer", ApplyNamingConventions = false)]
        public MoveLayer MoveLayer { get; set; } = new MoveLayer();

        public GroupLayer()
        {
            this.SeqType = SequenceType.GroupLayer;
            SequenceName = SeqType + "SequenceStyle";
            this.duration = 5.0m;
        }

        public GroupLayer(SequenceBase sequenceBase) : base()
        {

        }
    }
}
