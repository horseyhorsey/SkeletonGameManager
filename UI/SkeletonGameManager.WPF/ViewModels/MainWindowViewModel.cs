using Prism.Commands;
using System.IO;
using Prism.Events;
using SkeletonGame.Engine;
using SkeletonGameManager.Base;
using System.Windows.Input;
using Prism.Logging;
using System;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class MainWindowViewModel : SkeletonGameManagerViewModelBase
    {
        #region Fields
        private ISkeletonGameProvider _skeletonGameProvider;
        #endregion

        #region Commands
        public ICommand OpenFileFolderCommand { get; }
        #endregion

        #region Constructors

        public MainWindowViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider, 
            ISkeletonLogger skeletonLogger, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
            _skeletonGameProvider = skeletonGameProvider;            
            _skeletonGameProvider.StatusChanged += _skeletonGameProvider_StatusChanged;

            IsGameRunning = false;

            OpenFileFolderCommand = new DelegateCommand<string>(OnOpenFileFolder);

            Log("Initialized");
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
            Log($"Provider status changed. {e.Status}");

            if (e.Status == 2)
                this.IsMainTabEnabled = false;
            else
                this.IsMainTabEnabled = true;

            Log($"Main tab is enabled: {this.IsMainTabEnabled}");
        }
        #endregion
    }
}

