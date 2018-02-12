using Prism.Events;
using SkeletonGameManager.WPF.Events;
using SkeletonGameManager.WPF.Providers;
using SkeletonGame.Models;
using Prism.Commands;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using SkeletonGameManager.WPF.ViewModels.Config;
using System.Linq;

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

        private ObservableCollection<KeyboardMapItemViewModel> switchKeys = new ObservableCollection<KeyboardMapItemViewModel>();
        public ObservableCollection<KeyboardMapItemViewModel> SwitchKeys
        {
            get { return switchKeys; }
            set { SetProperty(ref switchKeys, value); }
        }

        #region Private Methods

        public async override Task OnLoadYamlFilesChanged()
        {
            GameConfigModel = _skeletonGameProvider.GameConfig;
            SwitchKeys?.Clear();

            var orderedSwitch = GameConfigModel.KeyboardSwitchMap.OrderBy(x => x.Value);

            foreach (var keySwitch in orderedSwitch)
            {
                SwitchKeys.Add(new KeyboardMapItemViewModel(keySwitch));
            }

            await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
            {
                SaveCommand.RaiseCanExecuteChanged();
            });
        }

        #endregion        
    }
}
