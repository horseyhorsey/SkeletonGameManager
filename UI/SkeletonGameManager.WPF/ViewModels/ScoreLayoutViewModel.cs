using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using SkeletonGame.Models.Score;
using SkeletonGameManager.WPF.Events;
using SkeletonGameManager.WPF.Providers;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class ScoreLayoutViewModel : SkeletonGameManagerViewModelBase
    {
        public ISkeletonGameProvider _skeletonGameProvider { get; set; }        

        public ScoreLayoutViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider) : base(eventAggregator)
        {
            _skeletonGameProvider = skeletonGameProvider;

            //Save new_score_display.yaml
            SaveCommand = new DelegateCommand(() =>
            {
                OnSaveCommand();
            });

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x => await OnLoadYamlFilesChanged());
        }

        public async override Task OnLoadYamlFilesChanged()
        {
            ScoreLayout = _skeletonGameProvider.ScoreDisplayConfig?.ScoreLayout;

            Animations = new ObservableCollection<string>(
                _skeletonGameProvider.AssetsConfig.Animations
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
