﻿using SkeletonGame.Models.Attract;
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

    public class SequenceYaml
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
            this.SeqType = SequenceType.ScriptedText;
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
        [YamlMember(Alias = "multiple_screens", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public bool MultipleScreens { get; set; } = false;

        public LastScores()
        {
            this.SeqType = SequenceType.LastScores;
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
            this.SeqType = SequenceType.Combo;
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
        [YamlMember(Alias = "x", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public string X { get; set; } = "0";

        [YamlMember(Alias = "y", ApplyNamingConventions = false, SerializeAs =typeof(string))]
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

        public TextLayer()
        {
            this.SeqType = SequenceType.TextLayer;
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
            this.SeqType = SequenceType.RandomText;            
        }

    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ZoomLayer : SequenceBase, IZoomLayer
    {
        
        [YamlMember(Alias = "hold", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public bool Hold { get; set; } = false;

        [YamlMember(Alias = "frames_per_zoom", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public int FramesPerZoom { get; set; } = 1;

        [YamlMember(Alias = "total_zooms", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public int TotalZooms { get; set; } = 30;

        [YamlMember(Alias = "scale_start", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public decimal ScaleStart { get; set; } = 0.1m;

        [YamlMember(Alias = "scale_stop", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public decimal ScaleStop { get; set; } = 1.0m;

        [YamlMember(Alias = "name", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public string Name { get; set; }

        [YamlMember(Alias = "enabled", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public bool IsEnabled { get; set; } = false;

        public ZoomLayer()
        {
            this.SeqType = SequenceType.ZoomLayer;
            this.duration = null;
            this.lampshow = null;
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

        [YamlMember(Alias = "move_layer", ApplyNamingConventions = false)]
        public MoveLayer move_layer { get; set; }

        [YamlMember(Alias = "Combo", ApplyNamingConventions = false)]
        public Combo combo_layer { get; set; }

        [YamlMember(Alias = "ScriptedText", ApplyNamingConventions = false)]
        public ScriptedText scripted_text_layer { get; set; }

        [YamlMember(Alias = "LastScores", ApplyNamingConventions = false)]
        public LastScores last_scores { get; set; }

        [YamlMember(Alias = "rotate_layer", ApplyNamingConventions = false)]
        public RotateLayer RotateLayer { get; set; }
        //C:\P-ROC\Games\Monkey
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class RotateLayer : IRotationLayer
    {
        public int x { get; set; }
        public int y { get; set; }

        [YamlMember(Alias = "rotation_update", ApplyNamingConventions = false)]
        public int RotateUpdate { get; set; } = 0;

        [YamlMember(Alias = "enabled", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public bool IsEnabled { get; set; } = false;

        public RotateLayer()
        {

        }
    }

    public interface IRotationLayer
    {
        int x { get; set; }

        int y { get; set; }

        int RotateUpdate { get; set; }
    }

    public interface IMoveLayer
    {
        int frames { get; set; }
        bool loop { get; set; }
        string start_x { get; set; }
        string start_y { get; set; }
        string target_x { get; set; }
        string target_y { get; set; }
    }

    public class MoveLayer : Combo, IMoveLayer
    {
        [YamlMember(Alias = "move_text", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public bool move_text { get; set; } = true;

        [YamlMember(Alias = "move_anim", ApplyNamingConventions = false)]
        public bool move_anim { get; set; } = true;

        [YamlMember(Alias = "frames", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public int frames { get; set; } = 15;

        [YamlMember(Alias = "loop", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public bool loop { get; set; } = false;

        [YamlMember(Alias = "target_x", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public string target_x { get; set; } = "0";

        [YamlMember(Alias = "target_y", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public string target_y { get; set; } = "0";

        [YamlMember(Alias = "start_x", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public string start_x { get; set; } = "0";

        [YamlMember(Alias = "start_y", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public string start_y { get; set; } = "0";

        [YamlMember(Alias = "enabled", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public string IsEnabled { get; set; }

        public MoveLayer()
        {
            this.SeqType = SequenceType.MoveLayer;            
        }
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
        public new  string Name { get; set; }

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
            this.SeqType = SequenceType.GroupLayer;
            this.duration = 5.0m;
        }

        public GroupLayer(SequenceBase sequenceBase) :base()
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

        [YamlMember(Alias = "anim_name", ApplyNamingConventions = false)]
        public string AnimName { get; set; } = "missing";

        [YamlMember(Alias = "name", ApplyNamingConventions = false)]
        public string Name { get; set; }

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
            this.AnimName = "missing";
            this.SeqType = SequenceType.Animation;
        }

        [YamlMember(Alias = "zoom_layer", ApplyNamingConventions = false)]
        public ZoomLayer ZoomLayer { get; set; } = new ZoomLayer();

        [YamlMember(Alias = "rotate_layer", ApplyNamingConventions = false)]
        public RotateLayer RotateLayer { get; set; } = new RotateLayer();

        [YamlMember(Alias = "move_layer", ApplyNamingConventions = false)]
        public MoveLayer MoveLayer { get; set; } = new MoveLayer();
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class HighScores : SequenceTextAnimationBase
    {

        [YamlMember(Alias = "Order", ApplyNamingConventions = false, SerializeAs = typeof(List<string>))]
        public ObservableCollection<string> Order { get; set; } = new ObservableCollection<string> { "player", "category", "score" };

        public HighScores()
        {
            this.SeqType = SequenceType.HighScores;
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
            this.SeqType = SequenceType.Credits;
        }
    }
}
