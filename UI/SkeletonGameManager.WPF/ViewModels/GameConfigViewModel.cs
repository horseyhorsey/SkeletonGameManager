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
            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(x => OnLoadYamlFilesChanged());

            SaveCommand = new DelegateCommand(() =>
            {
                GameConfigModel.AudioBufferSize = (int)SelectedBufferSize;
                _skeletonGameProvider.SaveGameConfig(GameConfigModel);
            },() => GameConfigModel == null ? false : true);

            LaunchGameCommand = new DelegateCommand(() =>
            {
                var gameEntryPointFile = Path.Combine(_skeletonGameProvider.GameFolder, "game.py");

                if (!File.Exists(gameEntryPointFile))
                    return;

                var startInfo = new ProcessStartInfo("python");
                startInfo.WorkingDirectory = _skeletonGameProvider.GameFolder;
                startInfo.Arguments = $"{gameEntryPointFile}";

                Process.Start(startInfo);
            });
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
