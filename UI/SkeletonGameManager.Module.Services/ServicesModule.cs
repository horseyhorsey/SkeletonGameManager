
using Prism.Modularity;
using Microsoft.Practices.Unity;

namespace SkeletonGameManager.Module.Services
{
    public class ServicesModule : IModule
    {
        private IUnityContainer _container;

        public ServicesModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            
        }
    }
}