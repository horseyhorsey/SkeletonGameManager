using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
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
