using Prism.Events;
using SkeletonGameManager.Base;

namespace SkeletonGameManager.Module.SceneManage.ViewModels
{
    class SequenceCreateViewModel : SequenceViewModelBase
    {
        public SequenceCreateViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider) : base(eventAggregator)
        {

        }
    }
}
