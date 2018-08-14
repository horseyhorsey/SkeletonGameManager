using Prism.Events;
using Prism.Logging;
using SkeletonGame.Models;

namespace SkeletonGameManager.Module.Assets.ViewModels
{
    public class LoadingProgressViewModel : AssetFileBaseViewModel
    {
        public LoadingProgressViewModel(UserInterface userInterface, IEventAggregator eventAggregator, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
            Title = "Loading";
        }

        #region Properties
        private UserInterface userInterface;
        public UserInterface UserInterface
        {
            get { return userInterface; }
            set { SetProperty(ref userInterface, value); }
        } 
        #endregion
    }
}
