using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Attract
{
    public class PanningLayer : SequenceBase
    {
        [YamlMember(Alias = "width", ApplyNamingConventions = false)]
        public string Width { get; set; }

        [YamlMember(Alias = "height", ApplyNamingConventions = false)]
        public string Height { get; set; }

        [YamlMember(Alias = "origin_x", ApplyNamingConventions = false)]
        public string OriginX { get; set; }

        [YamlMember(Alias = "origin_y", ApplyNamingConventions = false)]
        public string OriginY { get; set; }

        [YamlMember(Alias = "scroll_x", ApplyNamingConventions = false)]
        public string ScrollX { get; set; }

        [YamlMember(Alias = "scroll_y", ApplyNamingConventions = false)]
        public string ScrollY { get; set; }

        [YamlMember(Alias = "frames_per_movement", ApplyNamingConventions = false)]
        public string FramesPerMovement { get; set; }

        [YamlMember(Alias = "bounce", ApplyNamingConventions = false)]
        public string Bounce { get; set; }

        [YamlMember(Alias = "contents", ApplyNamingConventions = false)]
        public Contents Contents { get; set; }        
    }
}
