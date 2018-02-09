using System.Collections.Generic;
using System.Collections.ObjectModel;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
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
}
