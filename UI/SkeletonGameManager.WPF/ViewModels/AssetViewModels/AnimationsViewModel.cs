using Prism.Commands;
using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGameManager.WPF.Providers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace SkeletonGameManager.WPF.ViewModels.AssetViewModels
{
    public class AnimationsViewModel : AssetFileBaseViewModel
    {
        private ISkeletonGameFiles _skeletonGameFiles;
        private ISkeletonGameProvider _skeletonGameProvider;
        private Uri _dmdPath;

        public AnimationsViewModel(ISkeletonGameFiles skeletonGameFiles, ISkeletonGameProvider skeletonGameProvider)
        {
            _skeletonGameFiles = skeletonGameFiles;
            _skeletonGameProvider = skeletonGameProvider;

            //Just assign this collection, don't create a new observable
            Animations =_skeletonGameProvider.AssetsConfig.Animations;
            Animations.CollectionChanged += Animations_CollectionChanged;

            OpenDirectoryCommand = new DelegateCommand(() => OpenDirectory(_dmdPath.AbsolutePath));
        }

        private void Animations_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)

        {
            //if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            //    _skeletonGameProvider.AssetsConfig.Animations.Add(new Animation());
            //else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            //    _skeletonGameProvider.AssetsConfig.Animations.RemoveAt(e.OldStartingIndex);
        }

        private ObservableCollection<Animation> animation;        
        public ObservableCollection<Animation> Animations
        {
            get { return animation; }
            set { SetProperty(ref animation, value); }
        }

        public async override Task GetFiles()
        {
            var animPath = _skeletonGameProvider.GameConfig.DmdPath;

            if (animPath.Contains("."))
                _dmdPath = new Uri(Path.Combine(_skeletonGameProvider.GameFolder, animPath), UriKind.RelativeOrAbsolute);
            else
                _dmdPath = new Uri(animPath);

            var animFiles = await _skeletonGameFiles.GetFilesAsync(_dmdPath.AbsolutePath, AssetTypes.Animations);

            this.AssetFiles = new ObservableCollection<string>(animFiles);
        }
    }
}
