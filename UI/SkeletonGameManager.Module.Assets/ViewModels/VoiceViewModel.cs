using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGameManager.Base;

namespace SkeletonGameManager.Module.Assets.ViewModels
{
    public class VoiceViewModel : SoundViewModel
    {
        public VoiceViewModel(ISkeletonGameFiles skeletonGameFiles, ISkeletonGameProvider skeletonGameProvider) : base(skeletonGameFiles, skeletonGameProvider)
        {
        }
    }
}
