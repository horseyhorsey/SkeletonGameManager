using Prism.Events;
using SkeletonGameManager.WPF.Events;
using SkeletonGameManager.WPF.Providers;
using SkeletonGame.Models;
using Prism.Commands;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class GameConfigViewModel : SkeletonGameManagerViewModelBase
    {        
        private ISkeletonGameProvider _skeletonGameProvider;        

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
