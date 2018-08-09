using SkeletonGameManager.Module.LogViewer.Views;
using Prism.Modularity;
using Microsoft.Practices.Unity;
using SkeletonGameManager.Module.LogViewer.ViewModels;

namespace SkeletonGameManager.Module.LogViewer
{
    public class LogViewerModule : IModule
    {
        private IUnityContainer _container;

        public LogViewerModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterInstance(_container.Resolve<AppLogViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(_container.Resolve<SkeletonGameLogViewModel>(), new ContainerControlledLifetimeManager());

            ////Register type of view with container to be resolved.
            _container.RegisterType<object, AppLogView>("AppLogView");
            _container.RegisterType<object, SkeletonGameLogView>("SkeletonGameLogView");
        }
    }
}