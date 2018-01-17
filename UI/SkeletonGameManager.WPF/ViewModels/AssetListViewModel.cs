using GongSolutions.Wpf.DragDrop;
using Prism.Commands;
using Prism.Events;
using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGameManager.WPF.Events;
using SkeletonGameManager.WPF.Providers;
using SkeletonGameManager.WPF.ViewModels.AssetViewModels;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class AssetListViewModel : SkeletonGameManagerViewModelBase
    {
        #region Fields

        private ISkeletonGameProvider _skeletonGameProvider;
        private ISkeletonGameFiles _skeletonGameFiles;

        #endregion

        public AssetListViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider, ISkeletonGameFiles skeletonGameFiles) : base(eventAggregator)
        {
            _skeletonGameProvider = skeletonGameProvider;
            _skeletonGameFiles = skeletonGameFiles;

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x => await OnLoadYamlFilesChanged());

            SaveCommand = new DelegateCommand(() =>
            {
                _skeletonGameProvider.SaveAssetsFile(AssetsFile);
            }, () => AssetsFile == null ? false : true);
        }

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

        private LampshowViewModel lampshowViewModel;
        public LampshowViewModel LampshowViewModel
        {
            get { return lampshowViewModel; }
            set { SetProperty(ref lampshowViewModel, value); }
        }

        private FontsViewModel fontsViewModel;
        public FontsViewModel FontsViewModel
        {
            get { return fontsViewModel; }
            set { SetProperty(ref fontsViewModel, value); }
        }

        private LoadingProgressViewModel loadingProgressViewModel;
        public LoadingProgressViewModel LoadingProgressViewModel
        {
            get { return loadingProgressViewModel; }
            set { SetProperty(ref loadingProgressViewModel, value); }
        }

        private SoundViewModel musicViewModel;
        public SoundViewModel MusicViewModel
        {
            get { return musicViewModel; }
            set { SetProperty(ref musicViewModel, value); }
        }

        private SoundViewModel voiceViewModel;
        public SoundViewModel VoiceViewModel
        {
            get { return voiceViewModel; }
            set { SetProperty(ref voiceViewModel, value); }
        }

        private SoundViewModel sfxViewModel;
        public SoundViewModel SfxViewModel
        {
            get { return sfxViewModel; }
            set { SetProperty(ref sfxViewModel, value); }
        }

        private AnimationsViewModel animationsViewModel;
        public AnimationsViewModel AnimationsViewModel
        {
            get { return animationsViewModel; }
            set { SetProperty(ref animationsViewModel, value); }
        }

        #endregion

        public override async Task OnLoadYamlFilesChanged()
        {
            AssetsFile = _skeletonGameProvider.AssetsConfig;
            
            try
            {
                var lampshowPath = Path.Combine(_skeletonGameProvider.GameFolder, "assets\\lampshows");
                LampshowViewModel = new LampshowViewModel(AssetsFile.LampShows, lampshowPath);

                //Only path in skele game that isn't editable
                var lampshows = await _skeletonGameFiles.GetFilesAsync(lampshowPath, AssetTypes.Lampshows);

                LampshowViewModel.AssetFiles = new System.Collections.ObjectModel.ObservableCollection<string>();
                foreach (var lampshow in lampshows)
                {
                    var lampFile = Path.GetFileName(lampshow);
                    if (!AssetsFile.LampShows.Any(x => x.File == lampFile))
                    {
                        await Dispatcher.CurrentDispatcher.InvokeAsync(() => {
                            LampshowViewModel.AssetFiles.Add(lampFile);
                        });
                    }
                }

                //Fonts Vm
                FontsViewModel = new FontsViewModel(_skeletonGameFiles, _skeletonGameProvider);
                await FontsViewModel.GetFiles();

                //Loading
                LoadingProgressViewModel = new LoadingProgressViewModel(_skeletonGameProvider.AssetsConfig.UserInterface);

                MusicViewModel = new SoundViewModel(_skeletonGameFiles, _skeletonGameProvider, AssetTypes.Music);
                await MusicViewModel.GetFiles();

                VoiceViewModel = new SoundViewModel(_skeletonGameFiles, _skeletonGameProvider, AssetTypes.Voice);
                await VoiceViewModel.GetFiles();

                SfxViewModel = new SoundViewModel(_skeletonGameFiles, _skeletonGameProvider, AssetTypes.Sfx);
                await SfxViewModel.GetFiles();

                AnimationsViewModel = new AnimationsViewModel(_skeletonGameFiles, _skeletonGameProvider);
                await AnimationsViewModel.GetFiles();

                SaveCommand.RaiseCanExecuteChanged();
            }
            catch (System.Exception)
            {

                
            }
            
        }
    }
}
