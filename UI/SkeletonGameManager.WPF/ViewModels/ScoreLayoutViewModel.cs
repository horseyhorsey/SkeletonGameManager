using System.Threading.Tasks;
using Prism.Events;
using SkeletonGame.Models.Score;
using SkeletonGameManager.WPF.Events;
using SkeletonGameManager.WPF.Providers;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class ScoreLayoutViewModel : SkeletonGameManagerViewModelBase
    {
        public ISkeletonGameProvider SkeletonGameProvider { get; set; }

        public ScoreLayout ScoreLayout { get; private set; } = null;

        public ScoreLayoutViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider) : base(eventAggregator)
        {
            SkeletonGameProvider = skeletonGameProvider;            

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x => await OnLoadYamlFilesChanged());
        }

        public async override Task OnLoadYamlFilesChanged()
        {
            ScoreLayout = SkeletonGameProvider.ScoreDisplayConfig?.ScoreLayout;

            await Task.Delay(1000);
        }
    }
}
