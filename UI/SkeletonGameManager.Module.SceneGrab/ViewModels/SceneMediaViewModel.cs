using System;
using System.Windows.Input;
using Prism.Events;
using Prism.Commands;
using GongSolutions.Wpf.DragDrop;
using System.Windows;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using FFMpegSharp;
using SkeletonGameManager.Base;
using SkeletonGameManager.Module.SceneGrab.Model;
using static SkeletonGameManager.Module.SceneGrab.Events.ViewModelEvents;
using static SkeletonGameManager.Base.Events;

namespace SkeletonGameManager.Module.SceneGrab.ViewModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class SceneMediaViewModel : SkeletonGameManagerViewModelBase, IDropTarget
    {
        public IMediaPlayer _mediaElement;
        private ISkeletonGameProvider _skeletonGameProvider;

        #region Commands
        public ICommand MarkVideoRangeCommand { get; set; }
        public DelegateCommand<IMediaPlayer> MediaElementLoadedCommand { get; set; }
        public ICommand VideoControlCommand { get; set; }
        public ICommand AddToProcessListCommand { get; set; }

        #endregion

        public SceneMediaViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider) : base(eventAggregator)
        {
            _skeletonGameProvider = skeletonGameProvider;

            MarkVideoRangeCommand = new DelegateCommand<string>(MarkVideoRange);
            MediaElementLoadedCommand = new DelegateCommand<IMediaPlayer>(MediaElementLoaded);
            VideoControlCommand = new DelegateCommand<string>(OnVideoControl);

            _eventAggregator.GetEvent<VideoSourceEvent>().Subscribe(OnVideoSourceUpdated);

            //Add the selection start and end to process list
            AddToProcessListCommand = new DelegateCommand(AddToProcessList);            
        }

        #region Properties
        private string videoPreviewHeader;
        public string VideoPreviewHeader
        {
            get { return videoPreviewHeader; }
            set { SetProperty(ref videoPreviewHeader, value); }
        }

        private VideoInfo loadedMediaInfo;
        public VideoInfo LoadedMediaInfo
        {
            get { return loadedMediaInfo; }
            set { SetProperty(ref loadedMediaInfo, value); }
        }

        private int sliderVlue;
        public int SliderValue
        {
            get { return sliderVlue; }
            set { SetProperty(ref sliderVlue, value); }
        }

        private int selectionEnd;
        public int SelectionEnd
        {
            get { return selectionEnd; }
            set
            {
                SetProperty(ref selectionEnd, value);

                _mediaElement.SetPosition(SelectionEnd);
                EndTime = _mediaElement.GetCurrentPosition();
            }
        }

        private int selectionStart;
        public int SelectionStart
        {
            get { return selectionStart; }
            set
            {
                SetProperty(ref selectionStart, value);

                _mediaElement.SetPosition(SelectionStart);
                StartTime = _mediaElement.GetCurrentPosition();
            }
        }

        private string videoSource;
        public string VideoSource
        {
            get { return videoSource; }
            set { SetProperty(ref videoSource, value); }
        }

        private TimeSpan startTime;
        public TimeSpan StartTime
        {
            get { return startTime; }
            set { SetProperty(ref startTime, value); }
        }

        private TimeSpan endTime;
        public TimeSpan EndTime
        {
            get { return endTime; }
            set { SetProperty(ref endTime, value); }
        }
        #endregion

        #region Public Methods

        public void DragOver(IDropInfo dropInfo)
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

        public void Drop(IDropInfo dropInfo)
        {
            List<string> droppedFiles = new List<string>();

            //Needs a few checks here. We can be dragging in from explorer or across to the datagrid.
            var dragFileList = dropInfo.Data;

            //Dragged files from windows
            if (dragFileList.GetType() == typeof(DataObject))
            {
                var windowsFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();

                droppedFiles.AddRange(windowsFileList);

                VideoSource = droppedFiles[0];
                _eventAggregator.GetEvent<VideoSourceEvent>().Publish(VideoSource);
            }
        }

        #endregion

        #region Support Methods

        /// <summary>
        /// Creates a <see cref="TrimVideo"/> object and adds to process list.
        /// </summary>
        private void AddToProcessList()
        {
            var trimVideoItem = new TrimVideo
            {
                File = VideoSource,
                StartFrame = SelectionStart,
                Frames = SelectionEnd - SelectionStart,
                FrameRate = _mediaElement.FrameRate,     
                StartTime = this.StartTime,
                EndTime = this.EndTime
            };

            _eventAggregator.GetEvent<VideoProcessItemAddedEvent>()
                .Publish(trimVideoItem);
        }

        /// <summary>
        /// Called when the Loaded event happens on the a MediaElement
        /// </summary>
        /// <param name="mediaPlayer">The media player.</param>
        private void MediaElementLoaded(IMediaPlayer mediaPlayer)
        {
            if (mediaPlayer != null)
            {
                _mediaElement = mediaPlayer;
                SliderValue = 0;
            }
        }

        /// <summary>
        /// Called when a button is fired in the view
        /// </summary>
        /// <param name="obj">The object.</param>
        private void OnVideoControl(string obj)
        {
            switch (obj)
            {
                case "pause":
                    _mediaElement.Pause();
                    break;
                case "play":
                    _mediaElement.Play();
                    break;
                case "stop":
                    _mediaElement.Stop();
                    break;
                default:
                    break;
            }
        }

        private void OnVideoSourceUpdated(string obj)
        {
            VideoSource = obj;

            VideoPreviewHeader = obj;

            LoadedMediaInfo = FFMpegSharp.VideoInfo.FromPath(obj);

            //Set the framerate and count from the loaded video
            _mediaElement.FrameRate = LoadedMediaInfo.FrameRate;
            _mediaElement.FrameCount = Math.Ceiling(LoadedMediaInfo.Duration.TotalSeconds * LoadedMediaInfo.FrameRate);            
        }

        private void MarkVideoRange(string inOut)
        {
            try
            {
                if (inOut == "In")
                {
                    SelectionStart = SliderValue;
                    StartTime = _mediaElement.GetCurrentPosition();                   
                }
                else if (inOut == "Out")
                {
                    SelectionEnd = SliderValue;
                    EndTime = _mediaElement.GetCurrentPosition();
                }

            }
            catch { }
        }
        #endregion
    }

}
