using SkeletonGame.Models.Transforms;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ZoomLayer : SequenceBase, IZoomLayer
    {

        [YamlMember(Alias = "hold", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public bool Hold { get; set; } = false;

        [YamlMember(Alias = "frames_per_zoom", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public int FramesPerZoom { get; set; } = 1;

        [YamlMember(Alias = "total_zooms", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public int TotalZooms { get; set; } = 30;

        [YamlMember(Alias = "scale_start", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public decimal ScaleStart { get; set; } = 0.1m;

        [YamlMember(Alias = "scale_stop", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public decimal ScaleStop { get; set; } = 1.0m;

        [YamlMember(Alias = "name", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public string Name { get; set; }

        [YamlMember(Alias = "enabled", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public bool IsEnabled { get; set; } = false;

        public ZoomLayer()
        {
            this.SeqType = SequenceType.ZoomLayer;
            this.duration = null;
            this.lampshow = null;
        }
    }
}
