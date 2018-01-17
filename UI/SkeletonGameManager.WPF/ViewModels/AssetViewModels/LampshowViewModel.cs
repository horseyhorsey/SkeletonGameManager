using GongSolutions.Wpf.DragDrop;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using SkeletonGame.Models;
using System.Collections.Generic;

namespace SkeletonGameManager.WPF.ViewModels.AssetViewModels
{
    public class LampshowViewModel : AssetFileBaseViewModel
    {
        private readonly string _lampshowPath;

        public LampshowViewModel(List<LampShow> lampShows, string lampshowPath)
        {
            _lampshowPath = lampshowPath;
            LampShows = new ObservableCollection<LampShow>(lampShows);
            LampShows.CollectionChanged += LampShows_CollectionChanged;
        }

        private void LampShows_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                var lampFile = e.OldItems[0] as LampShow;

                if (lampFile !=null)
                    this.AssetFiles.Add(lampFile.File);
            }
            
        }

        private ObservableCollection<LampShow> lampshows;        

        public ObservableCollection<LampShow> LampShows
        {
            get { return lampshows; }
            set { SetProperty(ref lampshows, value); }
        }

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
                                File.Copy(lampFile, Path.Combine(_lampshowPath, lampFileName));
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
    }
}
