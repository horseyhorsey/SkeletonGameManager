using Prism.Events;
using Prism.Mvvm;
using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGameManager.WPF.Providers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkeletonGameManager.WPF.ViewModels
{
    class RecordingsViewModel : SkeletonGameManagerViewModelBase
    {
        private ISkeletonGameProvider _skeletonGameProvider;

        public RecordingsViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider) : base(eventAggregator)
        {
            _skeletonGameProvider = skeletonGameProvider;
        }

        private static IList<PlayBackItem> GetPlayBackItems(string playBackFile)
        {
            return RecordingManager.ParsePlaybackFile(playBackFile);
        }

        public async override Task OnLoadYamlFilesChanged()
        {
            await Task.Run(() =>
             {
                //Populate the recordings directory
                RecordingManager.GetPlaybackFiles(_skeletonGameProvider.GameFolder + @"\recordings");
             });
        }
    }
}
