using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Transition
{
    public abstract class TransitionBaseLayer : SequenceBase, ITransitionLayer
    {
        [YamlMember(Alias = "transition", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public TransitionType Transition { get; set; }

        [YamlMember(Alias = "trans_param", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public TransitionParam TransParam { get; set; }

        [YamlMember(Alias = "trans_length", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public int TransLength { get; set; }
    }
}
