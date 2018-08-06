using Prism.Modularity;
using Microsoft.Practices.Unity;
using SkeletonGameManager.Module.Assets.ViewModels;
using SkeletonGameManager.Module.Assets.Views;

namespace SkeletonGameManager.Module.Assets
{
    public class AssetsModule : IModule
    {
        private IUnityContainer _container;

        public AssetsModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<object, AssetListView>("AssetListView");
            
            //Register these view models with container.
            _container.RegisterInstance(_container.Resolve<AssetListViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(_container.Resolve<FontsViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(_container.Resolve<SfxViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(_container.Resolve<MusicViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(_container.Resolve<VoiceViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(_container.Resolve<LampshowViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(_container.Resolve<AnimationsViewModel>(), new ContainerControlledLifetimeManager());
        }
    }
}