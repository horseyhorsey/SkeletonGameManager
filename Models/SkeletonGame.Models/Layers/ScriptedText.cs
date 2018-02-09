using SkeletonGame.Models.Transition;
using System.Collections.ObjectModel;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ScriptedText : SequenceTextAnimationBase, ITransitionLayer
    {
        [YamlMember(Alias = "Name", ApplyNamingConventions = false)]
        public string Name { get; set; }

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
}
