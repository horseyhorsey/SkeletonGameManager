using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using SkeletonGame.Models.Score;
using SkeletonGameManager.Base;
using static SkeletonGameManager.Base.Events;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class ScoreLayoutViewModel : SkeletonTabViewModel
    {
        public ISkeletonGameProvider _skeletonGameProvider { get; set; }

        public ScoreLayoutViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
            Title = "Score Display";

            _skeletonGameProvider = skeletonGameProvider;

            //Save new_score_display.yaml
            SaveCommand = new DelegateCommand(() =>
            {
                OnSaveCommand();
            });

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x =>
            {
                try
                {
                    await OnLoadYamlFilesChanged();
                }
                catch (System.Exception)
                {
                    throw;
                }
            }            
            );
        }

        public async override Task OnLoadYamlFilesChanged()
        {
            ScoreLayout = _skeletonGameProvider.ScoreDisplayConfig?.ScoreLayout;

            Animations = new ObservableCollection<string>(
                _skeletonGameProvider.AssetsConfig?.Animations
                .Select(x => x.Key));

            await Task.Delay(100);
        }

        private ObservableCollection<string> animations;
        public ObservableCollection<string> Animations
        {
            get { return animations; }
            set { SetProperty(ref animations, value); }
        }

        private ScoreLayout scoreLayout = new ScoreLayout();
        public ScoreLayout ScoreLayout
        {
            get { return scoreLayout; }
            set { SetProperty(ref scoreLayout, value); }
        }

        private void OnSaveCommand()
        {
            _skeletonGameProvider.ScoreDisplayConfig.ScoreLayout = ScoreLayout;
            _skeletonGameProvider.SaveScoreDsiplayFile(_skeletonGameProvider.ScoreDisplayConfig);
        }
    }
}
