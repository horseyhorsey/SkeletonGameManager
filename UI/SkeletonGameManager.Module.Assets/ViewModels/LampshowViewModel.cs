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

            OpenDirectoryCommand = new DelegateCommand(() => OpenDirectory(_lampshowPath));

            ReverseLampshowCommand = new DelegateCommand<LampShow>((x) =>
            {
                OnReverseLampshow(x);
            });
        }
        #endregion

        #region Commands
        public ICommand ReverseLampshowCommand { get; set; } 
        #endregion

        #region Properties
        private ObservableCollection<LampShow> lampshows;
        public ObservableCollection<LampShow> LampShows
        {
            get { return lampshows; }
            set { SetProperty(ref lampshows, value); }
        }
        #endregion

        #region Public Methods

        public async override Task GetFiles()
        {
            //Lamps from yaml
            if (LampShows == null)
            {
                LampShows = _skeletonGameProvider.AssetsConfig?.LampShows;
                LampShows.CollectionChanged += LampShows_CollectionChanged;
            }            
           
            //Get lamp files
            _lampshowPath = Path.Combine(_skeletonGameProvider.GameFolder, "assets\\lampshows");
            if (!Directory.Exists(_lampshowPath)) Directory.CreateDirectory(_lampshowPath);
            var lampshowFiles = await _skeletonGameFiles.GetFilesAsync(_lampshowPath, AssetTypes.Lampshows);
            this.AssetFiles = new ObservableCollection<string>();
            foreach (var lampshow in lampshowFiles)
            {
                var lampFile = Path.GetFileName(lampshow);
                if (!LampShows.Any(x => x.File == lampFile))
                {
                    await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                    {
                        AssetFiles.Add(lampFile);
                    });
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
                        if (Path.GetExtension(lampFile) == ".lampshow")
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
                        if (Path.GetExtension(lampFile) == ".lampshow")
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
                        LampShows.AddRange(addedLampshows);
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
                _lampshowEdit.ReverseLampshowFile(Path.Combine(_lampshowPath, x.File),
                _lampshowPath + $"\\{Path.GetFileNameWithoutExtension(x.File)}_reversed.lampshow");
            }
        }

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
