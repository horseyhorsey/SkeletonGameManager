using SkeletonGame.Engine;
using SkeletonGame.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace SkeletonGameManager.WPF.ViewModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class PlaybackItemViewModel
    {
        public PlaybackItemViewModel(string playBackFile)
        {
            PlaybackFile = playBackFile;
        }

        public string PlaybackFile { get; set; }

        public ObservableCollection<PlayBackItem> PlayBackItems { get; set; }

        public void UpdatePlayBackItems(bool orderByDescending = false)
        {
            PlayBackItems = new ObservableCollection<PlayBackItem>(RecordingManager.ParsePlaybackFile(PlaybackFile));

            if (orderByDescending)
                PlayBackItems = new ObservableCollection<PlayBackItem>(PlayBackItems.OrderByDescending(x => x.Event));
        }
    }
}
