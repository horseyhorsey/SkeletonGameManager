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
        public ObservableCollection<Sequence> AttractSequences { get; set; }

        [YamlIgnore]
        public ObservableCollection<SequenceBase> Sequences { get; set; } = new ObservableCollection<SequenceBase>();
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ScriptedText : SequenceTextAnimationBase, ITransitionLayer
    {
        [YamlMember(Alias = "Name", ApplyNamingConventions = false)]
        public new string Name { get; set; }

        [YamlMember(Alias = "flashing", ApplyNamingConventions = false)]
        public bool Flashing { get; set; }

        [YamlMember(Alias = "transition", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public TransitionType Transition { get; set; } = TransitionType.None;

        [YamlMember(Alias = "trans_param", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public TransitionParam TransParam { get; set; } = TransitionParam.None;

        [YamlMember(Alias = "trans_length", ApplyNamingConventions = false)]
        public int TransLength { get; set; } = 30;

        [YamlMember(Alias = "TextOptions", ApplyNamingConventions = false)]
        public ObservableCollection<TextOption> TextOptions { get; set; } = new ObservableCollection<TextOption>();

        public ScriptedText()
        {
            this.SeqType = AttractSequenceType.ScriptedText;
        }
    }

    public enum TransitionType
    {
        None,
        CrossFadeTransition,
        ExpandTransition,
        FadeTransition,
        ObscuredWipeTransition,
        PushTransition,
        SlideOverTransition,
        WipeTransition
    }

    public enum TransitionParam
    {
        None,
        horizontal,
        vertical,
        north,
        south,
        west,
        east,
        WipeTransition,
        In,
        Out
    }

    public class Message
    {
        [YamlMember(Alias = "TextOptions", ApplyNamingConventions = false)]
        public List<TextOption> TextOptions { get; set; }
    }


    public class LastScores : SequenceTextAnimationBase
    {
        public LastScores()
        {
            this.SeqType = AttractSequenceType.LastScores;
        }
    }

    //C:\P-ROC\Games\jaws
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Combo : SequenceTextAnimationBase, ITextEntries, ITransitionLayer
    {
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


        [YamlMember(Alias = "transition", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public TransitionType Transition { get; set; } = TransitionType.None;

        [YamlMember(Alias = "trans_param", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public TransitionParam TransParam { get; set; } = TransitionParam.None;

        [YamlMember(Alias = "trans_length", ApplyNamingConventions = false)]
        public int TransLength { get; set; } = 30;

        public Combo()
        {
            this.SeqType = AttractSequenceType.Combo;
        }
    }


    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class TextOption : ITextEntries
    {
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
        public ObservableCollection<TextListViewModel> TextEntries { get; set; } =
        new ObservableCollection<TextListViewModel>();

    }

    public class TextListViewModel
    {
        public string TextLine { get; set; } = "";
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class TextLayer : SequenceTextBase
    {
        [YamlMember(Alias = "x", ApplyNamingConventions = false)]
        public int X { get; set; } = 0;

        [YamlMember(Alias = "y", ApplyNamingConventions = false)]
        public int Y { get; set; }

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

        [YamlMember(Alias = "blink_frames", ApplyNamingConventions = false)]
        public string BlinkFrames { get; set; }

        public TextLayer()
        {
            this.SeqType = AttractSequenceType.TextLayer;
        }

    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class RandomText : SequenceTextAnimationBase
    {
        [YamlMember(Alias = "Header", ApplyNamingConventions = false)]
        public string Header { get; set; }

        [YamlMember(Alias = "TextOptions", ApplyNamingConventions = false)]
        public ObservableCollection<TextOption> TextOptions { get; set; } = new ObservableCollection<TextOption>();

        public RandomText()
        {
            this.SeqType = AttractSequenceType.RandomText;
        }

    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class AnimationLayer
    {
        [YamlMember(Alias = "name", ApplyNamingConventions = false)]
        public string Name { get; set; }
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Content : SequenceBase
    {        
        [YamlMember(Alias = "animation_layer", ApplyNamingConventions = false)]
        public AttractAnimation animation_layer { get; set; }

        [YamlMember(Alias = "group_layer", ApplyNamingConventions = false)]
        public GroupLayer group_layer { get; set; }

        [YamlMember(Alias = "markup_layer", ApplyNamingConventions = false)]
        public MarkupLayer markup_layer { get; set; }

        [YamlMember(Alias = "text_layer", ApplyNamingConventions = false)]
            public TextLayer text_layer { get; set; }

        [YamlMember(Alias = "Combo", ApplyNamingConventions = false)]
            public Combo combo_layer { get; set; }
    }

    public abstract class TransitionBaseLayer : SequenceBase, ITransitionLayer
    {
        [YamlMember(Alias = "transition", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public TransitionType Transition { get; set; }

        [YamlMember(Alias = "trans_param", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public TransitionParam TransParam{get; set;}

        [YamlMember(Alias = "trans_length", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public int TransLength { get; set; }
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class GroupLayer : TransitionBaseLayer
    {
        [YamlMember(Alias = "Name", ApplyNamingConventions = false)]
        public string Name { get; set; }

        [YamlMember(Alias = "width", ApplyNamingConventions = false)]
        public string Width { get; set; }

        [YamlMember(Alias = "height", ApplyNamingConventions = false)]
        public string Height { get; set; }

        [YamlMember(Alias = "opaque", ApplyNamingConventions = false)]
        public string Opaque { get; set; } = null;

        [YamlMember(Alias = "fill_color", ApplyNamingConventions = false)]
        public byte[] FillColor { get; set; } = null;

        [YamlMember(Alias = "contents", ApplyNamingConventions = false)]
        public ObservableCollection<Content> Contents { get; set; } = new ObservableCollection<Content>();

        public GroupLayer()
        {
            this.SeqType = AttractSequenceType.GroupLayer;
        }

        public GroupLayer(SequenceBase sequenceBase)
        {            
        }
    }

    internal interface ITransitionLayer
    {
        [YamlMember(Alias = "transition", ApplyNamingConventions = false)]
        TransitionType Transition { get; set; }

        [YamlMember(Alias = "trans_param", ApplyNamingConventions = false)]
        TransitionParam TransParam { get; set; }

        [YamlMember(Alias = "trans_length", ApplyNamingConventions = false)]
        int TransLength { get; set; }
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Contents
    {
        [YamlMember(Alias = "contents", ApplyNamingConventions = false)]
        public ObservableCollection<Content> Content { get; set; } = new ObservableCollection<Content>();
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class AttractAnimation : SequenceBase
    {
        [YamlMember(Alias = "name", ApplyNamingConventions = false)]
        public new string Name { get; set; } = "missing";

        [YamlMember(Alias = "x", ApplyNamingConventions = false)]
        public int X { get; set; } = 0;

        [YamlMember(Alias = "y", ApplyNamingConventions = false)]
        public int Y { get; set; }

        [YamlMember(Alias = "repeat", ApplyNamingConventions = false)]
        public bool Repeat { get; set; }

        [YamlMember(Alias = "hold_last_frame", ApplyNamingConventions = false)]
        public bool Hold { get; set; }

        [YamlMember(Alias = "opaque", ApplyNamingConventions = false)]
        public bool Opaque { get; set; }

        public AttractAnimation()
        {
            this.Name = "missing";
            this.SeqType = AttractSequenceType.Animation;
        }
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class HighScores : SequenceTextAnimationBase
    {

        [YamlMember(Alias = "Order", ApplyNamingConventions = false)]
        public List<string> Order { get; set; }

        public HighScores()
        {
            this.SeqType = AttractSequenceType.HighScores;
        }
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Credits : SequenceBase
    {
        [YamlMember(Alias = "Image", ApplyNamingConventions = false)]
        public string Image { get; set; }

        [YamlMember(Alias = "Animation", ApplyNamingConventions = false)]
        public string Animation { get; set; }

        public Credits()
        {
            this.SeqType = AttractSequenceType.Credits;
        }
    }
}
