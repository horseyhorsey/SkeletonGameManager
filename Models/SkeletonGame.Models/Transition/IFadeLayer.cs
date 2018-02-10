using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Transition
{
    public interface IFadeLayer
    {        
        string FadeType { get; set; }

        int FrameCount { get; set; }
    }

    public class FadeLayer : IFadeLayer
    {
        [YamlMember(Alias = "param", ApplyNamingConventions = false, SerializeAs = typeof(string), ScalarStyle = YamlDotNet.Core.ScalarStyle.SingleQuoted)]
        public string FadeType { get; set; } = "off";

        [YamlMember(Alias = "frame_count", ApplyNamingConventions = false)]
        public int FrameCount { get; set; } = 25;
    }
}
