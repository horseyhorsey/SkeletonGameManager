using SkeletonGameManager.Module.SceneGrab.Views;
using Prism.Modularity;
using Microsoft.Practices.Unity;
using SkeletonGameManager.Module.SceneGrab.ViewModels;

namespace SkeletonGameManager.Module.SceneGrab
{
    public class SceneGrabModule : IModule
    {
        private IUnityContainer _container;

        public SceneGrabModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<object, ScenesView>("ScenesView");
            _container.RegisterInstance(_container.Resolve<ScenesViewModel>(), new ContainerControlledLifetimeManager());
        }
    }
}