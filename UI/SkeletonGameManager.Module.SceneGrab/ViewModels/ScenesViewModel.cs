using Prism.Events;
using Prism.Logging;
using SkeletonGameManager.Base;

namespace SkeletonGameManager.Module.SceneGrab.ViewModels
{
    public class ScenesViewModel : SkeletonGameManagerViewModelBase
    {
        public ScenesViewModel(IEventAggregator eventAggregator, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
            
        }
    }
}
