using Prism.Events;
using SkeletonGameManager.WPF.Providers;

namespace SkeletonGameManager.WPF.ViewModels
{
    class SequenceCreateViewModel : SequenceViewModelBase
    {
        public SequenceCreateViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider) : base(eventAggregator)
        {

        }
    }
}
