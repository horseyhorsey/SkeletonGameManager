using Prism.Events;
using Prism.Logging;
using SkeletonGameManager.Base;

namespace SkeletonGameManager.Module.SceneManage.ViewModels
{
    class SequenceCreateViewModel : SequenceViewModelBase
    {
        public SequenceCreateViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider, 
            ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {

        }
    }
}
