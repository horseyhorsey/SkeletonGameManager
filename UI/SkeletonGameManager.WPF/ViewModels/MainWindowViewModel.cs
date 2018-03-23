using Prism.Commands;
using Prism.Mvvm;
using SkeletonGameManager.WPF.Providers;
using System.Windows.Forms;
using System.Windows.Input;
using System;
using System.IO;
using System.Threading.Tasks;
using Prism.Events;
using SkeletonGameManager.WPF.Events;
using SkeletonGameManager.WPF.Views;
using System.Diagnostics;
using System.Windows;
using SkeletonGame.Engine;
using System.Collections.Generic;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class MainWindowViewModel : SkeletonGameManagerViewModelBase
    {
        #region Fields
        private ISkeletonGameProvider _skeletonGameProvider;
        private ISkeletonLogger _skeletonLogger;
        #endregion

        #region Commands
        public DelegateCommand SetDirectoryCommand { get; set; }
        public DelegateCommand RefreshObjectsCommand { get; set; }
        public DelegateCommand CreateNewGameCommand { get; set; }
        public DelegateCommand<string> OpenFileFolderCommand { get; set; }
        public DelegateCommand LaunchGameCommand { get; set; }
        public DelegateCommand OpenGameFolderCommand { get; set; }
        #endregion

        #region Constructors

        public MainWindowViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider, ISkeletonLogger skeletonLogger) : base(eventAggregator)
        {
            _skeletonGameProvider = skeletonGameProvider;
            _skeletonLogger = skeletonLogger;
            IsGameRunning = false;

            SetDirectoryCommand = new DelegateCommand(() => OnSetDirectory(), () => !IsGameRunning);

            RefreshObjectsCommand = new DelegateCommand(async () => await OnRefreshSkeletonGameObjects(), () => IsValidGameFolder());

            CreateNewGameCommand = new DelegateCommand(CreateNewGame);

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

            _eventAggregator.GetEvent<OnLaunchGameEvent>().Subscribe(async (x) =>
            {
                IsGameRunning = true;
                await OnLaunchedGame();
            });

            //Open the game folder
            OpenGameFolderCommand = new DelegateCommand(() => Process.Start(_skeletonGameProvider.GameFolder), () => IsValidGameFolder());
        }

        private void CreateNewGame()
        {
            var vm = new CreateNewGameWindowViewModel();
            var window = new CreateNewGameWindow();
            window.DataContext = vm;

            var dialog = window.ShowDialog();

        }

        #endregion

        #region Properties

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
                this.OpenGameFolderCommand.RaiseCanExecuteChanged();
            }
        }


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

        #region Private Methods        

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
                        IsMainTabEnabled = true;

                    return true;
                }

            IsMainTabEnabled = false;
            return false;
        }

        /// <summary>
        /// Launches the current game. Checks if python  can run from Enviroment. If not, points to download skeletongame.com
        /// </summary>
        private async Task OnLaunchedGame()
        {
            if (_skeletonGameProvider.GameFolder == null) return;

            var gameEntryPointFile = Path.Combine(_skeletonGameProvider.GameFolder, "game.py");

            if (!File.Exists(gameEntryPointFile))
                return;

            try
            {
                LaunchGameCommand.RaiseCanExecuteChanged();
                CreateNewGameCommand.RaiseCanExecuteChanged();
                RefreshObjectsCommand.RaiseCanExecuteChanged();
                OpenFileFolderCommand.RaiseCanExecuteChanged();
                SetDirectoryCommand.RaiseCanExecuteChanged();

                //Clear the previous log.
                _skeletonLogger.LogData.Clear();

                //Check python 27 installed first
                string getUserEnv = Environment.GetEnvironmentVariable("path", EnvironmentVariableTarget.User);
                if (!getUserEnv.Contains(@"C:\Python27"))
                    throw new FileNotFoundException(@"C:\Python27 python not found in your enviroment");

                //Build args to run the game.py with python
                var startInfo = new ProcessStartInfo("python");
                startInfo.WorkingDirectory = _skeletonGameProvider.GameFolder;
                startInfo.Arguments = $"{gameEntryPointFile}";

                //Redirect just the error output...
                startInfo.CreateNoWindow = true;
                startInfo.RedirectStandardError = true;
                startInfo.RedirectStandardInput = true;
                startInfo.UseShellExecute = false;

                await Task.Run(() =>
                 {
                     var p = new Process { StartInfo = startInfo };

                     p.Start();
                     //p.BeginOutputReadLine();
                     p.BeginErrorReadLine();

                     p.ErrorDataReceived += P_OutputDataReceived1;
                     p.Exited += P_Exited;
                     p.Disposed += P_Disposed;
                     p.WaitForExit();
                 });

            }
            catch (FileNotFoundException ex)
            {
                var result = System.Windows.MessageBox.Show($"{ex.Message}. Download skeleton game installer with python?", "", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                    Process.Start(@"http://skeletongame.com/step-1-installation-and-testing-the-install-windows/");
            }
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

                if (_skeletonLogger?.LogData?.Count > 0)
                {
                    _lastgameLog = _skeletonLogger.LogData;
                    _skeletonLogger.LogToFile(Path.Combine(GameFolder, "Logs"));
                }

                _eventAggregator.GetEvent<OnGameEndedEvent>().Publish(true);
            }
        }

        private void P_Disposed(object sender, EventArgs e)
        {

        }

        private void P_Exited(object sender, EventArgs e)
        {
            var logs = _skeletonLogger.LogData;
        }

        private void P_OutputDataReceived1(object sender, DataReceivedEventArgs e)
        {
            _skeletonLogger.LogData.Add(e.Data);
        }

        private void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {

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

                IsMainTabEnabled = true;

                //Disable machine tab if failed top parse
                IsMachineConfigEnabled = _skeletonGameProvider.MachineConfig == null ? false : true;
            }
            catch (Exception ex)
            {
                IsMainTabEnabled = false;

                System.Windows.MessageBox.Show($"Failed loading skeleton game files. {ex.Data["yaml"]} {ex.Message}");
            }
        }

        /// <summary>
        /// Called when /[set directory], sets the current skeleton game folder path
        /// </summary>
        private void OnSetDirectory()
        {
            var dlg = new FolderBrowserDialog();
            dlg.SelectedPath = @"C:\P-ROC";
            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                _skeletonGameProvider.ClearConfigs();

                IsMainTabEnabled = false;

                GameFolder = dlg.SelectedPath;

                //_eventAggregator.GetEvent<LoadYamlFilesChanged>().Publish(null);
            };
        }
        #endregion
    }
}

