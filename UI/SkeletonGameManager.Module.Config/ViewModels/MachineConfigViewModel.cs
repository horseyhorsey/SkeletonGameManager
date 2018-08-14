using System.Threading.Tasks;
using Prism.Events;
using SkeletonGame.Models.Machine;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using System;
using SkeletonGame.Engine;
using SkeletonGameManager.Base;
using static SkeletonGameManager.Base.Events;
using SkeletonGameManager.Module.Config.ViewModels.Machine;
using Prism.Logging;

namespace SkeletonGameManager.Module.Config.ViewModels
{
    public class MachineConfigViewModel : SkeletonTabViewModel
    {
        #region Fields
        private ISkeletonGameProvider _skeletonGameProvider;
        private ISkeletonOSC _skeletonOSC;
        private ISkeletonGameExport _skeletonExport;
        #endregion

        #region Commands
        public ICommand SaveMachineConfigCommand { get; set; }
        public ICommand SendOscMessageCommand { get; set; } 
        #endregion

        #region Constructors
        public MachineConfigViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider, 
            ISkeletonOSC skeletonOSC, ISkeletonGameExport skeletonGameExport, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
            Title = "Machine";

            _skeletonGameProvider = skeletonGameProvider;
            _skeletonOSC = skeletonOSC;
            _skeletonExport = skeletonGameExport;            

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x => await OnLoadYamlFilesChanged());

            SaveMachineConfigCommand = new DelegateCommand(() =>
            {
                try
                {
                    SaveMachineConfig();
                }
                catch (Exception ex)
                {
                    var msg = $"Error saving machine configuration. {ex.Message}";
                    Log(msg, Category.Exception);
                    _eventAggregator.GetEvent<ErrorMessageEvent>().Publish(msg);
                }

            });

            SendOscMessageCommand = new DelegateCommand<object>((x) =>
            {
                try
                {
                    var obj = x as SwitchViewModel;

                    SwitchViewModel pushedSwitch = null;
                    if (obj.Number.Contains("D"))
                        pushedSwitch = this.DedicatedSwitches.First((c) => c == obj);
                    else
                        pushedSwitch = this.Switches.First((c) => c == obj);

                    pushedSwitch.State = !pushedSwitch.State;

                    var value = 0.0f;
                    if (pushedSwitch.State)
                        value = 1.0f;

                    _skeletonOSC.Send($@"/sw/{pushedSwitch.Name}", value);
                }
                catch { }
            });

            InitializeCollections();
        }

        #endregion

        #region Properties

        private ObservableCollection<SolenoidFlasherViewModel> coils;
        public ObservableCollection<SolenoidFlasherViewModel> Coils
        {
            get { return coils; }
            set { SetProperty(ref coils, value); }
        }

        private ObservableCollection<SwitchViewModel> dSwitches;
        public ObservableCollection<SwitchViewModel> DedicatedSwitches
        {
            get { return dSwitches; }
            set { SetProperty(ref dSwitches, value); }
        }

        private ObservableCollection<SwitchViewModel> fSwitches;
        public ObservableCollection<SwitchViewModel> FlippersSwitches
        {
            get { return fSwitches; }
            set { SetProperty(ref fSwitches, value); }
        }

        private ObservableCollection<LampViewModel> lamps;
        public ObservableCollection<LampViewModel> Lamps
        {
            get { return lamps; }
            set { SetProperty(ref lamps, value); }
        }

        private MachineConfig machineConfigModel;
        public MachineConfig MachineConfig
        {
            get { return machineConfigModel; }
            set { SetProperty(ref machineConfigModel, value); }
        }

        private ObservableCollection<SwitchViewModel> switches;
        public ObservableCollection<SwitchViewModel> Switches
        {
            get { return switches; }
            set { SetProperty(ref switches, value); }
        }

        private ObservableCollection<LampViewModel> _pRLeds;
        public ObservableCollection<LampViewModel> PRLeds
        {
            get { return _pRLeds; }
            set { SetProperty(ref _pRLeds, value); }
        }
        #endregion

        #region Public Methods        

        /// <summary>
        /// Called when [load yaml files changed]. Adds any items found in the machines yaml file.
        /// </summary>
        /// <returns></returns>
        public async override Task OnLoadYamlFilesChanged()
        {
            Log("Clearing machine configs");
            this.Lamps.Clear();
            this.Switches.Clear();
            this.Coils.Clear();
            this.DedicatedSwitches.Clear();
            this.PRLeds.Clear();

            Log("Loading machine configuration");
            MachineConfig = _skeletonGameProvider.MachineConfig;
            if (MachineConfig != null)
            {
                Log("Creating factory");
                MachineType type = (MachineType)Enum.Parse(typeof(MachineType), MachineConfig.PRGame.MachineType.ToUpper());
                IPinballFactoryBuilder pinballFactoryBuilder = new PinballFactoryBuilder(MachineConfig);
                IPinballFactory pinFactory = pinballFactoryBuilder.Build(type);
                Log($"Factory created. {pinFactory.GetType().Name}");

                await pinFactory.PopConfig();

                UpdateMachineConfigCollections();
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the collections for Matrixs and tables
        /// </summary>
        private void InitializeCollections()
        {
            Log("Initializing Machine Config collections");
            Switches = new ObservableCollection<SwitchViewModel>();
            Lamps = new ObservableCollection<LampViewModel>();
            Coils = new ObservableCollection<SolenoidFlasherViewModel>();
            DedicatedSwitches = new ObservableCollection<SwitchViewModel>();
            FlippersSwitches = new ObservableCollection<SwitchViewModel>();
            PRLeds = new ObservableCollection<LampViewModel>();
        }

        private void SaveMachineConfig()
        {
            var mConfig = MachineConfig;
            Log("Saving Machine configuration");

            Log("Adding Flipper switches");
            mConfig.PRSwitches.Clear();
            mConfig.PRSwitches
                .AddRange(FlippersSwitches.Select(x => new PRSwitch()
                {
                    Name = x.Name,
                    Number = x.Number,
                    Tags = x.Tags,
                    SwitchType = x.Type,
                    Label = x.Label,
                    VpSwitchType = x.VpSwitchType
                }));

            Log("Adding Dedicated switches");
            // Save dedicated switches 
            foreach (var item in DedicatedSwitches)
            {
                if (item.Name != "NOT USED")
                {
                    var sw = new PRSwitch()
                    {
                        Name = item.Name,
                        Number = item.Number,
                        Tags = item.Tags,
                        SwitchType = item.Type,
                        Label = item.Label

                    };

                    mConfig.PRSwitches.Add(sw);
                }
            }

            //Save switches
            Log("Adding switches");
            foreach (var item in Switches)
            {
                if (item.Name != "NOT USED")
                {
                    var sw = new PRSwitch()
                    {
                        Name = item.Name,
                        Number = item.Number.Split(':')[1],
                        Tags = item.Tags,
                        SwitchType = item.Type,
                        Label = item.Label,
                        VpSwitchType = item.VpSwitchType
                    };

                    if (item.BallSearch.Any(x => x != null))
                        sw.BallSearch = $"{item.BallSearch[0]}, {item.BallSearch[1]}";

                    mConfig.PRSwitches.Add(sw);
                }
            }

            //Save Lamps
            Log("Adding Lamps");
            mConfig.PRLamps?.Clear();

            if (mConfig.PRLamps == null)
                Log("No PRLEDs found in config.", Category.Warn);
            else
            {
                foreach (var item in Lamps)
                {
                    if (item.Name != "NOT USED")
                    {
                        var lamp = new PRLamp()
                        {
                            Name = item.Name,
                            Number = item.Number,
                            Tags = item.Tags,
                            Label = item.Label
                        };

                        MachineConfig.PRLamps.Add(lamp);
                    }
                }
            }            

            //Save LEds
            Log("Adding LEDs");
            if (mConfig.PRLeds == null)
                Log("No PRLEDs found in config.", Category.Warn);
            else
            {
                mConfig.PRLeds.Clear();
                foreach (var item in PRLeds)
                {
                    if (item.Name != "NOT USED")
                    {
                        var lamp = new PRLed()
                        {
                            Name = item.Name,
                            Number = item.Number,
                            Tags = item.Tags,
                            Label = item.Label
                        };

                        mConfig.PRLeds.Add(lamp);
                    }
                }
            }

            mConfig.PRCoils.Clear();
            mConfig.PRFlippers.Clear();

            Log("Adding Coils");
            foreach (var coil in Coils)
            {
                if (coil.Name != "NOT USED")
                {
                    var newcoil = new PRCoil()
                    {
                        Name = coil.Name,
                        Number = coil.Number,
                        Tags = coil.Tags,
                        PatterOffTime = coil.PatterOffTime,
                        PatterOnTime = coil.PatterOnTime,
                        BallSearch = coil.BallSearch,
                        PulseTime = coil.PulseTime,
                        SolenoidType = coil.SolenoidType,
                        Label = coil.Label
                    };

                    mConfig.PRCoils.Add(newcoil);

                    //Add PRFlippers list from the coil name once
                    if (coil.Name.Contains("Main"))
                    {
                        mConfig.PRFlippers.Add(newcoil.Name.Replace("Main", string.Empty));
                    }
                }
            }

            Log("Attempting Save...");
            _skeletonGameProvider.SaveMachineConfig(mConfig);
        }

        /// <summary>
        /// Updates the machine configuration collections. Switches, lamps, coils etc.
        /// </summary>
        private void UpdateMachineConfigCollections()
        {
            Log("Updating UI Collections");

            //Add only the switches not containing dedicated and flipper
            if (MachineConfig.PRSwitches != null)
            {
                Log("Adding switches");
                Switches.AddRange(MachineConfig.PRSwitches
                    .Where(x => !x.Number.Contains("F") & !x.Number.Contains("SD"))
                    .Select(x => new SwitchViewModel(x)).OrderBy(x => x.Number));

                Log("Adding dedicated switches");
                DedicatedSwitches.AddRange(MachineConfig.PRSwitches
                    .Where(x => x.Number.Contains("SD"))
                    .Select(x => new SwitchViewModel(x)).OrderBy(x => x.Number));

                Log("Adding flipper switches");
                FlippersSwitches.AddRange(MachineConfig.PRSwitches
                    .Where(x => x.Number.Contains("SF"))
                    .Select(x => new SwitchViewModel(x)).OrderBy(x => x.Number));
            }                
            else
                Log("PRSwitches doesn't exist");

            if (MachineConfig.PRCoils != null)
                Coils.AddRange(MachineConfig.PRCoils.Select(x => new SolenoidFlasherViewModel(x)).OrderBy(x => x.Number));
            else
                Log("PRCoils doesn't exist");

            if (MachineConfig.PRLamps != null)
                Lamps.AddRange(MachineConfig.PRLamps.Select(x => new LampViewModel(x)).OrderBy(x => x.Number));
            else
                Log("PRLamps doesn't exist");

            if (MachineConfig.PRLeds != null)
                PRLeds.AddRange(MachineConfig.PRLeds.Select(x => new LampViewModel(x)));//.OrderBy(x => x.Number.Replace("A8-", "")));
            else
                Log("PRLeds doesn't exist");
        }
        #endregion
    }
}
