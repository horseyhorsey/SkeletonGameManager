using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
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

        [YamlMember(Alias = "particle_layer", ApplyNamingConventions = false)]
        public ParticleLayer particle_layer { get; set; }
    }
}
