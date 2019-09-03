
using SkeletonGameManager.Module.Config.Views;
using Prism.Modularity;
using Microsoft.Practices.Unity;
using SkeletonGameManager.Module.Config.ViewModels;

namespace SkeletonGameManager.Module.Config
{
    public class ConfigModule : IModule
    {
        private IUnityContainer _container;

        public ConfigModule(IUnityContainer container)
        {
            _container = container;            
        }

        public void Initialize()
        {
            _container.RegisterInstance(_container.Resolve<KeyboardMappingsViewModel>());

            //Create and register as singletons
            _container.RegisterInstance(_container.Resolve<GameConfigViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(_container.Resolve<MachineConfigViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(_container.Resolve<VisualPinballViewModel>(), new ContainerControlledLifetimeManager());

            //Register type of view with container to be resolved.
            _container.RegisterType<object, GameConfigView>("GameConfigView");
            _container.RegisterType<object, MachineConfigView>("MachineConfigView");
            _container.RegisterType<object, VisualPinballView>("VisualPinballView");
        }
    }
}