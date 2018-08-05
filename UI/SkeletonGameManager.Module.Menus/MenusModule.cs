﻿using SkeletonGameManager.Module.Menus.Views;
using Prism.Modularity;
using Prism.Regions;
using System;
using Microsoft.Practices.Unity;
using Prism.Unity;

namespace SkeletonGameManager.Module.Menus
{
    public class MenusModule : IModule
    {
        private IRegionManager _regionManager;
        private IUnityContainer _container;

        public MenusModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {            
            _regionManager.RegisterViewWithRegion("FileMenuRegion", typeof(FileMenuView));
        }
    }
}