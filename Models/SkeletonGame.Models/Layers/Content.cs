using System;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
    [Serializable]
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

        [YamlMember(Alias = "solid_layer", ApplyNamingConventions = false)]
        public SolidLayer solid_layer { get; set; }

        #region Public Methods

        /// <summary>
        /// Gets the content layer that isn't null. A Sequene contains all but only one is not null
        /// </summary>
        /// <returns></returns>
        public SequenceBase GetNotNullContentLayer()
        {
            if (animation_layer != null)
                return animation_layer;
            else if (combo_layer != null)
                return combo_layer;
            else if (group_layer != null)
                return group_layer;
            else if (last_scores != null)
                return last_scores;
            else if (scripted_text_layer != null)
                return scripted_text_layer;
            else if (text_layer != null)
                return text_layer;
            else if (particle_layer != null)
                return particle_layer;

            return null;
        } 
        #endregion
    }
}
