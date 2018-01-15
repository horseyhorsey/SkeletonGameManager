using SkeletonGame.Models.Attract;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models
{
    public class AttractYaml
    {
        [YamlMember(Alias = "Sequence", ApplyNamingConventions = false)]
        public List<Sequence> Sequence { get; set; }
    }

    public class SequenceBase
    {
        public string lampshow { get; set; }
        public string duration { get; set; }
    }

    public class Sequence
    {
        [YamlMember(Alias = "LastScores", ApplyNamingConventions = false)]
        public LastScores LastScores { get; set; }

        [YamlMember(Alias = "Combo", ApplyNamingConventions = false)]
        public Combo Combo { get; set; }

        [YamlMember(Alias = "text_layer", ApplyNamingConventions = false)]
        public TextLayer text_layer { get; set; }

        [YamlMember(Alias = "panning_layer", ApplyNamingConventions = false)]
        public PanningLayer panning_layer { get; set; }

        [YamlMember(Alias = "RandomText", ApplyNamingConventions = false)]
        public RandomText RandomText { get; set; }

        [YamlMember(Alias = "Animation", ApplyNamingConventions = false)]
        public AttractAnimation Animation { get; set; }

        [YamlMember(Alias = "HighScores", ApplyNamingConventions = false)]
        public HighScores HighScores { get; set; }

        [YamlMember(Alias = "Credits", ApplyNamingConventions = false)]
        public Credits Credits { get; set; }
    }
    
    public class LastScores : SequenceBase
    {
        [YamlMember(Alias = "Font", ApplyNamingConventions = false)]
        public string Font { get; set; }

        [YamlMember(Alias = "FontStyle", ApplyNamingConventions = false)]
        public string FontStyle { get; set; }

        [YamlMember(Alias = "Background", ApplyNamingConventions = false)]
        public string Background { get; set; }

        [YamlMember(Alias = "sound", ApplyNamingConventions = false)]
        public string Sound { get; set; }

        public AttractSequenceType GetSequenceType => AttractSequenceType.LastScores;
    }

    public class Combo : SequenceBase
    {
        [YamlMember(Alias = "Text", ApplyNamingConventions = false)]
        public List<string> Text { get; set; }

        [YamlMember(Alias = "Animation", ApplyNamingConventions = false)]
        public string Animation { get; set; }

        [YamlMember(Alias = "sound", ApplyNamingConventions = false)]
        public string Sound { get; set; }

        [YamlMember(Alias = "Font", ApplyNamingConventions = false)]
        public string Font { get; set; }

        [YamlMember(Alias = "FontStyle", ApplyNamingConventions = false)]
        public string FontStyle { get; set; }

        public AttractSequenceType GetSequenceType => AttractSequenceType.Combo;
    }

    public class TextLayer : SequenceBase
    {
        [YamlMember(Alias = "x", ApplyNamingConventions = false)]
        public string X { get; set; }

        [YamlMember(Alias = "y", ApplyNamingConventions = false)]
        public string Y { get; set; }

        [YamlMember(Alias = "v_justify", ApplyNamingConventions = false)]
        public string VJustify { get; set; }

        [YamlMember(Alias = "h_justify", ApplyNamingConventions = false)]
        public string HJustify { get; set; }

        [YamlMember(Alias = "Text", ApplyNamingConventions = false)]
        public string Text { get; set; }

        [YamlMember(Alias = "Font", ApplyNamingConventions = false)]
        public string Font { get; set; }

        public AttractSequenceType GetSequenceType => AttractSequenceType.text_layer;
    }

    public class RandomText : SequenceBase
    {
        [YamlMember(Alias = "Header", ApplyNamingConventions = false)]
        public string Header { get; set; }

        [YamlMember(Alias = "TextOptions", ApplyNamingConventions = false)]
        public List<TextOption> TextOptions { get; set; }

        [YamlMember(Alias = "Font", ApplyNamingConventions = false)]
        public string Font { get; set; }

        [YamlMember(Alias = "Animation", ApplyNamingConventions = false)]
        public string Animation { get; set; }

        [YamlMember(Alias = "FontStyle", ApplyNamingConventions = false)]
        public string FontStyle { get; set; }

        public AttractSequenceType GetSequenceType => AttractSequenceType.RandomText;
    }

    public class AnimationLayer
    {
        public string name { get; set; }
    }

    public class Content
    {
        [YamlMember(Alias = "animation_layer", ApplyNamingConventions = false)]
        public AnimationLayer AnimationLayer { get; set; }

        [YamlMember(Alias = "markup_layer", ApplyNamingConventions = false)]
        public MarkupLayer MarkupLayer { get; set; }
    }

    public class GroupLayer
    {
        [YamlMember(Alias = "width", ApplyNamingConventions = false)]
        public string Width { get; set; }

        [YamlMember(Alias = "height", ApplyNamingConventions = false)]
        public string Height { get; set; }

        [YamlMember(Alias = "contents", ApplyNamingConventions = false)]
        public List<Content> Contents { get; set; }
    }

    public class Contents
    {
        [YamlMember(Alias = "group_layer", ApplyNamingConventions = false)]
        public GroupLayer GroupLayer { get; set; }
    }

    public class TextOption
    {
        [YamlMember(Alias = "Text", ApplyNamingConventions = false)]
        public object Text { get; set; }
    }

    public class AttractAnimation : SequenceBase
    {
        [YamlMember(Alias = "Name", ApplyNamingConventions = false)]
        public string Name { get; set; }

        [YamlMember(Alias = "sound", ApplyNamingConventions = false)]
        public string Sound { get; set; }

        public AttractSequenceType GetSequenceType => AttractSequenceType.Animation;
    }

    public class HighScores : SequenceBase
    {
        [YamlMember(Alias = "Font", ApplyNamingConventions = false)]
        public string Font { get; set; }

        [YamlMember(Alias = "FontStyle", ApplyNamingConventions = false)]
        public string FontStyle { get; set; }

        [YamlMember(Alias = "Background", ApplyNamingConventions = false)]
        public string Background { get; set; }

        [YamlMember(Alias = "Order", ApplyNamingConventions = false)]
        public List<string> Order { get; set; }

        public AttractSequenceType GetSequenceType => AttractSequenceType.HighScores;
    }

    public class Credits : SequenceBase
    {
        [YamlMember(Alias = "Image", ApplyNamingConventions = false)]
        public string Image { get; set; }

        [YamlMember(Alias = "Animation", ApplyNamingConventions = false)]
        public string Animation { get; set; }

        [YamlMember(Alias = "sound", ApplyNamingConventions = false)]
        public string Sound { get; set; }

        public AttractSequenceType GetSequenceType => AttractSequenceType.HighScores;
    }
}
