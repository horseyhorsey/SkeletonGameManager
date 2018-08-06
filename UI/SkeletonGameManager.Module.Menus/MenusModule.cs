using SkeletonGameManager.Module.Menus.Views;
using Prism.Modularity;
using Prism.Regions;

namespace SkeletonGameManager.Module.Menus
{
    public class MenusModule : IModule
    {
        private IRegionManager _regionManager;

        public MenusModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {            
            _regionManager.RegisterViewWithRegion("FileMenuRegion", typeof(FileMenuView));
        }
    }
}