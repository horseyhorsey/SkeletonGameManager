using Microsoft.Practices.Unity;
using Prism.Unity;
using System.Windows;
using Prism.Modularity;
using SkeletonGame.Engine;
using SkeletonGameManager.WPF.Views;
using Prism.Regions;
using SkeletonGameManager.Base;
using SkeletonGameManager.Module.Assets.Views;
using SkeletonGameManager.Module.Services;
using SkeletonGameManager.Module.Recordings.ViewModels;
using Prism.Logging;

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
                regionManager.RegisterViewWithRegion("AssetsRegion", typeof(AssetListView));
                regionManager.RegisterViewWithRegion("ScoreLayoutRegion", typeof(ScoreLayoutView));
                regionManager.RegisterViewWithRegion("GameDataRegion", typeof(GameDataView));

                //Game Data
                regionManager.RegisterViewWithRegion("TrophyDataRegion", typeof(TrophyDataView));                
            }

            Application.Current.MainWindow.Show();                                            
        }

        protected override void ConfigureModuleCatalog()
        {
            var moduleCatalog = (ModuleCatalog)ModuleCatalog;                                   
            moduleCatalog.AddModule(typeof(Module.Assets.AssetsModule));
            moduleCatalog.AddModule(typeof(Module.Config.ConfigModule));
            moduleCatalog.AddModule(typeof(Module.Recordings.RecordingsModule));
            moduleCatalog.AddModule(typeof(Module.SceneGrab.SceneGrabModule));
            moduleCatalog.AddModule(typeof(Module.SceneManage.SceneManageModule));
            moduleCatalog.AddModule(typeof(Module.Menus.MenusModule));
            moduleCatalog.Initialize();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            //ViewModelLocationProvider.SetDefaultViewModelFactory((type) =>
            //{
            //    return Container.Resolve(type);
            //});

            Container.RegisterType<ISkeletonGameProvider, SkeletonGameProvider>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IGameRunnner, SkeletonGameRunner>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IVpScriptExporter, VpScriptExporter>(new ContainerControlledLifetimeManager());            

            Container.RegisterInstance<ISkeletonLogger>(Container.Resolve<SkeletonLogger>());
            Container.RegisterInstance<ISkeletonOSC>(Container.Resolve<SkeletonOSC>());
            Container.RegisterInstance<ISkeletonGameSerializer>(Container.Resolve<SkeletonGameSerializer>());
            Container.RegisterInstance<ISkeletonGameFiles>(Container.Resolve<SkeletonGameFiles>());
            Container.RegisterInstance<ISkeletonGameExport>(Container.Resolve<SkeletonGameExport>());

            Container.RegisterType<RecordingsViewModel>(new ContainerControlledLifetimeManager());
            //Container.RegisterInstance(Container.Resolve<ScoreLayoutViewModel>());
            //Container.RegisterInstance(Container.Resolve<TrophyDataViewModel>());
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
        }

        protected override ILoggerFacade CreateLogger()
        {
            return new SkeletonGameLogger("Sgm_Logger");
        }
    }
}
