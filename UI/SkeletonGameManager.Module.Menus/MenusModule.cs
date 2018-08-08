using SkeletonGameManager.Module.Menus.Views;
using Prism.Modularity;
using Prism.Regions;
using Microsoft.Practices.Unity;
using SkeletonGameManager.Module.Menus.Model;
using SkeletonGameManager.Base.Interface;

namespace SkeletonGameManager.Module.Menus
{
    public class MenusModule : IModule
    {
        private IRegionManager _regionManager;
        private IUnityContainer _unityContainer;

        public MenusModule(IRegionManager regionManager, IUnityContainer unityContainer)
        {
            _regionManager = regionManager;
            _unityContainer = unityContainer;
            _unityContainer.RegisterInstance<IAppSettingsModel>(_unityContainer.Resolve<AppSettingsModel>(), new ContainerControlledLifetimeManager());
        }

        public void Initialize()
        {            
            _regionManager.RegisterViewWithRegion("FileMenuRegion", typeof(FileMenuView));
                       
        }
    }
}