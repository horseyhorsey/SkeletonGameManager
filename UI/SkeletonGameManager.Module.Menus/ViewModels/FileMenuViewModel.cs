using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SkeletonGameManager.Base;
using SkeletonGameManager.Module.Menus.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using static SkeletonGameManager.Base.Events;

namespace SkeletonGameManager.Module.Menus.ViewModels
{
    public class FileMenuViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        private ISkeletonGameProvider _skeletonGameProvider;
        private IGameRunnner _gameRunnner;
        #region Commands
        public DelegateCommand CreateNewGameCommand { get; }
        public DelegateCommand OpenGameFolderCommand { get; }
        public DelegateCommand SetDirectoryCommand { get;  }
        public DelegateCommand RefreshObjectsCommand { get; }
        public DelegateCommand<string> OpenRecentCommand { get; }
        public DelegateCommand<string> OpenFileFolderCommand { get;}
        public DelegateCommand LaunchGameCommand { get;}
        public DelegateCommand<string> ExportCommand { get; }        
        #endregion

        public FileMenuViewModel(ISkeletonGameProvider skeletonGameProvider, IGameRunnner gameRunnner, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _skeletonGameProvider = skeletonGameProvider;
            _gameRunnner = gameRunnner;

            CreateNewGameCommand = new DelegateCommand(OnCreateNewGame);
            OpenGameFolderCommand = new DelegateCommand(() => Process.Start(skeletonGameProvider.GameFolder), () => IsValidGameFolder());

            LaunchGameCommand = new DelegateCommand(async () =>
            {
                IsGameRunning = true;
                await OnLaunchedGame();

            }, () => IsValidGameFolder());

            SetDirectoryCommand = new DelegateCommand(() => OnSetDirectory(), () => !IsGameRunning);

            RefreshObjectsCommand = new DelegateCommand(async () => await OnRefreshSkeletonGameObjects(), () => IsValidGameFolder());

            OpenRecentCommand = new DelegateCommand<string>(OnOpenRecent);

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
                        Process.Start(Path.Combine(_skeletonGameProvider.GameFolder, "config", x));
                        break;
                    case "config.yaml":
                        Process.Start(Path.Combine(_skeletonGameProvider.GameFolder, x));
                        break;
                    default:
                        break;
                }
            });

            LaunchGameCommand = new DelegateCommand(async () =>
            {
                IsGameRunning = true;
                await OnLaunchedGame();

            }, () => IsValidGameFolder());

            ExportCommand = new DelegateCommand<string>(OnExport, (x) => IsValidGameFolder());

            _eventAggregator.GetEvent<OnLaunchGameEvent>().Subscribe(async (x) =>
            {
                IsGameRunning = true;
                await OnLaunchedGame();
            });
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
                default:
                    break;
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
                RefreshObjectsCommand.RaiseCanExecuteChanged();
                LaunchGameCommand.RaiseCanExecuteChanged();
                SetDirectoryCommand.RaiseCanExecuteChanged();
                ExportCommand.RaiseCanExecuteChanged();
                this.OpenGameFolderCommand.RaiseCanExecuteChanged();
            }
        }

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

            LaunchGameCommand.RaiseCanExecuteChanged();
            CreateNewGameCommand.RaiseCanExecuteChanged();
            RefreshObjectsCommand.RaiseCanExecuteChanged();
            OpenFileFolderCommand.RaiseCanExecuteChanged();
            SetDirectoryCommand.RaiseCanExecuteChanged();
            ExportCommand.RaiseCanExecuteChanged();

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

                LaunchGameCommand.RaiseCanExecuteChanged();
                CreateNewGameCommand.RaiseCanExecuteChanged();
                RefreshObjectsCommand.RaiseCanExecuteChanged();
                OpenFileFolderCommand.RaiseCanExecuteChanged();
                SetDirectoryCommand.RaiseCanExecuteChanged();
                ExportCommand.RaiseCanExecuteChanged();

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
            await SetGamePath(obj);
        }

        /// <summary>
        /// Sets the game path folder. Clears configs.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private async Task SetGamePath(string path)
        {
            _skeletonGameProvider.ClearConfigs();            
            GameFolder = path;

            if (!IsValidGameFolder())
            {
                System.Windows.MessageBox.Show($"Not a valid Game folder -  {GameFolder}");
                GameFolder = null;
            }
            else
                await this.OnRefreshSkeletonGameObjects();
        }

        /// <summary>
        /// Called when [refresh skeleton game objects], loads all yaml files into the provider
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private async Task OnRefreshSkeletonGameObjects()
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

        #endregion
    }
}
