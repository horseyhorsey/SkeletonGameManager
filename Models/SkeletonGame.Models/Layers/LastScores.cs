using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
    public class LastScores : SequenceTextAnimationBase
    {
        [YamlMember(Alias = "multiple_screens", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public bool MultipleScreens { get; set; } = false;

        public LastScores()
        {
            this.SeqType = SequenceType.LastScores;
        }
    }
}
