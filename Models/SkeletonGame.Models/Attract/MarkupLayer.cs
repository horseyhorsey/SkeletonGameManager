using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Attract
{
    public class MarkupLayer : SequenceBase
    {
        [YamlMember(Alias = "width", ApplyNamingConventions = false)]
        public string Width { get; set; }

        [YamlMember(Alias = "Bold", ApplyNamingConventions = false)]
        public Bold Bold { get; set; }

        [YamlMember(Alias = "Normal", ApplyNamingConventions = false)]
        public Normal Normal { get; set; }

        [YamlMember(Alias = "Message", ApplyNamingConventions = false)]
        public List<string> Message { get; set; }

        [YamlMember(Alias = "Animation", ApplyNamingConventions = false)]
        public string Animation { get; set; }
    }

    public class Bold
    {
        [YamlMember(Alias = "Font", ApplyNamingConventions = false)]
        public string Font { get; set; }

        [YamlMember(Alias = "FontStyle", ApplyNamingConventions = false)]
        public string FontStyle { get; set; }
    }

    public class Normal
    {
        [YamlMember(Alias = "Font", ApplyNamingConventions = false)]
        public string Font { get; set; }

        [YamlMember(Alias = "FontStyle", ApplyNamingConventions = false)]
        public string FontStyle { get; set; }
    }
}
