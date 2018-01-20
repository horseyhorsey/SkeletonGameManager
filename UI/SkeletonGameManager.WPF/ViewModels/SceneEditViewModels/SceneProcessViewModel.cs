using System;
using System.Threading.Tasks;
using Prism.Events;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SkeletonGameManager.WPF.Model;
using SkeletonGameManager.WPF.Events;
using SkeletonGame.Engine;

namespace SkeletonGameManager.WPF.ViewModels.SceneEditViewModels
{
    public class SceneProcessViewModel : SkeletonGameManagerViewModelBase
    {

        public SceneProcessViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            
            _eventAggregator.GetEvent<VideoProcessItemAddedEvent>().Subscribe(OnVideoProcessAdded);
            _eventAggregator.GetEvent<VideoProcessItemRemoveEvent>().Subscribe(x => this.VideoProcessItems.Remove(x));

            ProcessListCommand = new DelegateCommand(async () => await ProcessList());

            VideoProcessItems = new ObservableCollection<SceneProcessItemViewModel>();
        }

        private async Task ProcessList()
        {
            await Task.Run(() =>
            {
                var ffmpeg = string.Empty;
                foreach (var video in VideoProcessItems)
                {
                    try
                    {
                        VideoHelper.TrimVideoRange(ffmpeg, video.File, @"C:\Temp\OutputProcess.mp4", video.StartTime, video.EndTime);
                    }
                    catch (Exception ex) { }
                }
            });

            VideoProcessItems.Clear();
        }

        private void OnVideoProcessAdded(TrimVideo trimVideo)
        {
            VideoProcessItems.Add(new SceneProcessItemViewModel(_eventAggregator)
            {
                File = trimVideo.File,
                StartTime = trimVideo.Start,
                EndTime = trimVideo.End,
                Duration = trimVideo.End - trimVideo.Start,
                Overwrite = false,
            });

            trimVideo = null;

        }

        public ICommand ProcessListCommand { get; set; }

        public ObservableCollection<SceneProcessItemViewModel> VideoProcessItems { get; set; }
    }
}
