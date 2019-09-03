using Microsoft.Practices.Unity;
using Prism.Unity;
using System.Windows;
using Prism.Modularity;
using SkeletonGame.Engine;
using SkeletonGameManager.Base;
using SkeletonGameManager.Module.Services;
using Prism.Logging;
using SGM2.Wpf.Views;
using SGM2.Wpf.ViewModels;
using System.Configuration;

namespace SGM2.Wpf
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell() => Container.Resolve<MainWindow>();

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            var moduleCatalog = (ModuleCatalog)ModuleCatalog;
            moduleCatalog.AddModule(typeof(SkeletonGameManager.Module.Assets.AssetsModule));
            moduleCatalog.AddModule(typeof(SkeletonGameManager.Module.Config.ConfigModule));
            moduleCatalog.AddModule(typeof(SkeletonGameManager.Module.Recordings.RecordingsModule));
            moduleCatalog.AddModule(typeof(SkeletonGameManager.Module.SceneGrab.SceneGrabModule));
            moduleCatalog.AddModule(typeof(SkeletonGameManager.Module.SceneManage.SceneManageModule));
            moduleCatalog.AddModule(typeof(SkeletonGameManager.Module.Menus.MenusModule));
            moduleCatalog.AddModule(typeof(SkeletonGameManager.Module.LogViewer.LogViewerModule));
            moduleCatalog.Initialize();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<ISkeletonGameProvider, SkeletonGameProvider>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IGameRunnner, SkeletonGameRunner>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IVpScriptExporter, VpScriptExporter>(new ContainerControlledLifetimeManager());

            Container.RegisterInstance<ISkeletonLogger>(Container.Resolve<SkeletonLogger>());
            Container.RegisterInstance<ISkeletonOSC>(Container.Resolve<SkeletonOSC>());
            Container.RegisterInstance<ISkeletonGameSerializer>(Container.Resolve<SkeletonGameSerializer>());
            Container.RegisterInstance<ISkeletonGameFiles>(Container.Resolve<SkeletonGameFiles>());
            Container.RegisterInstance<ISkeletonGameExport>(Container.Resolve<SkeletonGameExport>());
            
            Container.RegisterInstance<IVpGameMapper>(Container.Resolve<VpGameMapper>());
            var vppath = ConfigurationManager.AppSettings["visualPinball"];
            Container.RegisterInstance<IVisualPinball>(new VpLaunch(vppath));

            //Register views for View discovery            
            Container.RegisterType<object, ScoreLayoutView>("ScoreLayoutView");
            Container.RegisterType<object, TrophyDataView>("TrophyDataView");

            Container.RegisterInstance(Container.Resolve<ScoreLayoutViewModel>(), new ContainerControlledLifetimeManager());

            Container.RegisterInstance(Container.Resolve<TrophyDataViewModel>(), new ContainerControlledLifetimeManager());
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
