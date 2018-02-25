using Prism.Commands;
using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGameManager.WPF.Providers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using GongSolutions.Wpf.DragDrop;
using System.Windows;
using System.Linq;

namespace SkeletonGameManager.WPF.ViewModels.AssetViewModels
{
    public class SoundViewModel : AssetFileBaseViewModel
    {
        #region Fields
        private ISkeletonGameFiles _skeletonGameFiles;
        private ISkeletonGameProvider _skeletonGameProvider;
        private AssetTypes _audioType;

        private Uri _audioPathFull;

        private readonly string[] Extenstions = { ".mp3", ".ogg", ".wav"};
        #endregion

        #region Commands
        public ICommand OpenSoundCommand { get; set; }
        
        #endregion

        public SoundViewModel(ISkeletonGameFiles skeletonGameFiles, ISkeletonGameProvider skeletonGameProvider, AssetTypes audioType)
        {
            _skeletonGameFiles = skeletonGameFiles;
            _skeletonGameProvider = skeletonGameProvider;
            _audioType = audioType;

            OpenSoundCommand = new DelegateCommand<string>((s) =>
            {
                Process.Start(s);
            });

            OpenDirectoryCommand = new DelegateCommand(() => OpenDirectory(_audioPathFull.AbsolutePath));

            InitCollectionByAudiotType(audioType);
        }

        #region Properties
        private ObservableCollection<Music> music;
        public ObservableCollection<Music> AudioEntries
        {
            get { return music; }
            set { SetProperty(ref music, value); }
        }
        #endregion

        #region Public Methods

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
            IList<Music> addedSounds = new List<Music>();
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
                    if (FileIsAudio(file))
                    {
                        var animFileName = Path.GetFileName(file);

                        //Don't add dupes
                        if (!this.AssetFiles.Any(x => x == file) && !this.AudioEntries.Any(x => x.File == animFileName))
                        {
                            //Copy the file to the lampshow path and add to list
                            var newFilePath = Path.Combine(_audioPathFull.AbsolutePath, animFileName);
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
                
                foreach (var audioFile in droppedFiles)
                {
                    if (FileIsAudio(audioFile))
                    {
                        var file = Path.GetFileName(audioFile);
                        var key = Path.GetFileNameWithoutExtension(audioFile);

                        if (!this.AudioEntries.Any(x => x.File == file))
                        {
                            addedSounds.Add(new Music()
                            {
                                Key = key,
                                File = Path.GetFileName(audioFile)
                            });
                        }                        
                    }
                }

                if (addedSounds.Count > 0)
                {
                    this.AudioEntries.AddRange(addedSounds);
                }
            }
        }

        /// <summary>
        /// Checks the file extension to see if video/image (animation)
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        private bool FileIsAudio(string file)
        {
            var extension = Path.GetExtension(file);

            switch (extension)
            {
                case ".wav":
                case ".mp3":
                case ".ogg":
                case ".aiff":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets the audio files based on <see cref="AssetTypes"/> this view model was initialized with
        /// </summary>
        /// <returns></returns>
        public async override Task GetFiles()
        {
            var audioDir = string.Empty;

            switch (_audioType)
            {
                case AssetTypes.Music:
                    audioDir = Path.Combine(_skeletonGameProvider.GameConfig.SoundPath, _skeletonGameProvider.GameConfig.MusicDir);
                    break;
                case AssetTypes.Voice:
                    audioDir = Path.Combine(_skeletonGameProvider.GameConfig.SoundPath, _skeletonGameProvider.GameConfig.VoiceDir);
                    break;
                case AssetTypes.Sfx:
                    audioDir = Path.Combine(_skeletonGameProvider.GameConfig.SoundPath, _skeletonGameProvider.GameConfig.SfxDir);
                    break;
                default:
                    break;
            }

            if (audioDir.Contains("."))
                _audioPathFull = new Uri(Path.Combine(_skeletonGameProvider.GameFolder, audioDir), UriKind.RelativeOrAbsolute);
            else
                _audioPathFull = new Uri(audioDir);

            if (!Directory.Exists(_audioPathFull.AbsolutePath)) Directory.CreateDirectory(_audioPathFull.AbsolutePath);

            var audioFiles = await _skeletonGameFiles.GetFilesAsync(_audioPathFull.AbsolutePath, _audioType);
            this.AssetFiles = new System.Collections.ObjectModel.ObservableCollection<string>(audioFiles);
        } 
        #endregion

        #region Private Methods

        private void InitCollectionByAudiotType(AssetTypes audioType)
        {
            switch (audioType)
            {
                case AssetTypes.Music:
                    AudioEntries = _skeletonGameProvider.AssetsConfig.Audio.Music;
                    _skeletonGameProvider.AssetsConfig.Audio.AllAudio.AddRange(_skeletonGameProvider.AssetsConfig.Audio.Music);
                    break;
                case AssetTypes.Voice:
                    AudioEntries = _skeletonGameProvider.AssetsConfig.Audio.Voice;
                    _skeletonGameProvider.AssetsConfig.Audio.AllAudio.AddRange(_skeletonGameProvider.AssetsConfig.Audio.Voice);
                    break;
                case AssetTypes.Sfx:
                    AudioEntries = _skeletonGameProvider.AssetsConfig.Audio.Effects;
                    _skeletonGameProvider.AssetsConfig.Audio.AllAudio.AddRange(_skeletonGameProvider.AssetsConfig.Audio.Effects);
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
