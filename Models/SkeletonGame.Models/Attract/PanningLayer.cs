using System.Collections.ObjectModel;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Attract
{
    public class Contents
    {
        [YamlMember(Alias = "group_layer", ApplyNamingConventions = false)]
        public GroupLayer group_layer { get; set; }
    }

    public class PanningLayer : SequenceBase
    { 

        [YamlMember(Alias = "width", ApplyNamingConventions = false)]
        public int Width { get; set; } = 500;

        [YamlMember(Alias = "height", ApplyNamingConventions = false)]
        public int Height { get; set; } = 500;

        [YamlMember(Alias = "origin_x", ApplyNamingConventions = false, SerializeAs =typeof(string))]
        public int? OriginX { get; set; } = -1;

        [YamlMember(Alias = "origin_y", ApplyNamingConventions = false)]
        public int OriginY { get; set; } = -130;

        [YamlMember(Alias = "scroll_x", ApplyNamingConventions = false,SerializeAs = typeof(string))]
        public string ScrollX { get; set; } = "0";

        [YamlMember(Alias = "scroll_y", ApplyNamingConventions = false)]
        public int ScrollY { get; set; } = 2;

        [YamlMember(Alias = "frames_per_movement", ApplyNamingConventions = false)]
        public int FramesPerMovement { get; set; } = 1;

        [YamlMember(Alias = "bounce", ApplyNamingConventions = false)]
        public string Bounce { get; set; } = "false";    

        [YamlMember(Alias = "contents", ApplyNamingConventions = false)]
        public Contents Contents { get; set; } = new Contents();

        public PanningLayer()
        {
            this.SeqType = SequenceType.PanningLayer;

            //Contents.Add(new Content() { group_layer = new GroupLayer() });            
        }
    }
}
