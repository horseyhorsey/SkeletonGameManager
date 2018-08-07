using Prism.Events;
using Prism.Logging;
using SkeletonGame.Engine;
using SkeletonGameManager.Base;

namespace SkeletonGameManager.Module.Assets.ViewModels
{
    public class VoiceViewModel : SoundViewModel
    {
        public VoiceViewModel(ISkeletonGameFiles skeletonGameFiles, ISkeletonGameProvider skeletonGameProvider, IEventAggregator eventAggregator, ILoggerFacade loggerFacade) : base(skeletonGameFiles, skeletonGameProvider, eventAggregator, loggerFacade)
        {
        }
    }
}
