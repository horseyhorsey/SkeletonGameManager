using Prism.Events;
using Prism.Logging;
using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGameManager.Base;

namespace SkeletonGameManager.Module.Assets.ViewModels
{
    public class SfxViewModel : SoundViewModel
    {
        public SfxViewModel(ISkeletonGameFiles skeletonGameFiles, ISkeletonGameProvider skeletonGameProvider, IEventAggregator eventAggregator, ILoggerFacade loggerFacade) : base(skeletonGameFiles, skeletonGameProvider, eventAggregator, loggerFacade)
        {
            Title = "Sfx";
        }
    }
}
