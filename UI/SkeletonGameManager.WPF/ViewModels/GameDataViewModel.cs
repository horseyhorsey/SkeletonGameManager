using System.Threading.Tasks;
using Prism.Events;
using SkeletonGame.Models.Data;
using SkeletonGameManager.WPF.Events;
using SkeletonGameManager.WPF.Providers;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class GameDataViewModel : SkeletonGameManagerViewModelBase
    {
        public GameDataViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }
    }
}
