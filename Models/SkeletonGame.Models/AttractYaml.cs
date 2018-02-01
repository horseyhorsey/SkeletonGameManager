using SkeletonGame.Models.Attract;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models
{
    public class AttractYaml
    {
        [YamlMember(Alias = "Sequence", ApplyNamingConventions = false)]
        public List<Sequence> AttractSequences { get; set; }

        [YamlIgnore]
        public ObservableCollection<SequenceBase> Sequences { get; set; } = new ObservableCollection<SequenceBase>();
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class SequenceBase
    {
        [YamlIgnore]
        public string Name { get; set; }

        public string lampshow { get; set; }
        public decimal duration { get; set; } = 1.0m;
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
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

        [YamlMember(Alias = "markup_layer", ApplyNamingConventions = false)]
        public MarkupLayer MarkupLayer { get; set; }
    }
    
    public class LastScores : SequenceBase
    {
        [YamlMember(Alias = "Font", ApplyNamingConventions = false)]
        public string Font { get; set; }

        [YamlMember(Alias = "FontStyle", ApplyNamingConventions = false)]
        public string FontStyle { get; set; }

        [YamlMember(Alias = "Animation", ApplyNamingConventions = false)]
        public string Animation { get; set; }

        [YamlMember(Alias = "sound", ApplyNamingConventions = false)]
        public string Sound { get; set; }
    }

    //C:\P-ROC\Games\jaws
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Combo : SequenceBase
    {        
        [YamlMember(Alias = "Text", ApplyNamingConventions = false)]        
        public ObservableCollection<string> TextList { get; set; }

        [YamlMember(Alias = "Animation", ApplyNamingConventions = false)]
        public string Animation { get; set; }

        [YamlMember(Alias = "sound", ApplyNamingConventions = false)]
        public string Sound { get; set; }

        [YamlMember(Alias = "Font", ApplyNamingConventions = false)]
        public string Font { get; set; }

        [YamlMember(Alias = "FontStyle", ApplyNamingConventions = false)]
        public string FontStyle { get; set; }
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

        [YamlMember(Alias = "blink_frames", ApplyNamingConventions = false)]
        public string BlinkFrames { get; set; }

    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
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

    }

    public class AnimationLayer
    {
        [YamlMember(Alias = "name", ApplyNamingConventions = false)]
        public string Name { get; set; }
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

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class TextOption
    {
        [YamlMember(Alias = "Text", ApplyNamingConventions = false)]
        public List<string> Text { get; set; }
    }

    public class AttractAnimation : SequenceBase
    {
        [YamlMember(Alias = "Name", ApplyNamingConventions = false)]
        public string Name { get; set; }

        [YamlMember(Alias = "sound", ApplyNamingConventions = false)]
        public string Sound { get; set; }

    }

    public class HighScores : SequenceBase
    {
        [YamlMember(Alias = "Font", ApplyNamingConventions = false)]
        public string Font { get; set; }

        [YamlMember(Alias = "FontStyle", ApplyNamingConventions = false)]
        public string FontStyle { get; set; }

        [YamlMember(Alias = "Animation", ApplyNamingConventions = false)]
        public string Animation { get; set; }

        [YamlMember(Alias = "Order", ApplyNamingConventions = false)]
        public List<string> Order { get; set; }
    }

    public class Credits : SequenceBase
    {
        [YamlMember(Alias = "Image", ApplyNamingConventions = false)]
        public string Image { get; set; }

        [YamlMember(Alias = "Animation", ApplyNamingConventions = false)]
        public string Animation { get; set; }

        [YamlMember(Alias = "sound", ApplyNamingConventions = false)]
        public string Sound { get; set; }
    }
}
