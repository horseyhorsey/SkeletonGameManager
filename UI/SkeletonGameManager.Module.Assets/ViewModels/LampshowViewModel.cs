using GongSolutions.Wpf.DragDrop;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using SkeletonGame.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using SkeletonGame.Engine;
using System.Windows.Threading;
using Prism.Commands;
using System.Windows.Input;
using System;
using SkeletonGameManager.Base;
using Prism.Events;
using Prism.Logging;
using System.ComponentModel;
using System.Windows.Data;

namespace SkeletonGameManager.Module.Assets.ViewModels
{
    public class LampshowViewModel : AssetFileBaseViewModel
    {
        #region Fields
        private string _lampshowPath;
        private ISkeletonGameFiles _skeletonGameFiles;
        private readonly ISkeletonGameProvider _skeletonGameProvider;
        private ILampshowEdit _lampshowEdit;
        #endregion

        #region Constructors
        public LampshowViewModel(ISkeletonGameFiles skeletonGameFiles, ISkeletonGameProvider skeletonGameProvider, IEventAggregator eventAggregator, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
            _skeletonGameFiles = skeletonGameFiles;
            _skeletonGameProvider = skeletonGameProvider;
            _lampshowEdit = new LampshowEdit();

            Title = "Lampshows";

            OpenDirectoryCommand = new DelegateCommand(() => OpenDirectory(_lampshowPath));
            ReverseLampshowCommand = new DelegateCommand<string>((x) =>
            {
                if (x != null)
                    OnReverseLampshow(new LampShow() { File = x });                
            });
        }

        private static bool Initiliazed = false;

        private void InitCollections()
        {
            if (_skeletonGameProvider.AssetsConfig?.LampShows == null)
                _skeletonGameProvider.AssetsConfig.LampShows = new ObservableCollection<LampShow>();

            if (_skeletonGameProvider.AssetsConfig?.RGBShows == null)
                _skeletonGameProvider.AssetsConfig.RGBShows = new ObservableCollection<LampShow>();

            LampShows = _skeletonGameProvider.AssetsConfig?.LampShows;
            RGBShows = _skeletonGameProvider.AssetsConfig?.RGBShows;

            if (LampShows != null)
            {
                LampShows.CollectionChanged -= LampShows_CollectionChanged;
                LampShows.CollectionChanged += LampShows_CollectionChanged;
                LampshowsCollectionView = new ListCollectionView(LampShows);
                LampshowsCollectionView.Filter = new Predicate<object>((x) => !GetFilteredView(x));
            }

            if (RGBShows != null)
            {
                RGBShows.CollectionChanged -= LampShows_CollectionChanged;
                RGBShows.CollectionChanged += LampShows_CollectionChanged;
                RgbshowsCollectionView = new ListCollectionView(RGBShows);
                RgbshowsCollectionView.Filter = new Predicate<object>(GetFilteredView);
            }

            Initiliazed = true;
        }
        #endregion

        #region Commands
        public ICommand ReverseLampshowCommand { get; set; }

        private ListCollectionView _lampshowsCollectionView;
        public ListCollectionView LampshowsCollectionView
        {
            get { return _lampshowsCollectionView; }
            set { SetProperty(ref _lampshowsCollectionView, value); }
        }

        private ListCollectionView _rgbshowsCollectionView;
        public ListCollectionView RgbshowsCollectionView
        {
            get { return _rgbshowsCollectionView; }
            set { SetProperty(ref _rgbshowsCollectionView, value); }
        }

        #endregion

        #region Properties
        public ObservableCollection<LampShow> LampShows { get; private set; }
        public ObservableCollection<LampShow> RGBShows { get; private set; }

        #endregion

        #region Public Methods

        private bool GetFilteredView(object obj)
        {
            var rgbShow = obj as LampShow;
            return rgbShow.File.Contains(".rgbshow");
        }

        public async override Task GetFiles()
        {
            InitCollections();

            //Get lamp files
            _lampshowPath = Path.Combine(_skeletonGameProvider.GameFolder, "assets\\lampshows");
            if (!Directory.Exists(_lampshowPath))
            {
                Log($"Creating lampshow directory. {_lampshowPath}");
                Directory.CreateDirectory(_lampshowPath);
            }

            Log($"Retrieving Lamp/RGB Shows from {_lampshowPath}");
            this.AssetFiles = new ObservableCollection<string>();
            var lampshowFiles = await _skeletonGameFiles.GetFilesAsync(_lampshowPath, AssetTypes.Lampshows);
            foreach (var lampshow in lampshowFiles)
            {
                var lampFile = Path.GetFileName(lampshow);
                if (lampFile.Contains(".lampshow"))
                {
                    if (!LampShows.Any(x => x.File == lampFile))
                    {
                        await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                        {
                            AssetFiles.Add(lampFile);
                        });
                    }
                }
                else if (lampFile.Contains(".rgbshow"))
                {
                    if (!RGBShows.Any(x => x.File == lampFile))
                    {
                        await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                        {
                            AssetFiles.Add(lampFile);
                        });
                    }
                }
            }
        }
        #endregion

        #region Drag Drop
        public override void Drop(IDropInfo dropInfo)
        {
            try
            {

                IList<LampShow> addedLampshows = new List<LampShow>();
                List<string> droppedFiles = new List<string>();

                //Needs a few checks here. We can be dragging in from explorer or across to the datagrid.
                var dragFileList = dropInfo.Data;

                //Dragged files from windows
                if (dragFileList.GetType() == typeof(DataObject))
                {
                    var windowsFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();

                    droppedFiles.AddRange(windowsFileList);
                }
                else
                {
                    //Catch if not a list of files, just add the one that's been dropped.
                    try
                    {
                        //Add files and remove them from the available list
                        droppedFiles.AddRange((IEnumerable<string>)dragFileList);
                    }
                    catch (System.Exception)
                    {
                        var file = (string)dragFileList;
                        droppedFiles.Add(file);
                    }
                }

                //Null draginfo here should mean files came from windows, so add them to file list
                if (dropInfo.DragInfo == null)
                {
                    foreach (var lampFile in droppedFiles)
                    {
                        if (Path.GetExtension(lampFile) == ".lampshow" || Path.GetExtension(lampFile) == ".rgbshow")
                        {
                            var lampFileName = Path.GetFileName(lampFile);

                            //Don't add dupes
                            if (!this.AssetFiles.Any(x => x == lampFile) && !this.LampShows.Any(x => x.File == lampFileName))
                            {
                                //Copy the file to the lampshow path and add to list
                                var newFilePath = Path.Combine(_lampshowPath, lampFileName);
                                if (!File.Exists(newFilePath))
                                    File.Copy(lampFile, newFilePath);

                                this.AssetFiles.Add(lampFileName);
                            }

                        }
                    }
                }
                //Convert the lampshow files to lampshow models and add to datagrid
                else
                {
                    // Return if trying to drag to the same element
                    if (dropInfo.DragInfo.VisualSource == dropInfo.VisualTarget) return;

                    //Remove all dragged files from list                    
                    foreach (var file in droppedFiles)
                    {
                        this.AssetFiles.Remove(file);
                    }

                    //Create lamp shows
                    foreach (var lampFile in droppedFiles)
                    {
                        if (Path.GetExtension(lampFile) == ".lampshow" || Path.GetExtension(lampFile) == ".rgbshow")
                        {
                            var file = Path.GetFileName(lampFile);
                            var key = Path.GetFileNameWithoutExtension(lampFile);

                            addedLampshows.Add(new LampShow()
                            {
                                Key = key,
                                File = Path.GetFileName(lampFile)
                            });
                        }
                    }

                    if (addedLampshows.Count > 0)
                    {                        
                        LampShows.AddRange(addedLampshows.Where(x => x.File.Contains(".lampshow")));
                        RGBShows.AddRange(addedLampshows.Where(x => x.File.Contains(".rgbshow")));
                    }
                }

            }
            catch (System.Exception)
            {
            }
        }

        public override void DragOver(IDropInfo dropInfo)
        {
            try
            {
                //Needs a few checks here. We can be dragging in from explorer or across to the datagrid.
                var dragFileList = dropInfo.Data;

                //Dragged from windows
                if (dragFileList.GetType() == typeof(DataObject))
                {
                    var windowsFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();

                    dropInfo.Effects = windowsFileList.Any(item =>
                    {
                        var extension = Path.GetExtension(item);
                        return extension != null;
                    }) ? DragDropEffects.Copy : DragDropEffects.None;
                }
                else
                {
                    dropInfo.Effects = DragDropEffects.Copy;
                }

            }
            catch (System.Exception)
            {
            }

        }
        #endregion

        #region Private Methods
        private void OnReverseLampshow(LampShow x)
        {
            //Select a lampshow before reversing
            if (x != null)
            {
                Log($"Reversing Lampshow. {x.Key}");

                var reverseFileName = _lampshowPath + $"\\{Path.GetFileNameWithoutExtension(x.File)}_reversed{Path.GetExtension(x.File)}";
                var result = _lampshowEdit.ReverseLampshowFile(Path.Combine(_lampshowPath, x.File), reverseFileName);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    if (!this.AssetFiles.Any(s => s == result))
                    {
                        this.AssetFiles.Add(result);
                        Log("Reverse Lampshow added");
                        return;
                    }
                    
                    Log("Lampshow reversed but not added, already exists.", Category.Warn);
                }
                else                    
                    Log($"Failed to reverse Lampshow. {x.Key}", Category.Warn);
            }
        }

        /// <summary>
        /// Adds the lampshow to the asset file list when item is removed from a collection
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void LampShows_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                var lampFile = e.OldItems[0] as LampShow;
                if (lampFile != null)
                    this.AssetFiles.Add(lampFile.File);
            }

        }
        #endregion
    }
}
