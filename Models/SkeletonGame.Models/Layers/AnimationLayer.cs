using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class AnimationLayer : SequenceBase
    {
        [YamlMember(Alias = "name", ApplyNamingConventions = false)]
        public string Name { get; set; }

    }
}
