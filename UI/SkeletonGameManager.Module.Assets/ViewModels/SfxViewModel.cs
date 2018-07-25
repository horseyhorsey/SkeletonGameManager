using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGameManager.Base;

namespace SkeletonGameManager.Module.Assets.ViewModels
{
    public class SfxViewModel : SoundViewModel
    {
        public SfxViewModel(ISkeletonGameFiles skeletonGameFiles, ISkeletonGameProvider skeletonGameProvider) : base(skeletonGameFiles, skeletonGameProvider)
        {
        }
    }
}
