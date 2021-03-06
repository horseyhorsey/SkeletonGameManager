﻿using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Regions;
using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGameManager.Base;
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

            #region Commands
            CloseTabCommand = new DelegateCommand<object>(OnCloseTab);
            SaveCommand = new DelegateCommand(() => { OnSaveAssetLists(); }, () => AssetsFile == null ? false : true);
            SwitchViewCommand = new DelegateCommand<string>(OnSwitchView);
            #endregion

            //Set the main assets view
            _regionManager.RequestNavigate(Regions.AssetDetailRegion, "AssetDetailsView");
        }

        #endregion

        #region Commands
        public ICommand CloseTabCommand { get; }
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
                loadingVm.UserInterface = this.AssetsFile.UserInterface;

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
                _eventAggregator.GetEvent<ErrorMessageEvent>().Publish($"Failed getting assets. {ex.Message}");
            }
        }
        #endregion

        #region Private Methods
    
        /// <summary>
        /// Called when [close tab] to close a view /tab from the OpenTabsRegion.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void OnCloseTab(object obj)
        {
            IRegion region = _regionManager.Regions["CurrentAssetRegion"];
            if (region.Views.Contains(obj))
            {
                region.Remove(obj);
                Log("Removed assets tab: " + obj);
            }
        }

        private void OnSaveAssetLists()
        {
            Log("Saving asset_list.yaml");

            try
            { _skeletonGameProvider.SaveAssetsFile(AssetsFile); }
            catch (System.Exception ex)
            {
                Log($"Failed saving asset list. {ex.Message}", Category.Exception);
            }
        }

        private void OnSwitchView(string viewName)
        {            
            switch (viewName)
            {
                case "Fonts":
                    viewName = "FontsView";
                    break;
                case "Lampshows":
                    viewName = "LampshowView";
                    break;
                case "Sfx":
                    viewName = "SfxView";
                    break;
                case "Voice":
                    viewName = "VoiceView";
                    break;
                case "Music":
                    viewName = "MusicView";
                    break;
                case "Progress":
                    viewName = "LoadingProgressView";
                    break;
                case "Anims":
                    viewName = "AnimationsView";
                    break;                    
                default:
                    break;
            }

            Log($"Switching View. {viewName}");
            _regionManager.RequestNavigate(Regions.CurrentAssetRegion, viewName);
        }
        #endregion

    }
}
