using FFMpegSharp.FFMPEG.Enums;
using Prism.Commands;
using Prism.Events;
using SkeletonGame.Models;
using SkeletonGameManager.WPF.Events;
using SkeletonGameManager.WPF.Model;
using System;
using System.Windows.Input;

namespace SkeletonGameManager.WPF.ViewModels.SceneEditViewModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class SceneProcessItemViewModel
    {        
        #region Fields
        private IEventAggregator _eventAggregator;
        #endregion

        #region Commands
        public ICommand RemoveCommand { get; set; }
        #endregion

        public SceneProcessItemViewModel(IEventAggregator ea)
        {
            _eventAggregator = ea;

            RemoveCommand = new DelegateCommand(Remove);
        }

        #region Properties

        public TrimVideo Video { get; set; }

        public string ExportName { get; set; }
        public string SystemName { get; set; }
        public string File { get; set; }
        public bool Overwrite { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan Duration { get; set; }

        public bool ExportAudio { get; set; } = true;
        public bool ExportVideo { get; set; } = true;
        public bool ResizeToDmdSize { get; set; } = true;

        public bool SplitVideoAndAudio { get; set; } = true;

        public Speed SelectedSpeed { get; set; } = Speed.Medium;
        public AudioTypes SelectedAudioType { get; set; } = AudioTypes.ogg;     
        #endregion

        #region Private Methods
        private void Remove()
        {
            _eventAggregator.GetEvent<VideoProcessItemRemoveEvent>().Publish(this);
        } 
        #endregion
    }
}
