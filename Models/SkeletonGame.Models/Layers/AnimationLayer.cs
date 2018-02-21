using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class AnimationLayer
    {
        [YamlMember(Alias = "name", ApplyNamingConventions = false)]
        public string Name { get; set; }

    }
}
