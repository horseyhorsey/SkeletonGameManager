using Prism.Commands;
using System.IO;
using Prism.Events;
using SkeletonGame.Engine;
using SkeletonGameManager.Base;
using System.Windows.Input;
using Prism.Logging;
using System;
using Prism.Regions;
using System.Diagnostics;
using Prism.Interactivity.InteractionRequest;
using static SkeletonGameManager.Base.Events;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class MainWindowViewModel : SkeletonGameManagerViewModelBase
    {
        #region Fields
        private ISkeletonGameProvider _skeletonGameProvider;
        public InteractionRequest<INotification> NotificationRequest { get; private set; }
        #endregion

        #region Commands        
        public ICommand CloseTabCommand { get; }
        public ICommand OpenFileFolderCommand { get; }
        public ICommand TestCommand { get; set; }
        #endregion

        #region Constructors

        public MainWindowViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider, 
            ISkeletonLogger skeletonLogger, ILoggerFacade loggerFacade, IRegionManager regionManager) : base(eventAggregator, loggerFacade)
        {
            _skeletonGameProvider = skeletonGameProvider;            
            _skeletonGameProvider.StatusChanged += _skeletonGameProvider_StatusChanged;
            _regionManager = regionManager;

            IsGameRunning = false;

            OpenFileFolderCommand = new DelegateCommand<string>(OnOpenFileFolder);

            CloseTabCommand = new DelegateCommand<object>(OnCloseTab);
            
            Log("".PadRight(25, '*'));
            Log($"Skeleton Game Manager : {Base.Helpers.Versions.GetVersion()}");
            Log("".PadRight(25, '*'));

            NotificationRequest = new InteractionRequest<INotification>();

            eventAggregator.GetEvent<ErrorMessageEvent>().Subscribe((x) =>
            {
                NotificationRequest.Raise(new Notification
                {
                    Content = x,
                    Title = "Error"
                });
            }, ThreadOption.UIThread);
            
        }
        #endregion

        #region Properties

        private bool isMainTabEnabled = false;
        /// <summary>
        /// Gets or sets the IsMainTabEnabled to enable the main tab. Should be disabled when user hasn't loaded a games folder config.yaml
        /// </summary>
        public bool IsMainTabEnabled
        {
            get { return isMainTabEnabled; }
            set
            {
                SetProperty(ref isMainTabEnabled, value);
            }
        }

        private bool isMachineConfigEnabled = false;
        /// <summary>
        /// Gets or sets the IsMachineConfigEnabled to enable the machine config tab. Should be disabled when a machine.yaml fails to parse
        /// </summary>
        public bool IsMachineConfigEnabled
        {
            get { return isMachineConfigEnabled; }
            set
            {
                SetProperty(ref isMachineConfigEnabled, value);
            }
        }

        private bool isGameRunning = false;
        private IRegionManager _regionManager;

        /// <summary>
        /// Gets or sets the IsGameRunning.
        /// </summary>
        public bool IsGameRunning
        {
            get { return isGameRunning; }
            set
            {
                SetProperty(ref isGameRunning, value);
            }
        }

        #endregion

        #region Private methods

        private void OnApplicationBusyChanged(bool isBusy)
        {
            this.IsMainTabEnabled = !isBusy;
        }

        /// <summary>
        /// Called when [close tab] to close a view /tab from the OpenTabsRegion.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void OnCloseTab(object obj)
        {
            IRegion region = _regionManager.Regions["OpenTabsRegion"];
            if (region.Views.Contains(obj))
            {                
                region.Remove(obj);
                Log("Removed tab: " + obj);
            }
        }

        /// <summary>
        /// Called when [open file folder]. Deals with opening certain SkeletonGame files from filename.
        /// </summary>
        /// <param name="path">The path.</param>
        private void OnOpenFileFolder(string path)
        {
            Log($"Attempting to open file. {path}");

            try
            {
                switch (path)
                {
                    case "asset_list.yaml":
                    case "new_score_display.yaml":
                    case "machine.yaml":
                    case "attract.yaml":
                    case "game_default_data.yaml":
                    case "game_default_settings.yaml":
                        FileFolder.Explore(Path.Combine(_skeletonGameProvider.GameFolder, "config", path));
                        break;
                    case "config.yaml":
                        FileFolder.Explore(Path.Combine(_skeletonGameProvider.GameFolder, path));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log($"Error loading file {path}. {ex.Message}", Category.Warn);
            }
        }

        private void _skeletonGameProvider_StatusChanged(object sender, ProviderUpdatedEventArgs e)
        {            
            if (e.Status == 2)
                this.IsMainTabEnabled = false;
            else
                this.IsMainTabEnabled = true;

            if (!String.IsNullOrWhiteSpace(_skeletonGameProvider.GameFolder))
                this.Title = "Skeleton Game Manager: " + _skeletonGameProvider.GameFolder;
            else
                this.Title = "Skeleton Game Manager";            
        }
        #endregion
    }
}

