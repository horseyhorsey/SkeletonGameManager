using Prism.Events;
using SkeletonGame.Models;
using Prism.Commands;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.Practices.Unity;
using SkeletonGameManager.Base;
using static SkeletonGameManager.Base.Events;
using Prism.Logging;
using System;

namespace SkeletonGameManager.Module.Config.ViewModels
{
    public class GameConfigViewModel : SkeletonTabViewModel
    {
        private ISkeletonGameProvider _skeletonGameProvider;
        private IUnityContainer _unityContainer;        

        #region Constructors

        public GameConfigViewModel(IEventAggregator ea, ISkeletonGameProvider skeletonGameProvider, IUnityContainer unityContainer, ILoggerFacade loggerFacade) : base(ea, loggerFacade)
        {
            Title = "Game Config";
            FileName = "config.yaml";

            _skeletonGameProvider = skeletonGameProvider;
            _unityContainer = unityContainer;            

            KeyboardMappingsVm = _unityContainer.Resolve<KeyboardMappingsViewModel>();

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x =>await  OnLoadYamlFilesChanged());

            SaveCommand = new DelegateCommand(OnSaveInvoked, () => GameConfigModel == null ? false : true);

            Log("Iniitalized Game Config", Category.Debug);
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
            Log("Updating Game Config", Category.Debug);

            GameConfigModel = _skeletonGameProvider.GameConfig;
            if (GameConfigModel == null)
            {
                Log("Game Config unavailable", Category.Debug);
            }

            await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
            {
                SaveCommand.RaiseCanExecuteChanged();
            });
        }

        #endregion

        #region Private Methods

        private void OnSaveInvoked()
        {
            GameConfigModel.AudioBufferSize = (int)SelectedBufferSize;
            UpdateSwitchMaps();

            Log("Saving config.yaml");
            _skeletonGameProvider.SaveGameConfig(GameConfigModel);
        }


        /// <summary>
        /// Update the Keyboard switch maps before running save.
        /// </summary>
        private void UpdateSwitchMaps()
        {
            Log("Updating Keyboard Switch Maps");
            _skeletonGameProvider.GameConfig.KeyboardSwitchMap.Clear();
            //KeyboardMappingsVm.SwitchKeys.OrderBy(x => x.Key);

            foreach (var item in KeyboardMappingsVm.SwitchKeys)
            {
                //Get the keycode....LShift etc are higher integer values
                var charCode = (int)item.Keycode;

                if (charCode > 10000)
                    item.Key = charCode.ToString();    

                //Replace in dictionary
                //TODO: Give user option to save the Name or the Number to the config.yaml as it can handle reading from both in pyproc
                if (_skeletonGameProvider.GameConfig.KeyboardSwitchMap.ContainsKey(item.Key))
                    _skeletonGameProvider.GameConfig.KeyboardSwitchMap[item.Key] = item.Name;
                else
                    _skeletonGameProvider.GameConfig.KeyboardSwitchMap.Add(item.Key, item.Name);
            }
        }

        #endregion        
    }
}
