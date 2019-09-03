using Prism.Events;
using Prism.Logging;
using SkeletonGameManager.Base;

namespace SGM2.Wpf.ViewModels
{
    public class GameDataViewModel : SkeletonTabViewModel
    {
        
        public GameDataViewModel(IEventAggregator eventAggregator, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
            //Todo: empty?
            Title = "Game Data";
        }
    }
}
