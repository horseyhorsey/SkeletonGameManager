using SkeletonGame.Models.Transition;
using System;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
    [Serializable]
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class AttractAnimation : SequenceBase
    {

        [YamlMember(Alias = "anim_name", ApplyNamingConventions = false)]
        public string AnimName { get; set; } = "missing";

        [YamlMember(Alias = "name", ApplyNamingConventions = false)]
        public string Name { get; set; }

        [YamlMember(Alias = "x", ApplyNamingConventions = false)]
        public int X { get; set; } = 0;

        [YamlMember(Alias = "y", ApplyNamingConventions = false)]
        public int Y { get; set; }

        [YamlMember(Alias = "repeat", ApplyNamingConventions = false)]
        public bool Repeat { get; set; }

        [YamlMember(Alias = "hold_last_frame", ApplyNamingConventions = false)]
        public bool Hold { get; set; }

        [YamlMember(Alias = "opaque", ApplyNamingConventions = false)]
        public bool Opaque { get; set; }

        public AttractAnimation()
        {
            this.AnimName = "missing";
            this.SeqType = SequenceType.Animation;
        }

        [YamlMember(Alias = "zoom_layer", ApplyNamingConventions = false)]
        public ZoomLayer ZoomLayer { get; set; } = new ZoomLayer();

        [YamlMember(Alias = "rotate_layer", ApplyNamingConventions = false)]
        public RotateLayer RotateLayer { get; set; } = new RotateLayer();

        [YamlMember(Alias = "move_layer", ApplyNamingConventions = false)]
        public MoveLayer MoveLayer { get; set; } = new MoveLayer();

        [YamlMember(Alias = "fade", ApplyNamingConventions = false)]
        public FadeLayer FadeLayer { get; set; } = new FadeLayer();
    }
}
