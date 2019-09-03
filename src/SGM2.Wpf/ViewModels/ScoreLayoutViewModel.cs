using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using SkeletonGame.Models.Score;
using SkeletonGameManager.Base;
using static SkeletonGameManager.Base.Events;

namespace SGM2.Wpf.ViewModels
{
    public class ScoreLayoutViewModel : SkeletonTabViewModel
    {
        public ISkeletonGameProvider _skeletonGameProvider { get; set; }

        public ScoreLayoutViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
            //TODO: Animations dropdown not updating if user adds to asset list or saves.
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
                catch (System.Exception ex)
                {
                    throw;
                }
            }            
            );
        }

        #region Properties
        private ObservableCollection<string> animations = new ObservableCollection<string>();
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
        #endregion

        public async override Task OnLoadYamlFilesChanged()
        {
            ScoreLayout = _skeletonGameProvider.ScoreDisplayConfig?.ScoreLayout;

            Animations.AddRange(_skeletonGameProvider.AssetsConfig?.Animations.Select(x => x.Key));

            await Task.Delay(100);
        }

        private void OnSaveCommand()
        {
            _skeletonGameProvider.ScoreDisplayConfig.ScoreLayout = ScoreLayout;
            _skeletonGameProvider.SaveScoreDsiplayFile(_skeletonGameProvider.ScoreDisplayConfig);
        }
    }
}
