using Prism.Commands;
using System.IO;
using Prism.Events;
using SkeletonGame.Engine;
using System.Collections.Generic;
using SkeletonGameManager.Base;
using System.Windows.Input;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class MainWindowViewModel : SkeletonGameManagerViewModelBase
    {
        #region Fields
        private ISkeletonGameProvider _skeletonGameProvider;
        private ISkeletonLogger _skeletonLogger;
        #endregion

        #region Commands

        public ICommand OpenFileFolderCommand { get; }
        #endregion

        #region Constructors

        public MainWindowViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider, ISkeletonLogger skeletonLogger) : base(eventAggregator)
        {
            _skeletonGameProvider = skeletonGameProvider;
            _skeletonLogger = skeletonLogger;
            IsGameRunning = false;

            _skeletonGameProvider.StatusChanged += _skeletonGameProvider_StatusChanged;

            OpenFileFolderCommand = new DelegateCommand<string>((x) =>
            {
                switch (x)
                {
                    case "asset_list.yaml":
                    case "new_score_display.yaml":
                    case "machine.yaml":
                    case "attract.yaml":
                    case "game_default_data.yaml":
                    case "game_default_settings.yaml":
                        FileFolder.Explore(Path.Combine(_skeletonGameProvider.GameFolder, "config", x));
                        break;
                    case "config.yaml":
                        FileFolder.Explore(Path.Combine(_skeletonGameProvider.GameFolder, x));
                        break;
                    default:
                        break;
                }
            });
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

        /// <summary>
        /// The last game launched log
        /// </summary>
        private IList<string> _lastgameLog;

        #endregion

        #region Private methods
        private void OnApplicationBusyChanged(bool isBusy)
        {
            this.IsMainTabEnabled = !isBusy;
        }

        private void _skeletonGameProvider_StatusChanged(object sender, ProviderUpdatedEventArgs e)
        {
            if (e.Status == 2)
                this.IsMainTabEnabled = false;
            else
                this.IsMainTabEnabled = true;
        }
        #endregion
    }
}

