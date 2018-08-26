using GongSolutions.Wpf.DragDrop;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGameManager.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SkeletonGameManager.Module.Assets.ViewModels
{
    public class AnimationsViewModel : AssetFileBaseViewModel
    {
        #region Fields
        private ISkeletonGameFiles _skeletonGameFiles;
        private ISkeletonGameProvider _skeletonGameProvider;
        private Uri _dmdPath;
        #endregion

        #region Constructors
        public AnimationsViewModel(ISkeletonGameFiles skeletonGameFiles, ISkeletonGameProvider skeletonGameProvider, IEventAggregator eventAggregator, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
            _skeletonGameFiles = skeletonGameFiles;
            _skeletonGameProvider = skeletonGameProvider;

            Title = "Animations";

            OpenDirectoryCommand = new DelegateCommand(() => OpenDirectory(_dmdPath.AbsolutePath));
            OpenFileCommand = new DelegateCommand<string>(x => OpenDirectory(Path.Combine(_dmdPath.AbsolutePath, x)));
        } 
        #endregion

        #region Properties
        private ObservableCollection<Animation> animation;
        public ObservableCollection<Animation> Animations
        {
            get { return animation; }
            set { SetProperty(ref animation, value); }
        }
        #endregion

        #region Public Methods
        public async override Task GetFiles()
        {
            Log("Populating Animation files");

            Animations = _skeletonGameProvider.AssetsConfig.Animations;

            //Get the dmd path and create relative URI if includes .
            var animPath = _skeletonGameProvider.GameConfig.DmdPath;
            if (animPath.Contains("."))
                _dmdPath = new Uri(Path.Combine(_skeletonGameProvider.GameFolder, animPath), UriKind.RelativeOrAbsolute);
            else
                _dmdPath = new Uri(animPath);

            // Get the files and create collection.
            var animFiles = await _skeletonGameFiles.GetFilesAsync(_dmdPath.AbsolutePath, AssetTypes.Animations);
            this.AssetFiles = new ObservableCollection<string>(animFiles);
        } 
        #endregion

        #region Private Methods
        /// <summary>
        /// Checks the file extension to see if video/image (animation)
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        private bool FileIsAnimation(string file)
        {
            var extension = Path.GetExtension(file);

            switch (extension)
            {
                case ".mp4":
                case ".avi":
                case ".png":
                case ".jpg":
                case ".gif":
                case ".bmp":
                    return true;
                default:
                    return false;
            }
        }
        #endregion

        #region Drag Drop
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

        public override void Drop(IDropInfo dropInfo)
        {
            try
            {

                IList<Animation> addedAnimations = new List<Animation>();
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
                    foreach (var file in droppedFiles)
                    {
                        if (FileIsAnimation(file))
                        {
                            var animFileName = Path.GetFileName(file);

                            //Don't add dupes
                            if (!this.AssetFiles.Any(x => x == file) && !this.Animations.Any(x => x.File == animFileName))
                            {
                                //Copy the file to the lampshow path and add to list
                                var newFilePath = Path.Combine(_dmdPath.AbsolutePath, animFileName);
                                if (!File.Exists(newFilePath))
                                    File.Copy(file, newFilePath);

                                this.AssetFiles.Add(animFileName);
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
                    foreach (var animationFile in droppedFiles)
                    {
                        if (FileIsAnimation(animationFile))
                        {
                            var file = Path.GetFileName(animationFile);
                            var key = Path.GetFileNameWithoutExtension(animationFile);

                            addedAnimations.Add(new Animation()
                            {
                                Key = key,
                                File = Path.GetFileName(animationFile)
                            });
                        }
                    }

                    if (addedAnimations.Count > 0)
                    {
                        Animations.AddRange(addedAnimations);
                    }
                }

            }
            catch (System.Exception)
            {
            }
        }
        #endregion
    }
}
