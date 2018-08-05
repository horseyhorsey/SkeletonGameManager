using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SkeletonGameManager.Base;
using SkeletonGameManager.Module.Menus.Views;
using SkeletonGameManager.Module.Recordings.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SkeletonGameManager.Base.Events;

namespace SkeletonGameManager.Module.Menus.ViewModels
{
    public class FileMenuViewModel : BindableBase
    {
        #region Fields
        private IEventAggregator _eventAggregator;
        private ISkeletonGameProvider _skeletonGameProvider;
        private IGameRunnner _gameRunnner;        
        #endregion

        #region Commands
        public DelegateCommand CreateNewGameCommand { get; }
        public DelegateCommand OpenGameFolderCommand { get; }
        public DelegateCommand SetDirectoryCommand { get;  }
        public DelegateCommand ReloadGameCommand { get; }
        public DelegateCommand<string> OpenRecentCommand { get; }        
        public DelegateCommand LaunchGameCommand { get;}
        public DelegateCommand<string> ExportCommand { get; }
        public DelegateCommand BrowseFolderCommand { get; }
        public DelegateCommand<string> LaunchRecordingCommand { get; }
        #endregion

        public List<string> RecentDirectories { get; set; } = new List<string>();

        #region Constructors
        public FileMenuViewModel(ISkeletonGameProvider skeletonGameProvider, IGameRunnner gameRunnner, 
            IEventAggregator eventAggregator, IUnityContainer unityContainer)
        {
            _eventAggregator = eventAggregator;
            _skeletonGameProvider = skeletonGameProvider;
            _gameRunnner = gameRunnner;

            //Recent dirs
            RecentDirectories.AddRange(new string[] { @"C:\P-ROC\Games\Jaws", @"C:\P-ROC\Games\EvilDead" });

            //Get the recordings stored from containers view model
            Recordings = unityContainer.Resolve<RecordingsViewModel>().PlaybackItemViewModels;

            LaunchRecordingCommand = new DelegateCommand<string>((playbackItem) =>
            {
                if (playbackItem != null)
                {
                    IsGameRunning = true;
                    UpdateCanExecuteCommands();
                    unityContainer.Resolve<RecordingsViewModel>().LaunchPlaybackFile(_skeletonGameProvider.GameFolder, playbackItem, true);                    
                }
                    
            }, (x) => !IsGameRunning);

            _eventAggregator.GetEvent<OnLaunchGameEvent>().Subscribe(async (x) =>
            {
                IsGameRunning = true;
                await OnLaunchedGame();
            });

            #region Commands
            BrowseFolderCommand = new DelegateCommand(() => FileFolder.Explore(_skeletonGameProvider.GameFolder), () => IsValidGameFolder());
            CreateNewGameCommand = new DelegateCommand(OnCreateNewGame, () => true);
            ExportCommand = new DelegateCommand<string>(OnExport, (x) => IsValidGameFolder());
            LaunchGameCommand = new DelegateCommand(async () =>
            {
                IsGameRunning = true;
                await OnLaunchedGame();

            }, () => IsValidGameFolder());
            OpenGameFolderCommand = new DelegateCommand(() => Process.Start(skeletonGameProvider.GameFolder), () => IsValidGameFolder());
            OpenRecentCommand = new DelegateCommand<string>(OnOpenRecent);
            ReloadGameCommand = new DelegateCommand(async () => await OnReloadGame(), () => IsValidGameFolder());
            SetDirectoryCommand = new DelegateCommand(() => OnSetDirectory(), () => !IsGameRunning);
            #endregion

        }
        #endregion

        #region Properties        

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

        private string gameFolder;
        /// <summary>
        /// Gets or sets the game folder and applies it to the underlying provider
        /// </summary>
        public string GameFolder
        {
            get { return gameFolder; }
            set
            {
                SetProperty(ref gameFolder, value);
                _skeletonGameProvider.GameFolder = value;

                UpdateCanExecuteCommands();
            }
        }

        public ObservableCollection<PlaybackItemViewModel> Recordings { get; private set; }
        #endregion

        #region Private Methods

        /// <summary>
        /// Launches the current game. Checks if python  can run from Enviroment. If not, points to download skeletongame.com
        /// </summary>
        private async Task OnLaunchedGame()
        {
            if (_skeletonGameProvider.GameFolder == null) return;

            var gameEntryPointFile = Path.Combine(_skeletonGameProvider.GameFolder, "game.py");

            if (!File.Exists(gameEntryPointFile))
                return;

            UpdateCanExecuteCommands();

            try
            {
                //Clear the previous log.
                //_skeletonLogger.LogData.Clear();

                await _gameRunnner.Run(_skeletonGameProvider.GameFolder, "game.py");
            }
            //catch (FileNotFoundException ex)
            //{
            //    var result = System.Windows.MessageBox.Show($"{ex.Message}. Download skeleton game installer with python?", "", MessageBoxButton.YesNo);

            //    if (result == MessageBoxResult.Yes)
            //        Process.Start(@"http://skeletongame.com/step-1-installation-and-testing-the-install-windows/");
            //}
            catch (Exception ex)
            {

            }
            finally
            {
                IsGameRunning = false;

                UpdateCanExecuteCommands();

                //if (_skeletonLogger?.LogData?.Count > 0)
                //{
                //    _lastgameLog = _skeletonLogger.LogData;
                //    _skeletonLogger.LogToFile(Path.Combine(GameFolder, "Logs"));
                //}

                _eventAggregator.GetEvent<OnGameEndedEvent>().Publish(true);
            }
        }

        /// <summary>
        /// Sets the current skeleton game folder path and loads the configuration for a game if a valid folder is given
        /// </summary>
        private async void OnSetDirectory()
        {
            var dlg = new FolderBrowserDialog();
            dlg.SelectedPath = @"C:\P-ROC";
            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                await SetGamePath(dlg.SelectedPath);
            };
        }

        /// <summary>
        /// Called from command when user requests to open a recent folder
        /// </summary>
        /// <param name="obj">The object.</param>
        private async void OnOpenRecent(string obj)
        {
            try
            {
                await SetGamePath(obj);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Sets the game path folder. Clears configs.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private Task SetGamePath(string path)
        {
            _skeletonGameProvider.ClearConfigs();            
            GameFolder = path;

            if (!IsValidGameFolder())
            {
                System.Windows.MessageBox.Show($"Not a valid Game folder -  {GameFolder}");
                GameFolder = null;
                return null;
            }
            else
                return this.OnReloadGame();
        }

        /// <summary>
        /// Determines whether [is valid game folder] by making sure folder exists and has a valid config.yaml
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is valid game folder]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidGameFolder()
        {
            if (IsGameRunning) return false;

            if (Directory.Exists(GameFolder))
                if (File.Exists(Path.Combine(GameFolder, "config.yaml")))
                {
                    if (_skeletonGameProvider.GameConfig != null)
                    {
                        //IsMainTabEnabled = true;
                    }
                        
                    return true;
                }

            //IsMainTabEnabled = false;
            return false;
        }

        /// <summary>
        /// Called when [create new game] to show the Create New Game window
        /// </summary>
        private void OnCreateNewGame()
        {
            //TODO: Use Prism request dialog
            var vm = new CreateNewGameWindowViewModel();
            var window = new CreateNewGameWindow();
            window.DataContext = vm;

            var dialog = window.ShowDialog();
        }

        private void OnExport(string exportParam)
        {
            switch (exportParam)
            {
                case "switch":
                case "coil":
                case "ScriptFull":
                    _skeletonGameProvider.ExportVpScript(exportParam);
                    break;
                case "switchPy":
                case "lampshowUi":
                    _skeletonGameProvider.ExportPyProcgame(exportParam);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Called when [refresh skeleton game objects], loads all yaml files into the provider
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private async Task OnReloadGame()
        {
            try
            {
                await _skeletonGameProvider.LoadYamlEntriesAsync();

                _eventAggregator.GetEvent<LoadYamlFilesChanged>().Publish(null);

                //Disable machine tab if failed top parse
                //IsMachineConfigEnabled = _skeletonGameProvider.MachineConfig == null ? false : true;
            }
            catch (Exception ex)
            {
                //IsMainTabEnabled = false;

                System.Windows.MessageBox.Show($"Failed loading skeleton game files. {ex.Data["yaml"]} {ex.Message}");
            }
            finally
            {
                UpdateCanExecuteCommands();
            }
        }

        /// <summary>
        /// Updates the can execute commands.
        /// </summary>
        private void UpdateCanExecuteCommands()
        {
            LaunchGameCommand.RaiseCanExecuteChanged();
            CreateNewGameCommand.RaiseCanExecuteChanged();
            ReloadGameCommand.RaiseCanExecuteChanged();            
            SetDirectoryCommand.RaiseCanExecuteChanged();
            ExportCommand.RaiseCanExecuteChanged();
            LaunchRecordingCommand.RaiseCanExecuteChanged();
        }


        #endregion
    }
}
