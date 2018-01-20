using System.Threading.Tasks;
using Prism.Events;
using SkeletonGameManager.WPF.Providers;
using SkeletonGame.Models.Machine;
using System.Windows.Threading;
using SkeletonGameManager.WPF.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SkeletonGameManager.WPF.ViewModels.Machine;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class MachineConfigViewModel : SkeletonGameManagerViewModelBase
    {
        private ISkeletonGameProvider _skeletonGameProvider;

        public MachineConfigViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider) : base(eventAggregator)
        {
            _skeletonGameProvider = skeletonGameProvider;

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x => await OnLoadYamlFilesChanged());

            InitializeCollections();
        }        

        private ObservableCollection<SwitchViewModel> switches;
        public ObservableCollection<SwitchViewModel> Switches
        {
            get { return switches; }
            set { SetProperty(ref switches, value); }
        }

        private ObservableCollection<PRSwitch> dSwitches;
        public ObservableCollection<PRSwitch> DedicatedSwitches
        {
            get { return dSwitches; }
            set { SetProperty(ref dSwitches, value); }
        }

        private ObservableCollection<PRSwitch> fSwitches;
        public ObservableCollection<PRSwitch> FlippersSwitches
        {
            get { return fSwitches; }
            set { SetProperty(ref fSwitches, value); }
        }

        private ObservableCollection<PRLamp> lamps;
        public ObservableCollection<PRLamp> Lamps
        {
            get { return lamps; }
            set { SetProperty(ref lamps, value); }
        }

        #region Properties
        private MachineConfig machineConfigModel;
        public MachineConfig MachineConfig
        {
            get { return machineConfigModel; }
            set { SetProperty(ref machineConfigModel, value); }
        }
        #endregion

        #region Public Methods
        public async override Task OnLoadYamlFilesChanged()
        {
            MachineConfig = _skeletonGameProvider.MachineConfig;

            foreach (var prSwitch in MachineConfig.PRSwitches)
            {
                if (prSwitch.Number.Contains("D"))
                    DedicatedSwitches.Add(prSwitch);
                else if (prSwitch.Number.Contains("F"))
                    FlippersSwitches.Add(prSwitch);
                else
                {
                    var sw = Switches.First(x => x.Number == prSwitch.Number);
                    if (sw !=null)
                    {
                        sw.Name = prSwitch.Name;
                        sw.Tags = prSwitch.Tags;
                        sw.Type = prSwitch.SwitchType;

                        if (prSwitch.BallSearch != null)
                        {
                            if (prSwitch.BallSearch.Contains("reset"))
                                sw.Reset = true;

                            if (prSwitch.BallSearch.Contains("stop"))
                                sw.Stop = true;
                        }                        
                        
                    }                        
                }                    
            }

            await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
            {
                //SaveCommand.RaiseCanExecuteChanged();
            });
        } 
        #endregion

        #region Private Methods
        /// <summary>
        /// Initializes the collections for Matrix
        /// </summary>
        private void InitializeCollections()
        {
            Switches = new ObservableCollection<SwitchViewModel>();
            Lamps = new ObservableCollection<PRLamp>();

            DedicatedSwitches = new ObservableCollection<PRSwitch>();
            FlippersSwitches = new ObservableCollection<PRSwitch>();

            for (int i = 11; i < 99; i++)
            {
                var numStr = i.ToString();
                if (!numStr.Contains("9")) { 
                    if (!numStr.Contains("0"))
                    {
                        AddToMatrix(numStr);
                    }
                }
                else if (i > 90)
                    AddToMatrix(numStr);
            }
        }

        private void AddToMatrix(string numStr)
        {
            Switches.Add(new SwitchViewModel() { Number = $"S{numStr}", Name = "NOT USED" });
            Lamps.Add(new PRLamp() { Number = $"L{numStr}", Name = "NOT USED" });
        }
        #endregion
    }
}
