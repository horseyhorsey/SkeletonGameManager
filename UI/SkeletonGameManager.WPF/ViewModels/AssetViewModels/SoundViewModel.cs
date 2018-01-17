using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGameManager.WPF.Providers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace SkeletonGameManager.WPF.ViewModels.AssetViewModels
{
    public class SoundViewModel : AssetFileBaseViewModel
    {
        #region Fields
        private ISkeletonGameFiles _skeletonGameFiles;
        private ISkeletonGameProvider _skeletonGameProvider;
        private AssetTypes _audioType;

        private Uri _audioPathFull;
        #endregion

        public SoundViewModel(ISkeletonGameFiles skeletonGameFiles, ISkeletonGameProvider skeletonGameProvider, AssetTypes audioType)
        {
            _skeletonGameFiles = skeletonGameFiles;
            _skeletonGameProvider = skeletonGameProvider;
            _audioType = audioType;


            InitCollectionByAudiotType(audioType);
        }

        private ObservableCollection<Music> music;
        public ObservableCollection<Music> AudioEntries
        {
            get { return music; }
            set { SetProperty(ref music, value); }
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
                _audioPathFull = new Uri(Path.Combine(_skeletonGameProvider.GameFolder,  audioDir), UriKind.RelativeOrAbsolute);
            else
                _audioPathFull = new Uri(audioDir);

            var audioFiles = await _skeletonGameFiles.GetFilesAsync(_audioPathFull.AbsolutePath, _audioType);
            this.AssetFiles = new System.Collections.ObjectModel.ObservableCollection<string>(audioFiles);
        }

        #region Private Methods

        private void InitCollectionByAudiotType(AssetTypes audioType)
        {
            switch (audioType)
            {
                case AssetTypes.Music:
                    AudioEntries = new ObservableCollection<Music>(_skeletonGameProvider.AssetsConfig.Audio.Music);
                    break;
                case AssetTypes.Voice:
                    AudioEntries = new ObservableCollection<Music>(_skeletonGameProvider.AssetsConfig.Audio.Voice);
                    break;
                case AssetTypes.Sfx:
                    AudioEntries = new ObservableCollection<Music>(_skeletonGameProvider.AssetsConfig.Audio.Effects);
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
