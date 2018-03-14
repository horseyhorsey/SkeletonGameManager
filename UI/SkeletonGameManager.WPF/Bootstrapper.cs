﻿using Microsoft.Practices.Unity;
using Prism.Unity;
using System.Windows;
using Prism.Modularity;
using SkeletonGame.Engine;
using SkeletonGameManager.WPF.Views;
using SkeletonGameManager.WPF.Providers;
using Prism.Regions;
using SkeletonGameManager.WPF.ViewModels;

namespace SkeletonGameManager.WPF
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell() => Container.Resolve<MainWindow>();

        protected override void InitializeShell()
        {
            base.InitializeShell();

            //Register views with regions
            var regionManager = this.Container.Resolve<IRegionManager>();
            if (regionManager != null)
            {                                
                regionManager.RegisterViewWithRegion("ConfigRegion", typeof(GameConfigView));
                regionManager.RegisterViewWithRegion("RecordingsRegion", typeof(RecordingsView));
                regionManager.RegisterViewWithRegion("AssetsRegion", typeof(AssetListView));
                regionManager.RegisterViewWithRegion("AttractRegion", typeof(AttractView));
                regionManager.RegisterViewWithRegion("ScenesRegion", typeof(ScenesView));
                regionManager.RegisterViewWithRegion("MachineRegion", typeof(MachineConfigView));
                regionManager.RegisterViewWithRegion("SequencesRegion", typeof(SequenceView));
                regionManager.RegisterViewWithRegion("ScoreLayoutRegion", typeof(ScoreLayoutView));                
            }

            Application.Current.MainWindow.Show();                                            
        }

        protected override void ConfigureModuleCatalog()
        {
            var moduleCatalog = (ModuleCatalog)ModuleCatalog;

            //moduleCatalog.AddModule(typeof(Skeleton.Assets.AssetsModule));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            
            Container.RegisterInstance<ISkeletonLogger>(Container.Resolve<SkeletonLogger>());

            Container.RegisterInstance<ISkeletonOSC>(Container.Resolve<SkeletonOSC>());

            Container.RegisterInstance<ISkeletonGameSerializer>(Container.Resolve<SkeletonGameSerializer>());

            Container.RegisterInstance<ISkeletonGameFiles>(Container.Resolve<SkeletonGameFiles>());

            Container.RegisterInstance<ISkeletonGameProvider>(Container.Resolve<SkeletonGameProvider>());
            Container.RegisterInstance<ISkeletonGameExport>(Container.Resolve<SkeletonGameExport>());

            Container.RegisterInstance(Container.Resolve<KeyboardMappingsViewModel>());
            Container.RegisterInstance(Container.Resolve<ScoreLayoutViewModel>());

            Container.RegisterTypeForNavigation<GameConfigView>("GameConfigView");                       
        }
    }
}
