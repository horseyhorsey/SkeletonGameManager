using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Logging;
using Prism.Regions;
using SkeletonGame.Models;
using SkeletonGameManager.Base;
using SkeletonGameManager.Base.Interface;
using SkeletonGameManager.Module.Menus.Model;
using SkeletonGameManager.Module.Recordings.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SkeletonGameManager.Base.Events;

namespace SkeletonGameManager.Module.Menus.ViewModels
{
    public class FileMenuViewModel : SkeletonGameManagerViewModelBase
    {
        #region Fields        
        private ISkeletonGameProvider _skeletonGameProvider;
        private IGameRunnner _gameRunnner;
        private IAppSettingsModel _appSettingsModel;
        private IRegionManager _regionManager;
        #endregion

        #region Commands
        public DelegateCommand BrowseFolderCommand { get; }
        public DelegateCommand CreateNewGameCommand { get; }
        public DelegateCommand<string> ExportCommand { get; }
        public DelegateCommand LaunchGameCommand { get; }
        public DelegateCommand<string> LaunchRecordingCommand { get; }
        public DelegateCommand<string> LaunchToolCommand { get; }
        public DelegateCommand<string> NavigateCommand { get; set; }
        public DelegateCommand OpenGameFolderCommand { get; }
        public DelegateCommand<string> OpenRecentCommand { get; }
        public DelegateCommand ReloadGameCommand { get; }
        public DelegateCommand SetDirectoryCommand { get; }                      
        #endregion

        #region Requests
        public InteractionRequest<IRequestNewGame> CreateNewGameRequest { get; set; }
        #endregion

        #region Constructors

        public FileMenuViewModel(ISkeletonGameProvider skeletonGameProvider, IGameRunnner gameRunnner, IAppSettingsModel appSettingsModel,
            IEventAggregator eventAggregator, IUnityContainer unityContainer, IRegionManager regionManager,
            ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
            _skeletonGameProvider = skeletonGameProvider;
            _regionManager = regionManager;
            _gameRunnner = gameRunnner;
            _appSettingsModel = appSettingsModel;
            _appSettingsModel.Load();

            if (_eventAggregator == null)
                _eventAggregator = eventAggregator;

            RecentDirectories = appSettingsModel.RecentDirectories;

            CreateNewGameRequest = new InteractionRequest<IRequestNewGame>();

            //Recent dirs
            //RecentDirectories.AddRange(new string[] { @"C:\P-ROC\Games\Jaws", @"C:\P-ROC\Games\EvilDead" });

            //Get the recordings stored from containers view model
            Recordings = unityContainer.Resolve<RecordingsViewModel>().PlaybackItemViewModels;
            _eventAggregator.GetEvent<OnLaunchGameEvent>().Subscribe(async (x) =>
            {
                IsGameRunning = true;
                await OnLaunchedGame();
            });

            #region Commands
            BrowseFolderCommand = new DelegateCommand(() => FileFolder.Explore(_skeletonGameProvider.GameFolder), () => GameFolder != null);
            CreateNewGameCommand = new DelegateCommand(OnCreateNewGame, () => !IsGameRunning);
            ExportCommand = new DelegateCommand<string>(OnExport, (x) => GameFolder != null);
            LaunchGameCommand = new DelegateCommand(async () =>
            {
                IsGameRunning = true;
                await OnLaunchedGame();

            }, () => GameNotRunningAndFolderValid());
            LaunchRecordingCommand = new DelegateCommand<string>((playbackItem) => { OnLaunchRecordings(unityContainer, playbackItem); }, (x) => GameNotRunningAndFolderValid());
            LaunchToolCommand = new DelegateCommand<string>((toolName) => { OnLaunchTool(toolName); }, (x) => GameFolder != null);

            OpenRecentCommand = new DelegateCommand<string>(OnOpenRecent, (x) => !IsGameRunning);
            ReloadGameCommand = new DelegateCommand(async () => await OnReloadGame(), () => GameNotRunningAndFolderValid());
            SetDirectoryCommand = new DelegateCommand(() => OnSetDirectory(), () => !IsGameRunning);

            NavigateCommand = new DelegateCommand<string>(OnNavigate, (x) => GameFolder != null);
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

        private ObservableCollection<string> _recentDirectories = new ObservableCollection<string>();
        public ObservableCollection<string> RecentDirectories
        {
            get { return _recentDirectories; }
            set { SetProperty(ref _recentDirectories, value); }
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

            //TODO: what if dev / user has an entry file other than game.py?
            var entryFile = Path.Combine(_skeletonGameProvider.GameFolder, "game.py");
            if (!File.Exists(entryFile))
            {
                var msg = $"{entryFile} doesn't exist.";
                Log(msg, Category.Exception);
                _eventAggregator.GetEvent<ErrorMessageEvent>().Publish(msg);
                return;
            }

            UpdateCanExecuteCommands();

            Log("Attempting to launch game through sg_runner");
            try
            {                
                await _gameRunnner.Run(_skeletonGameProvider.GameFolder, "game.py");
                _eventAggregator.GetEvent<OnGameEndedEvent>().Publish(true);

            }
            //catch (FileNotFoundException ex)
            //{
            //    var result = System.Windows.MessageBox.Show($"{ex.Message}. Download skeleton game installer with python?", "", MessageBoxButton.YesNo);

            //    if (result == MessageBoxResult.Yes)
            //        Process.Start(@"http://skeletongame.com/step-1-installation-and-testing-the-install-windows/");
            //}
            catch (Exception ex)
            {
                Log($"Error loading game. {ex.Message}", Category.Exception);
                _eventAggregator.GetEvent<OnGameEndedEvent>().Publish(false);
            }
            finally
            {
                IsGameRunning = false;

                UpdateCanExecuteCommands();

                _eventAggregator.GetEvent<OnGameEndedEvent>().Publish(true);
            }
        }

        private void OnLaunchRecordings(IUnityContainer unityContainer, string playbackItem)
        {
            if (playbackItem != null)
            {
                IsGameRunning = true;
                UpdateCanExecuteCommands();
                unityContainer.Resolve<RecordingsViewModel>().LaunchPlaybackFile(_skeletonGameProvider.GameFolder, playbackItem, true);
            }
        }

        private bool GameNotRunningAndFolderValid() => GameFolder != null && !IsGameRunning;

        private void OnLaunchTool(string toolName)
        {
            Log("Launching external tool");
            try
            {
                switch (toolName)
                {
                    case "SwitchMatrixClient":
                        LaunchSwitchMatrix();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log($"{ex.Message}");
                _eventAggregator.GetEvent<ErrorMessageEvent>().Publish($"{ex.Message}");
            }            

        }

        private void LaunchSwitchMatrix()
        {
            Log("Attempting to launch SwitchMatrixClient");

            var switchMatrixClient = _skeletonGameProvider.GameConfig.OscUiPath;
            var layout = _skeletonGameProvider.GameConfig.OscUiLayout;
            var playfield = _skeletonGameProvider.GameConfig.OscUiPlayfield;

            //Throw errors if user has no files
            if (!File.Exists(switchMatrixClient))
                throw new FileNotFoundException($"Couldn't find Switch Matrix Client file. {switchMatrixClient}");
            else if (!File.Exists(layout))
                throw new FileNotFoundException($"Couldn't find Switch Matrix layout file. {layout}");
            else if (!File.Exists(playfield))
                throw new FileNotFoundException($"Couldn't find Switch Matrix playfield file. {playfield}");

            //Build args to run the game.py with python
            var startInfo = new ProcessStartInfo("python");
            startInfo.WorkingDirectory = Path.GetDirectoryName(switchMatrixClient);
            startInfo.Arguments = $"\"{switchMatrixClient}\" -l \"{layout}\" -i \"{playfield}\" -y \"{Path.Combine(GameFolder, Constants.FILE_MACHINE)}\"";

            Process.Start(startInfo);
        }

        private void OnNavigate(string navParam)
        {
            Log($"Navigating to {navParam}");
            _regionManager.RequestNavigate("OpenTabsRegion", navParam);
        }

        /// <summary>
        /// Sets the current skeleton game folder path and loads the configuration for a game if a valid folder is given
        /// </summary>
        private async void OnSetDirectory()
        {
            Log("Setting game directory");
            CloseAllTabs();

            var dlg = new FolderBrowserDialog();            
            dlg.SelectedPath = @"C:\P-ROC";
            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                Log($"Selected Folder: {dlg.SelectedPath}");

                await SetGamePath(dlg.SelectedPath);
            };
        }

        private void CloseAllTabs()
        {
            Log("Closing all open tabs.");

            IRegion region = _regionManager.Regions["OpenTabsRegion"];
            if (region != null)
            {
                region.RemoveAll();
                Log("All tabs closed.");
            }
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
            catch (Exception ex)
            {
                Log(ex.Message, Category.Exception);
            }
        }

        /// <summary>
        /// Sets the game path folder. Clears configs. If it isn't a valid game then game folder is set null and menu disabled.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private Task SetGamePath(string path)
        {
            Log("Clearing UI configuration");            
            GameFolder = path;

            if (!IsValidGameFolder())
            {
                var msg = $"Not a valid Game folder -  {GameFolder}";
                _eventAggregator.GetEvent<ErrorMessageEvent>().Publish($"{msg}");
                Log(msg, Category.Warn);

                GameFolder = null;
                CloseAllTabs();

                return Task.CompletedTask;
            }
            else
            {
                return this.OnReloadGame();
            }
        }

        private void AddRecentFolder(string gameFolder)
        {
            Log($"Add recent folder {gameFolder}");

            _appSettingsModel.AddRecentDirectory(gameFolder);
        }

        /// <summary>
        /// Determines whether [is valid game folder] by making sure folder exists and has a valid config.yaml
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is valid game folder]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidGameFolder()
        {            
            if (Directory.Exists(GameFolder))
            {
                if (File.Exists(Path.Combine(GameFolder, "config.yaml")))
                {
                    if (_skeletonGameProvider.GameConfig != null)
                    {
                        //IsMainTabEnabled = true;
                    }

                    return true;
                }
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
            //var vm = new CreateNewGameWindowViewModel();
            //var window = new CreateNewGameWindow();
            //window.DataContext = vm;

            var result = false;
            this.CreateNewGameRequest.Raise(new RequestNewGame { Title = "Request new game", Content = "Custom message" },
                r =>
                {
                    result = r.Success;

                });
        }

        private void OnExport(string exportParam)
        {
            try
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
            catch (Exception ex)
            {
                string msg = $"Error exporting {exportParam}. {ex.Message}";
                Log(msg, Category.Exception);
                _eventAggregator.GetEvent<ErrorMessageEvent>().Publish(msg);
            }
        }

        /// <summary>
        /// Called when [refresh skeleton game objects], loads all yaml files into the provider
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private async Task OnReloadGame()
        {
            Log("Loading game configs");
            try
            {
                _skeletonGameProvider.GameFolder = GameFolder;

                await _skeletonGameProvider.LoadYamlEntriesAsync();

                _eventAggregator.GetEvent<LoadYamlFilesChanged>().Publish(null);

                Log("Loading success");
                AddRecentFolder(GameFolder);

                //Disable machine tab if failed top parse
                //IsMachineConfigEnabled = _skeletonGameProvider.MachineConfig == null ? false : true;
                this.OnNavigate("GameConfigView");
            }
            catch (Exception ex)
            {
                var msg = $"Failed loading Game. {ex.Data["yaml"]} {ex.Message}";
                msg += $"/n/r {ex.Data["err"]}";
                Log(msg, Category.Exception);

                _eventAggregator.GetEvent<ErrorMessageEvent>().Publish($"{msg}");                
                _skeletonGameProvider.ClearConfigs();
                GameFolder = null;                
            }
            finally
            {
                
            }            
        }

        /// <summary>
        /// Updates the can execute commands.
        /// </summary>
        private void UpdateCanExecuteCommands()
        {
            BrowseFolderCommand.RaiseCanExecuteChanged();
            LaunchGameCommand.RaiseCanExecuteChanged();
            CreateNewGameCommand.RaiseCanExecuteChanged();
            ReloadGameCommand.RaiseCanExecuteChanged();
            SetDirectoryCommand.RaiseCanExecuteChanged();
            ExportCommand.RaiseCanExecuteChanged();
            LaunchRecordingCommand.RaiseCanExecuteChanged();
            NavigateCommand.RaiseCanExecuteChanged();
            LaunchToolCommand.RaiseCanExecuteChanged();
        }

        #endregion
    }

    public interface IRequestNewGame : IConfirmation
    {
        bool Success { get; set; }
    }

    public class RequestNewGame : Confirmation, IRequestNewGame
    {
        public bool Success { get; set; }
    }
}
