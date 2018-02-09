using SkeletonGame.Models.Transforms;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class RotateLayer : IRotationLayer
    {
        public int x { get; set; }
        public int y { get; set; }

        [YamlMember(Alias = "rotation_update", ApplyNamingConventions = false)]
        public int RotateUpdate { get; set; } = 0;

        [YamlMember(Alias = "enabled", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public bool IsEnabled { get; set; } = false;

        public RotateLayer()
        {

        }
    }
}
