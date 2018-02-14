using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.Practices.Unity;
using Prism.Events;
using SkeletonGame.Models.Machine;
using SkeletonGameManager.WPF.Events;
using SkeletonGameManager.WPF.Providers;
using SkeletonGameManager.WPF.ViewModels.Config;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class KeyboardMappingsViewModel : SkeletonGameManagerViewModelBase
    {
        private ISkeletonGameProvider _skeletonGameProvider;
        private IUnityContainer _unityContainer;

        private IEnumerable<PRSwitch> _availableSwitches;

        public KeyboardMappingsViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider) : base(eventAggregator)
        {
            _skeletonGameProvider = skeletonGameProvider;

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x => await OnLoadYamlFilesChanged());
        }        

        private ObservableCollection<KeyboardMapItemViewModel> switchKeys = new ObservableCollection<KeyboardMapItemViewModel>();        
        public ObservableCollection<KeyboardMapItemViewModel> SwitchKeys
        {
            get { return switchKeys; }
            set { SetProperty(ref switchKeys, value); }
        }

        /// <summary>
        /// Called when [load yaml files changed]. Updates the switches from file.
        /// </summary>
        /// <returns></returns>
        public async override Task OnLoadYamlFilesChanged()
        {
            SwitchKeys?.Clear();

            //Order switches and create new view models for the collection
            var orderedSwitch = _skeletonGameProvider.GameConfig.KeyboardSwitchMap.OrderBy(x => x.Value);
            foreach (var keySwitch in orderedSwitch)
            {
                await Dispatcher.CurrentDispatcher.InvokeAsync(() => SwitchKeys.Add(new KeyboardMapItemViewModel(keySwitch)));
            }

            _availableSwitches = _skeletonGameProvider.MachineConfig.PRSwitches.Where(x => x.Name != "NOT USED");
        }
    }
}
