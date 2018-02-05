using System;
using Prism.Events;
using Prism.Mvvm;
using SkeletonGameManager.WPF.Events;
using SkeletonGameManager.WPF.Providers;
using SkeletonGame.Models;
using System.Windows.Input;
using Prism.Commands;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class GameConfigViewModel : SkeletonGameManagerViewModelBase
    {        
        private ISkeletonGameProvider _skeletonGameProvider;

        #region Command
        public ICommand LaunchGameCommand { get; set; }
        #endregion

        #region Constructors

        public GameConfigViewModel(IEventAggregator ea, ISkeletonGameProvider skeletonGameProvider) : base(ea)
        {            
            _skeletonGameProvider = skeletonGameProvider;
            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x =>await  OnLoadYamlFilesChanged());

            SaveCommand = new DelegateCommand(() =>
            {
                GameConfigModel.AudioBufferSize = (int)SelectedBufferSize;
                _skeletonGameProvider.SaveGameConfig(GameConfigModel);
            },() => GameConfigModel == null ? false : true);

            LaunchGameCommand = new DelegateCommand(() =>
            {
                OnLaunchedGame();
            });
        }

        /// <summary>
        /// Launches the current game. Checks if python  can run from Enviroment. If not, points to download skeletongame.com
        /// </summary>
        private void OnLaunchedGame()
        {
            var gameEntryPointFile = Path.Combine(_skeletonGameProvider.GameFolder, "game.py");

            if (!File.Exists(gameEntryPointFile))
                return;

            try
            {
                //Check python 27 installed first
                string getUserEnv = Environment.GetEnvironmentVariable("path",EnvironmentVariableTarget.User);
                if (!getUserEnv.Contains(@"C:\Python27"))
                    throw new FileNotFoundException(@"C:\Python27 python not found in your enviroment");

                //Build args to run the game.py with python
                var startInfo = new ProcessStartInfo("python");
                startInfo.WorkingDirectory = _skeletonGameProvider.GameFolder;
                startInfo.Arguments = $"{gameEntryPointFile}";
                Process.Start(startInfo);
            }
            catch (FileNotFoundException ex)
            {
                var result = MessageBox.Show($"{ex.Message}. Download skeleton game installer with python?", "", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                    Process.Start(@"http://skeletongame.com/step-1-installation-and-testing-the-install-windows/");
            }
            catch(Exception ex)
            {

            }
        }

        #endregion

        private GameConfig gameConfig;
        public GameConfig GameConfigModel
        {
            get { return gameConfig; }
            set { SetProperty(ref gameConfig, value); }
        }

        private BufferSize bufferSize = BufferSize.Buff512;
        public BufferSize SelectedBufferSize
        {
            get { return bufferSize; }
            set { SetProperty(ref bufferSize, value); }
        }

        #region Private Methods

        public async override Task OnLoadYamlFilesChanged()
        {
            GameConfigModel = _skeletonGameProvider.GameConfig;

            await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
            {
                SaveCommand.RaiseCanExecuteChanged();
            });
        }

        #endregion        
    }
}
