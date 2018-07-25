using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGameManager.Base;

namespace SkeletonGameManager.Module.Assets.ViewModels
{
    public class MusicViewModel : SoundViewModel
    {
        public MusicViewModel(ISkeletonGameFiles skeletonGameFiles, ISkeletonGameProvider skeletonGameProvider) : base(skeletonGameFiles, skeletonGameProvider)
        {
        }
    }
}
