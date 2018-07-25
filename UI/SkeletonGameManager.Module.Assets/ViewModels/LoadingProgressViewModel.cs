using SkeletonGame.Models;

namespace SkeletonGameManager.Module.Assets.ViewModels
{
    public class LoadingProgressViewModel : AssetFileBaseViewModel
    {
        public LoadingProgressViewModel(UserInterface userInterface)
        {
            UserInterface = userInterface;
        }

        private UserInterface userInterface;
        public UserInterface UserInterface
        {
            get { return userInterface; }
            set { SetProperty(ref userInterface, value); }
        }
    }
}
