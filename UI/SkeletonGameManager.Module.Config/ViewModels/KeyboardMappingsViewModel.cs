using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Prism.Commands;
using Prism.Events;
using SkeletonGame.Models.Machine;
using SkeletonGameManager.Base;
using static SkeletonGame.Models.SdlKeyCode;
using static SkeletonGameManager.Base.Events;

namespace SkeletonGameManager.Module.Config.ViewModels
{
    public class KeyboardMappingsViewModel : SkeletonGameManagerViewModelBase
    {
        private ISkeletonGameProvider _skeletonGameProvider;

        #region Commands
        public DelegateCommand<KeyEventArgs> KeyDownCommand { get; set; }
        public DelegateCommand<PRSwitch> AddUnusedSwitchCommand { get; set; }
        #endregion

        #region Constructors
        public KeyboardMappingsViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider) : base(eventAggregator)
        {
            _skeletonGameProvider = skeletonGameProvider;            

            AddUnusedSwitchCommand = new DelegateCommand<PRSwitch>(AddToUnusedSwitch);

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x => await OnLoadYamlFilesChanged());
        } 
        #endregion

        #region Properties
        private ObservableCollection<KeyboardMapItemViewModel> switchKeys = new ObservableCollection<KeyboardMapItemViewModel>();
        public ObservableCollection<KeyboardMapItemViewModel> SwitchKeys
        {
            get { return switchKeys; }
            set { SetProperty(ref switchKeys, value); }
        }

        private SDL_Keycode capturedSdlCode;
        public SDL_Keycode CapturedSdlCode
        {
            get { return capturedSdlCode; }
            set { SetProperty(ref capturedSdlCode, value); }
        }

        private ObservableCollection<PRSwitch> _availableSwitches;
        public ObservableCollection<PRSwitch> AvailableSwitches
        {
            get { return _availableSwitches; }
            set { SetProperty(ref _availableSwitches, value); }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Called when [load yaml files changed]. Updates the switches from file.
        /// </summary>
        /// <returns></returns>
        public async override Task OnLoadYamlFilesChanged()
        {
            SwitchKeys?.Clear();

            AvailableSwitches =
                new ObservableCollection<PRSwitch>(_skeletonGameProvider
                    .MachineConfig.PRSwitches.Where(x => x.Name != "NOT USED").ToList());

            //Order switches and create new view models for the collection
            var orderedSwitch = _skeletonGameProvider.GameConfig.KeyboardSwitchMap.OrderBy(x => x.Value);

            foreach (var keySwitch in orderedSwitch)
            {
                var availableSwitch = AvailableSwitches.FirstOrDefault(x => x.Number == keySwitch.Value);

                //keySwitch.Key = item.Key.Replace("\\b", "\b")
                //.Replace("\\r", "\r")
                //.Replace("\\t", "\t");

                if (availableSwitch != null)
                {
                    await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                    SwitchKeys.Add(new KeyboardMapItemViewModel(availableSwitch.Name, keySwitch)));

                    AvailableSwitches.Remove(availableSwitch);
                }
            }
        }
        #endregion

        #region Private Methods
        private void AddToUnusedSwitch(PRSwitch x)
        {
            if (x == null) return;

            char sdlKeyCodeChar = (char)CapturedSdlCode;
            string charString = sdlKeyCodeChar.ToString();

            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                SwitchKeys.Add(new KeyboardMapItemViewModel
                {
                    Keycode = CapturedSdlCode,
                    Name = x.Name,
                    Number = x.Number,
                    Key = sdlKeyCodeChar.ToString()
                });
            });

            AvailableSwitches.Remove(x);
        } 
        #endregion

    }
}
