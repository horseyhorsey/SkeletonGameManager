using System;
using System.Collections.ObjectModel;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
    [Serializable]
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class RandomText : SequenceTextAnimationBase
    {
        [YamlMember(Alias = "Header", ApplyNamingConventions = false)]
        public string Header { get; set; }

        [YamlMember(Alias = "TextOptions", ApplyNamingConventions = false)]
        public ObservableCollection<TextOption> TextOptions { get; set; } = new ObservableCollection<TextOption>();

        public RandomText()
        {
            this.SeqType = SequenceType.RandomText;
        }

    }
}
