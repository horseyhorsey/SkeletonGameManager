
using SkeletonGameManager.Module.SceneManage.Views;
using Prism.Modularity;
using Microsoft.Practices.Unity;

using SkeletonGameManager.Module.SceneManage.ViewModels;

namespace SkeletonGameManager.Module.SceneManage
{
    public class SceneManageModule : IModule
    {
        private IUnityContainer _container;

        public SceneManageModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<object, AttractView>("AttractView");
            _container.RegisterType<object, SequenceView>("SequenceView");

            _container.RegisterInstance(_container.Resolve<AttractViewModel>(), new ContainerControlledLifetimeManager());
        }
    }
}