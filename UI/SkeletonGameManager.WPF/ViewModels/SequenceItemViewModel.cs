using Prism.Mvvm;
using SkeletonGame.Models;

namespace SkeletonGameManager.WPF.ViewModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class SequenceItemViewModel : BindableBase
    {
        public SequenceItemViewModel(SequenceBase sequence)
        {
            Sequence = sequence;
        }

        public SequenceBase Sequence { get; set; }
    }
}