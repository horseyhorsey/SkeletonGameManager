﻿using Prism.Modularity;
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
            //Register these view models with container.            
            _container.RegisterInstance(_container.Resolve<AnimationsViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(_container.Resolve<AssetDetailsViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(_container.Resolve<AssetListViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(_container.Resolve<FontsViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(_container.Resolve<LampshowViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(_container.Resolve<LoadingProgressViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(_container.Resolve<MusicViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(_container.Resolve<SfxViewModel>(), new ContainerControlledLifetimeManager());
            _container.RegisterInstance(_container.Resolve<VoiceViewModel>(), new ContainerControlledLifetimeManager());

            _container.RegisterType<object, AssetListView>("AssetListView");
            _container.RegisterType<object, AssetDetailsView>("AssetDetailsView");

            _container.RegisterType<object, AnimationsView>("AnimationsView");
            _container.RegisterType<object, FontsView>("FontsView");
            _container.RegisterType<object, LampshowView>("LampshowView");
            _container.RegisterType<object, LoadingProgressView>("LoadingProgressView");
            _container.RegisterType<object, MusicView>("MusicView");
            _container.RegisterType<object, SfxView>("SfxView");
            _container.RegisterType<object, VoiceView>("VoiceView");                 
        }
    }
}