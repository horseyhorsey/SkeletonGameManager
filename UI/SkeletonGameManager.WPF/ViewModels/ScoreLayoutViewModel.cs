using System.Threading.Tasks;
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

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x => await OnLoadYamlFilesChanged());
        }

        public async override Task OnLoadYamlFilesChanged()
        {
            ScoreLayout = _skeletonGameProvider.ScoreDisplayConfig?.ScoreLayout;

            await Task.Delay(100);
        }

        private ScoreLayout scoreLayout = new ScoreLayout();
        public ScoreLayout ScoreLayout
        {
            get { return scoreLayout; }
            set { SetProperty(ref scoreLayout, value); }
        }
    }
}
