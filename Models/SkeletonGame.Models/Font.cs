using System.Collections.Generic;
using System.Collections.ObjectModel;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models
{
    public class Font
    {
        [YamlMember(Alias = "FontStyles", ApplyNamingConventions = false)]
        public ObservableCollection<FontStyle> FontStyles { get; set; }

        [YamlMember(Alias = "HDFonts", ApplyNamingConventions = false)]
        public ObservableCollection<HdFont> HdFonts { get; set; }

        [YamlMember(Alias = "DMDFonts", ApplyNamingConventions = false)]
        public DMDFont DmdFonts { get; set; }
    }

    public class FontStyle
    {
        [YamlMember(Alias = "key", ApplyNamingConventions = false)]
        public string Key { get; set; }

        [YamlMember(Alias = "interior_color", ApplyNamingConventions = false)]
        public List<byte> InteriorColor { get; set; }

        [YamlMember(Alias = "line_width", ApplyNamingConventions = false)]
        public decimal LineWidth { get; set; }

        [YamlMember(Alias = "line_color", ApplyNamingConventions = false)]
        public List<byte> LineColor { get; set; }
    }

    public class HdFont : AssetKeyFile
    {
        [YamlMember(Alias = "size", ApplyNamingConventions = false)]
        public int Size { get; set; }
    }

    public class DMDFont : AssetKeyFile
    {
    }
}
