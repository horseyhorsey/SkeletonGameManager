﻿using GongSolutions.Wpf.DragDrop;
using Prism.Mvvm;
using SkeletonGameManager.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkeletonGameManager.Module.Assets.ViewModels
{
    public abstract class AssetFileBaseViewModel : BindableBase, IDropTarget
    {
        public ICommand OpenDirectoryCommand { get; set; }

        public void OpenDirectory(string path)
        {
            if(!string.IsNullOrWhiteSpace(path))
                FileFolder.Explore(path);
        }

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