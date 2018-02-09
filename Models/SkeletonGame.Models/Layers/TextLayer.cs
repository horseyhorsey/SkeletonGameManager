using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class TextLayer : SequenceTextBase
    {
        [YamlMember(Alias = "x", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public string X { get; set; } = "0";

        [YamlMember(Alias = "y", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public string Y { get; set; } = "0";

        [YamlMember(Alias = "width", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public int Width { get; set; } = 0;

        [YamlMember(Alias = "height", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public int Height { get; set; } = 0;

        [YamlMember(Alias = "v_justify", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public VJustify VJustify { get; set; } = VJustify.top;

        [YamlMember(Alias = "h_justify", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public HJustify HJustify { get; set; } = HJustify.left;

        [YamlMember(Alias = "Text", ApplyNamingConventions = false)]
        public string Text { get; set; } = "TEXT LAYER";

        [YamlMember(Alias = "blink_frames", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public bool BlinkFrames { get; set; } = false;

        [YamlMember(Alias = "move_layer", ApplyNamingConventions = false)]
        public MoveLayer MoveLayer { get; set; }

        public TextLayer()
        {
            this.SeqType = SequenceType.TextLayer;
        }

    }
}
