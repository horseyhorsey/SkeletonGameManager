using System;
using System.Threading.Tasks;
using Prism.Events;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SkeletonGame.Engine;
using System.Linq;
using System.IO;
using System.Configuration;
using SkeletonGameManager.Base;
using SkeletonGameManager.Module.SceneGrab.Model;
using static SkeletonGameManager.Module.SceneGrab.Events.ViewModelEvents;
using Prism.Logging;

namespace SkeletonGameManager.Module.SceneGrab.ViewModels
{
    public class SceneProcessViewModel : SkeletonGameManagerViewModelBase
    {
        private ISkeletonGameProvider _skeletonGameProvider;
        private Uri _voiceDir;
        private Uri _dmdPath;
        private string _ffmpeg;

        public ICommand ProcessListCommand { get; set; }

        public SceneProcessViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider, ILoggerFacade loggerFacade) 
            : base(eventAggregator, loggerFacade)
        {
            _skeletonGameProvider = skeletonGameProvider;
            _eventAggregator.GetEvent<VideoProcessItemAddedEvent>().Subscribe(OnVideoProcessAdded);
            _eventAggregator.GetEvent<VideoProcessItemRemoveEvent>().Subscribe(x => this.VideoProcessItems.Remove(x));

            ProcessListCommand = new DelegateCommand(async () => await ProcessList());

            VideoProcessItems = new ObservableCollection<SceneProcessItemViewModel>();

            _ffmpeg = ConfigurationManager.AppSettings["ffmpegRoot"];
        }

        /// <summary>
        /// Does the process list have valid names and not empty strings.
        /// </summary>
        /// <returns></returns>
        private bool DoesProcessListHaveValidNamesAndNotEmptyStrings()
        {
            bool result = true;

            if (VideoProcessItems.Any(x => string.IsNullOrWhiteSpace(x.ExportName)))
                result = false;

            return result;
        }

        /// <summary>
        /// Checks the list making sure at least export video and audio is checked
        /// </summary>
        /// <returns></returns>
        private bool DoesProcessListHaveValidExportOptions()
        {
            foreach (var videoItem in VideoProcessItems)
            {
                if (!videoItem.ExportAudio && !videoItem.ExportVideo)
                    return false;
            }

            return true;
        }

        private async Task ProcessList()
        {
            //Check all is ok before continuing
            if (!CheckIfProcessCanBeInvoked()) return;

            var animPath = _skeletonGameProvider.GameConfig.DmdPath;
            if (animPath.Contains("."))
                _dmdPath = new Uri(Path.Combine(_skeletonGameProvider.GameFolder, animPath), UriKind.RelativeOrAbsolute);
            else
                _dmdPath = new Uri(animPath);

            var voicePath = _skeletonGameProvider.GameConfig.VoiceDir;
            _voiceDir = new Uri(Path.Combine(_skeletonGameProvider.GameFolder, _skeletonGameProvider.GameConfig.SoundPath, voicePath), UriKind.RelativeOrAbsolute);

            VideoHelper.AudioExportFolder = _voiceDir.AbsolutePath;
            VideoHelper.VideoExportFolder = _dmdPath.AbsolutePath;

            //Get the assets path and create a temp directory to store the conversions
            var assetsPath = _dmdPath.AbsolutePath.Replace(@"/dmd", string.Empty);
            if (!Directory.Exists(assetsPath + @"\temp"))
                Directory.CreateDirectory(assetsPath + @"\temp");

            //Show a window

            await Task.Run(() =>
            {
                foreach (var video in VideoProcessItems)
                {
                    try
                    {
                        //Temp video file name
                        var tempOutputFile = Path.Combine(assetsPath, "temp", video.ExportName + ".mp4");

                        var ffmpeg = _ffmpeg + "\\x86\\ffmpeg.exe";

                        //Is user wanting to resize to match the display resolution?
                        string resolution = null;
                        if (video.ResizeToDmdSize)
                            resolution = $"{ _skeletonGameProvider.GameConfig.DmdDotsWidth}x{ _skeletonGameProvider.GameConfig.DmdDotsHeight}";

                        //Is user just exporting the audio? If so we don't need to reencode the file
                        // Is not precise!
                        //if (video.ExportAudio && !video.ExportVideo)
                        //{
                        //    tempOutputFile = Path.Combine(assetsPath, "temp", video.ExportName + "." + video.SelectedAudioType.ToString());
                        //    VideoHelper.ConvertAudioClip(ffmpeg, video.File, tempOutputFile, video.Video.StartFrame, video.Video.Frames - 1, video.Video.FrameRate);
                        //}

                        //Process this clip to the temporary directory
                        VideoHelper.ConvertVideoClip(ffmpeg,
                            video.File, tempOutputFile,
                            video.Video.StartTime, video.Video.EndTime, video.Video.FrameRate, resolution,
                            video.SelectedSpeed.ToString());

                        //Split the temporary file and put into asset folders
                        if (video.SplitVideoAndAudio)
                        {
                            VideoHelper.SplitAudioAndVideo(ffmpeg, tempOutputFile, video.ExportName, video.ExportVideo, video.ExportAudio, video.SelectedAudioType.ToString());
                        }


                    }
                    //Any errors show to the user
                    catch (Exception ex) { System.Windows.MessageBox.Show(ex.Message); return; }
                }
            });

            //Remove the busy window

            VideoProcessItems.Clear();
        }

        private bool CheckIfProcessCanBeInvoked()
        {
            if (!DoesProcessListHaveValidNamesAndNotEmptyStrings())
            {
                System.Windows.MessageBox.Show("Fill in the export name in the process list.");
                return false;
            }

            if (!DoesProcessListHaveValidExportOptions())
            {
                System.Windows.MessageBox.Show("Make sure an export option is checked for each item.");
                return false;
            }

            return true;
        }

        private void OnVideoProcessAdded(TrimVideo trimVideo)
        {
            VideoProcessItems.Add(new SceneProcessItemViewModel(_eventAggregator)
            {
                File = trimVideo.File,
                Video = trimVideo,
                Overwrite = false,
            });
        }

        public ObservableCollection<SceneProcessItemViewModel> VideoProcessItems { get; set; }
    }
}
