using Prism.Commands;
using Prism.Events;
using SkeletonGame.Models;
using SkeletonGameManager.WPF.Events;
using SkeletonGameManager.WPF.Providers;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class AssetListViewModel : SkeletonGameManagerViewModelBase
    {
        #region Fields

        private ISkeletonGameProvider _skeletonGameProvider; 

        #endregion

        public AssetListViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider) : base(eventAggregator)
        {
            _skeletonGameProvider = skeletonGameProvider;

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(x => OnLoadYamlFilesChanged());

            SaveCommand = new DelegateCommand(() =>
            {
                _skeletonGameProvider.SaveAssetsFile(AssetsFile);
            }, () => AssetsFile == null ? false : true);
        }

        private AssetsFile assetsFile;
        public AssetsFile AssetsFile
        {
            get { return assetsFile; }
            set { SetProperty(ref assetsFile, value); }
        }

        public override void OnLoadYamlFilesChanged()
        {
            AssetsFile = _skeletonGameProvider.AssetsConfig;

            SaveCommand.RaiseCanExecuteChanged();
        }
    }
}
