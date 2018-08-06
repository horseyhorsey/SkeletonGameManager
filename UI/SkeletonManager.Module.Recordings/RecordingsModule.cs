using SkeletonGameManager.Module.Recordings.Views;
using Prism.Modularity;
using Prism.Regions;
using Microsoft.Practices.Unity;
using SkeletonGameManager.Module.Recordings.ViewModels;

namespace SkeletonGameManager.Module.Recordings
{
    public class RecordingsModule : IModule
    {
        private IUnityContainer _container;

        public RecordingsModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterInstance(_container.Resolve<RecordingsViewModel>(), new ContainerControlledLifetimeManager());            
            _container.RegisterType<object, RecordingsView>("RecordingsView");
        }
    }
}