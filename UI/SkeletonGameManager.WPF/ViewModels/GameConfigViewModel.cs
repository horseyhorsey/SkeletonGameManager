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
using System;
using Microsoft.Practices.Unity;
using System.Text;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class GameConfigViewModel : SkeletonGameManagerViewModelBase
    {        
        private ISkeletonGameProvider _skeletonGameProvider;
        private IUnityContainer _unityContainer;
        private readonly KeyboardMappingsViewModel _keyboardMappingsVm;

        #region Constructors

        public GameConfigViewModel(IEventAggregator ea, ISkeletonGameProvider skeletonGameProvider, IUnityContainer unityContainer) : base(ea)
        {            
            _skeletonGameProvider = skeletonGameProvider;
            _unityContainer = unityContainer;
            _keyboardMappingsVm = _unityContainer.Resolve<KeyboardMappingsViewModel>();

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x =>await  OnLoadYamlFilesChanged());

            SaveCommand = new DelegateCommand(() =>
            {
                GameConfigModel.AudioBufferSize = (int)SelectedBufferSize;

                UpdateSwitchMaps();

                _skeletonGameProvider.SaveGameConfig(GameConfigModel);

            }, () => GameConfigModel == null ? false : true);
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

        /// <summary>
        /// Updates the switch maps before running save.
        /// </summary>
        private void UpdateSwitchMaps()
        {
            foreach (var item in _keyboardMappingsVm.SwitchKeys)
            {
                //Get the keycode....LShift etc are higher integer values
                var charCode = (int)item.Keycode;

                if (charCode > 10000) item.Key = charCode.ToString();
                else item.Key = Convert.ToString((char)item.Keycode);

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
