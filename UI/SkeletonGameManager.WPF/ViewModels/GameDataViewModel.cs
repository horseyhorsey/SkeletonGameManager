using Prism.Events;
using SkeletonGameManager.Base;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class GameDataViewModel : SkeletonGameManagerViewModelBase
    {
        public GameDataViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }
    }
}
