using GongSolutions.Wpf.DragDrop;
using Prism.Events;
using Prism.Logging;
using Prism.Regions;
using SkeletonGameManager.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkeletonGameManager.Module.Assets.ViewModels
{
    public abstract class AssetFileBaseViewModel : SkeletonTabViewModel, IDropTarget, INavigationAware
    {
        public ICommand OpenDirectoryCommand { get; set; }
        public ICommand OpenFileCommand { get; set; }

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
            try
            {
                if (!string.IsNullOrWhiteSpace(path))
                    FileFolder.Explore(path);
            }
            catch (Exception ex)
            {
                Log($"Error opening file/folder: {path}, {ex.Message}");
            }

        }
    }
}
