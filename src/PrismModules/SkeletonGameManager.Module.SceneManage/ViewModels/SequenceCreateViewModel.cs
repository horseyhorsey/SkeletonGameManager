using Prism.Events;
using Prism.Logging;
using SkeletonGame.Engine;
using SkeletonGameManager.Base;

namespace SkeletonGameManager.Module.SceneManage.ViewModels
{
    class SequenceCreateViewModel : SequenceViewModelBase
    {
        public SequenceCreateViewModel(ISkeletonGameSerializer skeletonGameSerializer, IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider, 
            ILoggerFacade loggerFacade) : base(skeletonGameSerializer, eventAggregator, loggerFacade)
        {

        }
    }
}
