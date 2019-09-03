using System;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
    [Serializable]
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class AnimationLayer : SequenceBase
    {
        [YamlMember(Alias = "name", ApplyNamingConventions = false)]
        public string Name { get; set; }

    }
}
