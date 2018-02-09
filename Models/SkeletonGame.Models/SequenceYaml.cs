using System.Collections.ObjectModel;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models
{
    /// <summary>
    /// This is the Root for sequence yaml files. The attract.yaml is serialized against this class and so are any other yamls that hold a Sequence.
    /// </summary>
    public class SequenceYaml
    {
        [YamlMember(Alias = "Sequence", ApplyNamingConventions = false)]
        public ObservableCollection<Sequence> AttractSequences { get; set; }

        [YamlIgnore]
        public ObservableCollection<SequenceBase> Sequences { get; set; } = new ObservableCollection<SequenceBase>();
    }
}
