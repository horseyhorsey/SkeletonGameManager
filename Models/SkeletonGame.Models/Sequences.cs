using SkeletonGame.Models.Attract;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class SequenceBase
    {
        public string lampshow { get; set; }
        public decimal? duration { get; set; } = 1.0m;

        [YamlMember(Alias = "sound", ApplyNamingConventions = false)]
        public string Sound { get; set; }

        [YamlIgnore]
        public string Name { get; set; }

        [YamlIgnore]
        public AttractSequenceType SeqType { get; set; }
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

        public Sequence()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sequence"/> class from a <see cref="SequenceBase"/> <para/>
        /// </summary>
        /// <param name="sequenceBase">The sequence base.</param>
        public Sequence(SequenceBase sequenceBase)
        {
            switch (sequenceBase.SeqType)
            {
                case AttractSequenceType.Animation:
                    AttractAnimation = (AttractAnimation)sequenceBase;
                    break;
                case AttractSequenceType.Combo:
                    Combo = (Combo)sequenceBase;
                    break;
                case AttractSequenceType.Credits:
                    Credits = (Credits)sequenceBase;
                    break;
                case AttractSequenceType.GroupLayer:
                    GroupLayer = (GroupLayer)sequenceBase;
                    break;
                case AttractSequenceType.HighScores:
                    HighScores = (HighScores)sequenceBase;
                    break;
                case AttractSequenceType.LastScores:
                    LastScores = (LastScores)sequenceBase;
                    break;
                case AttractSequenceType.MarkupLayer:
                    MarkupLayer = (MarkupLayer)sequenceBase;
                    break;
                case AttractSequenceType.PanningLayer:
                    panning_layer = (PanningLayer)sequenceBase;
                    break;
                case AttractSequenceType.RandomText:
                    RandomText = (RandomText)sequenceBase;
                    break;
                case AttractSequenceType.ScriptedText:
                    ScriptedText = (ScriptedText)sequenceBase;
                    break;
                case AttractSequenceType.TextLayer:
                    text_layer = (TextLayer)sequenceBase;
                    break;
                default:
                    break;
            }
        }
    }

    public class SequenceTextBase : SequenceBase
    {
        [YamlMember(Alias = "Font", ApplyNamingConventions = false)]
        public string Font { get; set; }

        [YamlMember(Alias = "FontStyle", ApplyNamingConventions = false)]
        public string FontStyle { get; set; }
    }

    public class SequenceTextAnimationBase : SequenceTextBase
    {
        [YamlMember(Alias = "Animation", ApplyNamingConventions = false)]
        public string Animation { get; set; } = "missing";
    }
}
