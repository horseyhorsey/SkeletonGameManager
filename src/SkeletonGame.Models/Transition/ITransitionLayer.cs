using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Transition
{
    internal interface ITransitionLayer
    {
        [YamlMember(Alias = "transition", ApplyNamingConventions = false)]
        TransitionType Transition { get; set; }

        [YamlMember(Alias = "trans_param", ApplyNamingConventions = false)]
        TransitionParam TransParam { get; set; }

        [YamlMember(Alias = "trans_length", ApplyNamingConventions = false)]
        int TransLength { get; set; }
    }
}
