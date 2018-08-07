using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Regions;
using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGameManager.Base;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using static SkeletonGameManager.Base.Events;

namespace SkeletonGameManager.Module.Assets.ViewModels
{
    public class AssetListViewModel : SkeletonTabViewModel
    {
        #region Fields
        private IRegionManager _regionManager;
        private IUnityContainer _unityContainer;
        private ISkeletonGameProvider _skeletonGameProvider;
        private ISkeletonGameFiles _skeletonGameFiles;
        #endregion

        #region Constructors
        public AssetListViewModel(IRegionManager regionManager, IUnityContainer unityContainer, IEventAggregator eventAggregator, 
            ISkeletonGameProvider skeletonGameProvider, ISkeletonGameFiles skeletonGameFiles, ILoggerFacade loggerFacade) : 
            base(eventAggregator, loggerFacade)
        {
            Title = "Assets";

            _regionManager = regionManager;
            _unityContainer = unityContainer;
            _skeletonGameProvider = skeletonGameProvider;
            _skeletonGameFiles = skeletonGameFiles;

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x => await OnLoadYamlFilesChanged());

            SaveCommand = new DelegateCommand(() =>
            {
                _skeletonGameProvider.SaveAssetsFile(AssetsFile);
            }, () => AssetsFile == null ? false : true);

            SwitchViewCommand = new DelegateCommand<string>(OnSwitchView);
        } 
        #endregion

        #region Commands
        public ICommand SwitchViewCommand { get; set; } 
        #endregion

        #region Properties
        private AssetsFile assetsFile;
        /// <summary>
        /// Gets or sets the assets file which holds all values for an asset_list.yaml
        /// </summary>
        /// <value>
        /// The assets file.
        /// </value>
        public AssetsFile AssetsFile
        {
            get { return assetsFile; }
            set { SetProperty(ref assetsFile, value); }
        }

        #endregion

        #region Public Methods
        public override async Task OnLoadYamlFilesChanged()
        {
            Log("Loading assets");
            AssetsFile = _skeletonGameProvider.AssetsConfig;

            try
            {
                var fontsVm = _unityContainer.Resolve(typeof(FontsViewModel)) as FontsViewModel;
                await fontsVm?.GetFiles();

                var loadingVm = _unityContainer.Resolve(typeof(LoadingProgressViewModel)) as LoadingProgressViewModel;

                var animsVm = _unityContainer.Resolve(typeof(AnimationsViewModel)) as AnimationsViewModel;
                await animsVm?.GetFiles();

                var voiceVm = _unityContainer.Resolve(typeof(VoiceViewModel)) as VoiceViewModel;
                voiceVm.SetAudioType(AssetTypes.Voice);
                await voiceVm?.GetFiles();

                var musicVm = _unityContainer.Resolve<MusicViewModel>();
                musicVm.SetAudioType(AssetTypes.Music);
                await musicVm?.GetFiles();

                var sfxVm = _unityContainer.Resolve(typeof(SfxViewModel)) as SfxViewModel;
                sfxVm.SetAudioType(AssetTypes.Sfx);
                await sfxVm?.GetFiles();

                var lampVm = _unityContainer.Resolve(typeof(LampshowViewModel)) as LampshowViewModel;
                await lampVm?.GetFiles();

                SaveCommand.RaiseCanExecuteChanged();
            }
            catch (System.Exception ex)
            {
                Log(ex.Message, Category.Exception);
                //TODO: Log                
            }
        }
        #endregion

        #region Private Methods
        private void OnSwitchView(string viewName)
        {
            Type viewType = null;
            switch (viewName)
            {
                case "Fonts":
                    viewType = typeof(Views.FontsView);
                    break;
                case "Lampshows":
                    viewType = typeof(Views.LampshowView);
                    break;
                case "Sfx":
                    viewType = typeof(Views.SfxView);
                    break;
                case "Voice":
                    viewType = typeof(Views.VoiceView);
                    break;
                case "Music":
                    viewType = typeof(Views.MusicView);
                    break;
                case "Progress":
                    viewType = typeof(Views.LoadingProgressView);
                    break;
                case "Anims":
                    viewType = typeof(Views.AnimationsView);
                    break;                    
                default:
                    break;
            }

            _regionManager.Regions[Regions.CurrentAssetRegion].RemoveAll();
            _regionManager.RegisterViewWithRegion(Regions.CurrentAssetRegion, viewType);
        } 
        #endregion
    }
}
