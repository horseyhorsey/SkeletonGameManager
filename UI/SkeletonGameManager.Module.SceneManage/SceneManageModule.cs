using SkeletonGameManager.Module.SceneManage.Views;
using Prism.Modularity;
using Prism.Regions;
using System;
using Microsoft.Practices.Unity;
using Prism.Unity;

namespace SkeletonGameManager.Module.SceneManage
{
    public class SceneManageModule : IModule
    {
        private IRegionManager _regionManager;
        private IUnityContainer _container;

        public SceneManageModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("AttractRegion", typeof(AttractView));
            _regionManager.RegisterViewWithRegion("SequencesRegion", typeof(SequenceView));
        }
    }
}