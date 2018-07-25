using SkeletonGameManager.Module.SceneGrab.Views;
using Prism.Modularity;
using Prism.Regions;
using System;
using Microsoft.Practices.Unity;
using Prism.Unity;

namespace SkeletonGameManager.Module.SceneGrab
{
    public class SceneGrabModule : IModule
    {
        private IRegionManager _regionManager;
        private IUnityContainer _container;

        public SceneGrabModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("ScenesRegion", typeof(ScenesView));
        }
    }
}