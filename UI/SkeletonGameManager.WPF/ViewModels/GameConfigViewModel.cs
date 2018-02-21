using Prism.Events;
using SkeletonGameManager.WPF.Events;
using SkeletonGameManager.WPF.Providers;
using SkeletonGame.Models;
using Prism.Commands;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.Practices.Unity;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class GameConfigViewModel : SkeletonGameManagerViewModelBase
    {
        private ISkeletonGameProvider _skeletonGameProvider;
        private IUnityContainer _unityContainer;        

        #region Constructors

        public GameConfigViewModel(IEventAggregator ea, ISkeletonGameProvider skeletonGameProvider, IUnityContainer unityContainer) : base(ea)
        {            
            _skeletonGameProvider = skeletonGameProvider;
            _unityContainer = unityContainer;

            KeyboardMappingsVm = _unityContainer.Resolve<KeyboardMappingsViewModel>();

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x =>await  OnLoadYamlFilesChanged());

            SaveCommand = new DelegateCommand(() =>
            {
                GameConfigModel.AudioBufferSize = (int)SelectedBufferSize;

                UpdateSwitchMaps();

                _skeletonGameProvider.SaveGameConfig(GameConfigModel);

            }, () => GameConfigModel == null ? false : true);
        }

        #endregion

        #region Properties

        private GameConfig gameConfig;
        public GameConfig GameConfigModel
        {
            get { return gameConfig; }
            set { SetProperty(ref gameConfig, value); }
        }

        private KeyboardMappingsViewModel _keyboardMappingsVm;
        public KeyboardMappingsViewModel KeyboardMappingsVm
        {
            get { return _keyboardMappingsVm; }
            set { SetProperty(ref _keyboardMappingsVm, value); }
        }

        private BufferSize bufferSize = BufferSize.Buff512;
        public BufferSize SelectedBufferSize
        {
            get { return bufferSize; }
            set { SetProperty(ref bufferSize, value); }
        }
        #endregion

        #region Public Methods

        public async override Task OnLoadYamlFilesChanged()
        {
            GameConfigModel = _skeletonGameProvider.GameConfig;

            await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
            {
                SaveCommand.RaiseCanExecuteChanged();
            });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the switch maps before running save.
        /// </summary>
        private void UpdateSwitchMaps()
        {
            _skeletonGameProvider.GameConfig.KeyboardSwitchMap.Clear();

            //KeyboardMappingsVm.SwitchKeys.OrderBy(x => x.Key);

            foreach (var item in KeyboardMappingsVm.SwitchKeys)
            {
                //Get the keycode....LShift etc are higher integer values
                var charCode = (int)item.Keycode;

                if (charCode > 10000)
                    item.Key = charCode.ToString();    

                //Replace in dictionary.
                if (_skeletonGameProvider.GameConfig.KeyboardSwitchMap.ContainsKey(item.Key))
                    _skeletonGameProvider.GameConfig.KeyboardSwitchMap[item.Key] = item.Number;
                else
                    _skeletonGameProvider.GameConfig.KeyboardSwitchMap.Add(item.Key, item.Number);
            }
        }

        #endregion        
    }
}
