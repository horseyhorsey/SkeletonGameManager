using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using SkeletonGame.Engine;
using SkeletonGameManager.Base;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using static SkeletonGameManager.Base.Events;

namespace SkeletonGameManager.Module.Recordings.ViewModels
{
    public class RecordingsViewModel : SkeletonTabViewModel
    {
        #region Fields
        private ISkeletonGameProvider _skeletonGameProvider;
        private bool _launchCommandEnabled = true;
        #endregion

        #region Commands
        public DelegateCommand LaunchGameCommand { get; set; } 
        #endregion

        #region Constructors
        public RecordingsViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
            _skeletonGameProvider = skeletonGameProvider;

            LaunchGameCommand = new DelegateCommand(() =>
            {
                LaunchGame(this.PlaybackItemViewModel);
            }, () => _launchCommandEnabled);

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x => await OnLoadYamlFilesChanged());
            _eventAggregator.GetEvent<OnGameEndedEvent>().Subscribe(x =>
            {
                OnGameEndedEventChanged(x);
            });
        } 
        #endregion

        #region Properties
        private ObservableCollection<PlaybackItemViewModel> playbackItemViewModels = new ObservableCollection<PlaybackItemViewModel>();
        public ObservableCollection<PlaybackItemViewModel> PlaybackItemViewModels
        {
            get { return playbackItemViewModels; }
            set { SetProperty(ref playbackItemViewModels, value); }
        }

        private PlaybackItemViewModel playbackItemViewModel;
        public PlaybackItemViewModel PlaybackItemViewModel
        {
            get { return playbackItemViewModel; }
            set { SetProperty(ref playbackItemViewModel, value); }
        }

        private bool playbackIsChecked = true;
        public bool PlaybackIsChecked
        {
            get { return playbackIsChecked; }
            set { SetProperty(ref playbackIsChecked, value); }
        }

        private bool recordIsChecked;
        private bool _launchedWithPlayback;

        public bool RecordIsChecked
        {
            get { return recordIsChecked; }
            set { SetProperty(ref recordIsChecked, value); }
        }
        #endregion

        #region Public Methods

        public void LaunchPlaybackFile(string gameFolder, string playbackFile, bool playback = false)
        {
            _launchCommandEnabled = false;
            this.LaunchGameCommand.RaiseCanExecuteChanged();

            if (playback)
            {
                RecordingManager.CopyPlayBackFileToGameRoot(gameFolder, playbackFile);
                RecordingManager.SetFakePinProcPlayback(gameFolder, playback);
            }
            else
            {
                RecordingManager.SetSkeletonGameBaseClass(Path.Combine(gameFolder, playbackFile), true);
            }

            _launchedWithPlayback = playback;
            _eventAggregator.GetEvent<OnLaunchGameEvent>().Publish(null);
        }

        public async override Task OnLoadYamlFilesChanged()
        {
            PlaybackItemViewModels.Clear();
            var recordingDir = _skeletonGameProvider.GameFolder + @"\recordings";

            Log($"Loading recordings from.. {recordingDir}");            
            await Task.Run(() =>
             {
                 //Populate the recordings directory
                 RecordingManager.GetPlaybackFiles(recordingDir);

             });

            Log($"Populating recordings.");
            foreach (var playbackFile in RecordingManager.PlayBackFiles)
            {
                var vm = new PlaybackItemViewModel(playbackFile);
                vm.UpdatePlayBackItems(true);
                PlaybackItemViewModels.Add(vm);
            }

            if (PlaybackItemViewModels.Count > 0)
                Log($"Recordings loaded successfully");
            else
                Log($"No recordings populated");
        }

        #endregion

        #region Private Methods

        private void LaunchGame(PlaybackItemViewModel playbackItem = null)
        {
            if (PlaybackIsChecked)
            {
                Log("Running recording playback");
                if (playbackItem != null)
                {
                    LaunchPlaybackFile(_skeletonGameProvider.GameFolder, this.playbackItemViewModel.PlaybackFile, true);
                }
            }
            else
            {
                Log("New recording initiated.");
                //Replace the skeleton game base class, could be recording or not.
                var skeleGame = Path.Combine("procgame", "game", "skeletongame.py");
                LaunchPlaybackFile(_skeletonGameProvider.GameFolder, skeleGame);
            }
        }

        /// <summary>
        /// Called when [game ended event changed]. Sets any recording parameters back to the normal game classes.
        /// </summary>
        /// <param name="x">if set to <c>true</c> [x].</param>
        private void OnGameEndedEventChanged(bool x)
        {
            _launchCommandEnabled = x;

            if (_launchedWithPlayback)
            {
                Log("Setting FAKEPINPROC_CLASS");
                RecordingManager.SetFakePinProcPlayback(_skeletonGameProvider.GameFolder, false);
            }
            else
            {
                Log("Setting BASICGAME_CLASS");
                //Replace the skeleton game base class, could be recording or not.                
                var skeleGame = Path.Combine(_skeletonGameProvider.GameFolder, "procgame", "game", "skeletongame.py");                
                RecordingManager.SetSkeletonGameBaseClass(skeleGame, false);
            }

            this.LaunchGameCommand.RaiseCanExecuteChanged();
        }
        #endregion
    }
}
