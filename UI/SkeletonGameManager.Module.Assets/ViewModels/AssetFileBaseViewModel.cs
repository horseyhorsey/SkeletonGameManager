using GongSolutions.Wpf.DragDrop;
using Prism.Events;
using Prism.Logging;
using SkeletonGameManager.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkeletonGameManager.Module.Assets.ViewModels
{
    public abstract class AssetFileBaseViewModel : SkeletonGameManagerViewModelBase, IDropTarget
    {
        public ICommand OpenDirectoryCommand { get; set; }        

        public AssetFileBaseViewModel(IEventAggregator eventAggregator, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
        }

        #region Properties
        private ObservableCollection<string> assetFiles = new ObservableCollection<string>();
        public ObservableCollection<string> AssetFiles
        {
            get { return assetFiles; }
            set { SetProperty(ref assetFiles, value); }
        } 
        #endregion

        #region Drag/Drop
        public virtual void DragOver(IDropInfo dropInfo)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Drop(IDropInfo dropInfo)
        {
            throw new System.NotImplementedException();
        } 
        #endregion

        public virtual Task GetFiles()
        {
            throw new NotImplementedException();
        }

        public void OpenDirectory(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
                FileFolder.Explore(path);
        }
    }
}
