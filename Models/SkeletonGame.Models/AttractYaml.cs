using SkeletonGame.Models.Attract;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models
{
    /// <summary>
    /// Interface for sequences with text lists.
    /// </summary>
    public interface ITextEntries
    {
        List<string> TextList { get; set; }

        ObservableCollection<TextListViewModel> TextEntries { get; set; }

    }

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

        [YamlMember(Alias = "sound", ApplyNamingConventions = false)]
        public string Sound { get; set; }


        //[YamlMember(Alias = "Animation", ApplyNamingConventions = false)]
        //public AnimationLayer AnimationLayer { get; set; } = null;

        [YamlMember(Alias = "markup_layer", ApplyNamingConventions = false)]
        public MarkupLayer MarkupLayer { get; set; } = null;
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
        public AttractAnimation AttractAnimation { get; set; }

        [YamlMember(Alias = "HighScores", ApplyNamingConventions = false)]
        public HighScores HighScores { get; set; }

        [YamlMember(Alias = "Credits", ApplyNamingConventions = false)]
        public Credits Credits { get; set; }

        [YamlMember(Alias = "markup_layer", ApplyNamingConventions = false)]
        public MarkupLayer MarkupLayer { get; set; }

        [YamlMember(Alias = "group_layer", ApplyNamingConventions = false)]
        public GroupLayer GroupLayer { get; set; }

        [YamlMember(Alias = "ScriptedText", ApplyNamingConventions = false)]
        public ScriptedText ScriptedText { get; set; }
    }

    public class ScriptedText : SequenceBase
    {
        [YamlMember(Alias = "Name", ApplyNamingConventions = false)]
        public new string Name { get; set; }

        [YamlMember(Alias = "Font", ApplyNamingConventions = false)]
        public string Font { get; set; }

        [YamlMember(Alias = "FontStyle", ApplyNamingConventions = false)]
        public string FontStyle { get; set; }

        [YamlMember(Alias = "Animation", ApplyNamingConventions = false)]
        public string Animation { get; set; }

        [YamlMember(Alias = "flashing", ApplyNamingConventions = false)]
        public bool Flashing { get; set; }

        [YamlMember(Alias = "TextOptions", ApplyNamingConventions = false)]
        public List<TextOption> TextOptions { get; set; }

    }

    public class Message
    {
        [YamlMember(Alias = "TextOptions", ApplyNamingConventions = false)]
        public List<TextOption> TextOptions { get; set; }
    }


    public class LastScores : SequenceBase
    {
        [YamlMember(Alias = "Font", ApplyNamingConventions = false)]
        public string Font { get; set; }

        [YamlMember(Alias = "FontStyle", ApplyNamingConventions = false)]
        public string FontStyle { get; set; }

        [YamlMember(Alias = "Animation", ApplyNamingConventions = false)]
        public string Animation { get; set; }
    }

    //C:\P-ROC\Games\jaws
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Combo : SequenceBase, ITextEntries
    {
        [YamlMember(Alias = "Animation", ApplyNamingConventions = false)]
        public string Animation { get; set; }

        [YamlMember(Alias = "Font", ApplyNamingConventions = false)]
        public string Font { get; set; } = string.Empty;

        [YamlMember(Alias = "FontStyle", ApplyNamingConventions = false)]
        public string FontStyle { get; set; }

        private List<string> _textList;
        [YamlMember(Alias = "Text", ApplyNamingConventions = false)]
        public List<string> TextList
        {
            get
            {
                return _textList;
            }

            set
            {
                _textList = value;

                if (_textList != null)
                {
                    TextEntries.Clear();
                    foreach (var textLine in _textList)
                    {
                        TextEntries.Add(new TextListViewModel { TextLine = textLine });
                    }
                }
                else
                {
                    TextEntries.Clear();
                }
            }
        }

        [YamlIgnore]
        public ObservableCollection<TextListViewModel> TextEntries { get; set; } = new ObservableCollection<TextListViewModel>();
    }
    
    public class TextListViewModel
    {
        public string TextLine { get; set; }
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class TextLayer : SequenceBase
    {
        [YamlMember(Alias = "x", ApplyNamingConventions = false)]
        public string X { get; set; }

        [YamlMember(Alias = "y", ApplyNamingConventions = false)]
        public string Y { get; set; }

        [YamlMember(Alias = "v_justify", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public VJustify VJustify { get; set; }

        [YamlMember(Alias = "h_justify", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public HJustify HJustify { get; set; }

        [YamlMember(Alias = "Text", ApplyNamingConventions = false)]
        public string Text { get; set; }

        [YamlMember(Alias = "Font", ApplyNamingConventions = false)]
        public string Font { get; set; }

        [YamlMember(Alias = "Background", ApplyNamingConventions = false)]
        public string Animation { get; set; }

        [YamlMember(Alias = "blink_frames", ApplyNamingConventions = false)]
        public string BlinkFrames { get; set; }

        [YamlMember(Alias = "FontStyle", ApplyNamingConventions = false)]
        public string FontStyle { get; set; }

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

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class AnimationLayer
    {
        [YamlMember(Alias = "Name", ApplyNamingConventions = false)]
        public string Name { get; set; }
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Content : SequenceBase
    {
        [YamlMember(Alias = "Animation", ApplyNamingConventions = false)]
        public AnimationLayer AnimationLayer { get; set; }

        [YamlMember(Alias = "markup_layer", ApplyNamingConventions = false)]
        public MarkupLayer MarkupLayer { get; set; }
    }

    public abstract class GroupBaseLayer : SequenceBase
    {
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class GroupLayer : GroupBaseLayer
    {
        [YamlMember(Alias = "width", ApplyNamingConventions = false)]
        public string Width { get; set; }

        [YamlMember(Alias = "height", ApplyNamingConventions = false)]
        public string Height { get; set; }

        [YamlMember(Alias = "opaque", ApplyNamingConventions = false)]
        public string Opaque { get; set; } = null;

        [YamlMember(Alias = "fill_color", ApplyNamingConventions = false)]
        public byte[] FillColor { get; set; } = null;

        [YamlMember(Alias = "contents", ApplyNamingConventions = false)]
        public List<Content> Layers { get; set; } = new List<Content>();
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Contents
    {
        [YamlMember(Alias = "group_layer", ApplyNamingConventions = false)]
        public GroupLayer GroupedLayer { get; set; } = new GroupLayer();
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class TextOption : ITextEntries
    {
        [YamlMember(Alias = "Text", ApplyNamingConventions = false)]
        public List<string> TextList { get; set; }

        public ObservableCollection<TextListViewModel> TextEntries { get; set; }
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class AttractAnimation : SequenceBase
    {
        [YamlMember(Alias = "Name", ApplyNamingConventions = false)]
        public new string Name { get; set; }
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
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

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Credits : SequenceBase
    {
        [YamlMember(Alias = "Image", ApplyNamingConventions = false)]
        public string Image { get; set; }

        [YamlMember(Alias = "Animation", ApplyNamingConventions = false)]
        public string Animation { get; set; }
    }
}
