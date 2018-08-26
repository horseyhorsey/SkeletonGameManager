using Prism.Events;
using Prism.Logging;
using Prism.Regions;

namespace SkeletonGameManager.Base
{
    public abstract class SkeletonTabViewModel : SkeletonGameManagerViewModelBase, INavigationAware
    {
        public SkeletonTabViewModel(IEventAggregator eventAggregator, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
        }

        #region Properties
        public string FileName { get; set; }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        } 
        #endregion

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            //Returning true finds the existing and activates it, return false to create new
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
