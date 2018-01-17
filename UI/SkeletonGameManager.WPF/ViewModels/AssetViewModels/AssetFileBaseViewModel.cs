using GongSolutions.Wpf.DragDrop;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SkeletonGameManager.WPF.ViewModels.AssetViewModels
{
    public abstract class AssetFileBaseViewModel : BindableBase, IDropTarget
    {
        private ObservableCollection<string> assetFiles = new ObservableCollection<string>();
        public ObservableCollection<string> AssetFiles
        {
            get { return assetFiles; }
            set { SetProperty(ref assetFiles, value); }
        }

        public virtual void DragOver(IDropInfo dropInfo)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Drop(IDropInfo dropInfo)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task GetFiles()
        {
            throw new NotImplementedException();
        }
    }
}
