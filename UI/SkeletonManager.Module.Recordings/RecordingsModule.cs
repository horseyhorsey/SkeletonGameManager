using SkeletonGameManager.Module.Recordings.Views;
using Prism.Modularity;
using Prism.Regions;
using System;
using Microsoft.Practices.Unity;
using Prism.Unity;

namespace SkeletonGameManager.Module.Recordings
{
    public class RecordingsModule : IModule
    {
        private IRegionManager _regionManager;
        private IUnityContainer _container;

        public RecordingsModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            //_container.RegisterTypeForNavigation<RecordingsView>();
            _regionManager.RegisterViewWithRegion("RecordingsRegion", typeof(RecordingsView));
        }
    }
}