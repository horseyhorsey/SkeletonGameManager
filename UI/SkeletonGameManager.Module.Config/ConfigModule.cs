using SkeletonGameManager.Module.Config.Views;
using Prism.Modularity;
using Prism.Regions;
using System;
using Microsoft.Practices.Unity;
using Prism.Unity;
using SkeletonGameManager.Module.Config.ViewModels;

namespace SkeletonGameManager.Module.Config
{
    public class ConfigModule : IModule
    {
        private IRegionManager _regionManager;
        private IUnityContainer _container;

        public ConfigModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            //_container.RegisterTypeForNavigation<ViewA>();
            //_container.RegisterTypeForNavigation<GameConfigView>("GameConfigView");

            _container.RegisterInstance(_container.Resolve<KeyboardMappingsViewModel>());
            _regionManager.RegisterViewWithRegion("ConfigRegion", typeof(GameConfigView));
            _regionManager.RegisterViewWithRegion("MachineRegion", typeof(MachineConfigView));
        }
    }
}