using Prism.Events;
using Prism.Logging;
using SkeletonGameManager.Base;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class GameDataViewModel : SkeletonGameManagerViewModelBase
    {
        public GameDataViewModel(IEventAggregator eventAggregator, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
        }
    }
}
