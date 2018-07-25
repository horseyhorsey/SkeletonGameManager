using SkeletonGameManager.Module.Assets.Views;
using Prism.Modularity;
using Prism.Regions;
using Microsoft.Practices.Unity;
using SkeletonGameManager.Module.Assets.ViewModels;

namespace SkeletonGameManager.Module.Assets
{
    public class AssetsModule : IModule
    {
        private IRegionManager _regionManager;
        private IUnityContainer _container;

        public AssetsModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {            
            _container.RegisterInstance(typeof(FontsViewModel), _container.Resolve<FontsViewModel>(), new ContainerControlledLifetimeManager());            
            _container.RegisterInstance(typeof(SfxViewModel), _container.Resolve<SfxViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(typeof(MusicViewModel), _container.Resolve<MusicViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(typeof(VoiceViewModel), _container.Resolve<VoiceViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(typeof(LampshowViewModel), _container.Resolve<LampshowViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(typeof(AnimationsViewModel), _container.Resolve<AnimationsViewModel>(), new ContainerControlledLifetimeManager());
        }
    }
}