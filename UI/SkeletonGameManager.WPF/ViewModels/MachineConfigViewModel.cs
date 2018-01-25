﻿using System.Threading.Tasks;
using Prism.Events;
using SkeletonGameManager.WPF.Providers;
using SkeletonGame.Models.Machine;
using System.Windows.Threading;
using SkeletonGameManager.WPF.Events;
using System.Collections.ObjectModel;
using System.Linq;
using SkeletonGameManager.WPF.ViewModels.Machine;
using System.Windows.Input;
using Prism.Commands;
using System;
using System.Reflection;
using System.ComponentModel;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class MachineConfigViewModel : SkeletonGameManagerViewModelBase
    {
        private ISkeletonGameProvider _skeletonGameProvider;

        public ICommand SaveMachineConfigCommand { get; set; }

        #region Constructors
        public MachineConfigViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider) : base(eventAggregator)
        {
            _skeletonGameProvider = skeletonGameProvider;

            InitializeCollections();

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x => await OnLoadYamlFilesChanged());

            SaveMachineConfigCommand = new DelegateCommand(() =>
            {
                SaveMachineConfig();
            });
        }
        #endregion

        #region Properties

        private ObservableCollection<SolenoidFlasherViewModel> coils;
        public ObservableCollection<SolenoidFlasherViewModel> Coils
        {
            get { return coils; }
            set { SetProperty(ref coils, value); }
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
        #endregion

        #region Public Methods

        public string DescriptionAttr<T>(T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }

        /// <summary>
        /// Called when [load yaml files changed]. Adds any items found in the machines yaml file.
        /// </summary>
        /// <returns></returns>
        public async override Task OnLoadYamlFilesChanged()
        {

            MachineConfig = _skeletonGameProvider.MachineConfig;

            if (MachineConfig != null)
            {
                MachineType type = (MachineType)Enum.Parse(typeof(MachineType), MachineConfig.PRGame.MachineType.ToUpper());

                CreateSwitchesAndLamps(type);
                CreateCoils(type);

                //Lamps
                foreach (var prLamp in MachineConfig.PRLamps)
                {
                    var lamp = Lamps.FirstOrDefault(x => x.Number == prLamp.Number);
                    if (lamp != null)
                    {
                        //lamp.Number = prLamp.Number;
                        lamp.Name = prLamp.Name;
                        lamp.Tags = prLamp.Tags;
                    }
                }

                //Switches
                foreach (var prSwitch in MachineConfig.PRSwitches)
                {
                    if (prSwitch.Number.Contains("D"))
                        DedicatedSwitches.Add(prSwitch);
                    else if (prSwitch.Number.Contains("F"))
                        FlippersSwitches.Add(prSwitch);
                    else
                    {
                        var sw = Switches.FirstOrDefault(x => x.Number == prSwitch.Number);
                        if (sw != null)
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

                foreach (var prCoil in MachineConfig.PRCoils)
                {
                    var coil = Coils.FirstOrDefault(x => x.Number == prCoil.Number);
                    if (coil != null)
                    {
                        coil.Name = prCoil.Name;
                        coil.Number = prCoil.Number;
                        coil.PatterOffTime = prCoil.PatterOffTime;
                        coil.PatterOnTime = prCoil.PatterOnTime;
                        coil.PulseTime = prCoil.PulseTime;
                        coil.SolenoidType = prCoil.SolenoidType ?? 0;
                        coil.BallSearch = prCoil.BallSearch;
                    }
                }

                await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                {
                    //SaveCommand.RaiseCanExecuteChanged();
                });
            }
        }
        #endregion

        #region Private Methods
        private void AddToMatrix(string numStr, int num)
        {
            Switches.Add(new SwitchViewModel() { Number = $"S{numStr}", Name = "NOT USED" });

            if (num > 90 && num < 96)
            {
                Lamps.Add(new LampViewModel() { Number = $"G0{numStr.Substring(1, 1)}", Name = "NOT USED" });
            }
            else if (num < 90)
            {
                Lamps.Add(new LampViewModel() { Number = $"L{numStr}", Name = "NOT USED" });
            }
        }

        /// <summary>
        /// Adds to a new <see cref="LampViewModel"/> to collection for matrix
        /// </summary>
        /// <param name="i">The i.</param>
        private void AddToLampMatrix(int i)
        {
            string numString = string.Empty;

            numString = (i < 10) ? $"0{i}" : $"{i}";

            Lamps.Add(new LampViewModel() { Number = $"L{numString}", Name = "NOT USED" });
        }

        private void AddToSwitchMatrix(int i)
        {
            string numString = string.Empty;

            numString = (i < 10) ? $"0{i}" : $"{i}";

            Switches.Add(new SwitchViewModel() { Number = $"S{numString}", Name = "NOT USED" });
        }

        private void CreateCoils(MachineType type)
        {
            if (type == MachineType.WPC || type == MachineType.WPC95 || type == MachineType.WPCALPHANUMERIC)
            {
                for (int i = 1; i < 29; i++)
                {
                    string num = i.ToString();
                    if (i < 10) { num = "0" + i; }
                    Coils.Add(new SolenoidFlasherViewModel
                    {
                        Name = "NOT USED",
                        Number = $"C{num}",
                        SolenoidType = 0
                    });
                }

                //Add flipper circuits
                foreach (var flipCoil in Enum.GetNames(typeof(FlipperCoils)))
                {
                    var t = Enum.Parse(typeof(FlipperCoils), flipCoil);
                    var desc = DescriptionAttr<FlipperCoils>((FlipperCoils)t);
                    //var enabled = desc.Contains("flipperLw") ? true : false;
                    if (desc.Contains("Up"))
                        desc = "NOT USED";

                    Coils.Add(new SolenoidFlasherViewModel
                    {
                        Name = desc,
                        Number = flipCoil,
                    });
                }

                for (int i = 37; i < 44; i++)
                {
                    Coils.Add(new SolenoidFlasherViewModel
                    {
                        Name = "NOT USED",
                        Number = $"C{i}",
                        SolenoidType = 0
                    });
                }

            }
            else if(type == MachineType.STERNSAM)
            {
                for (int i = 1; i < 33; i++)
                {
                    var numString = string.Empty;
                    numString = (i < 10) ?  $"C0{i}" : $"C{i}";

                    Coils.Add(new SolenoidFlasherViewModel
                    {
                        Name = "NOT USED",
                        Number = numString,
                    });
                }
            }
        }

        /// <summary>
        /// Creates switches based on machine type. TODO: Other machine types like stern and PDB
        /// </summary>
        /// <param name="machineType">Type of the machine.</param>
        private void CreateSwitchesAndLamps(MachineType type)
        {
            this.Lamps.Clear();
            this.Switches.Clear();
            this.Coils.Clear();

            if (type == MachineType.WPC || type == MachineType.WPC95 || type == MachineType.WPCALPHANUMERIC)
            {
                //Add switches and lamps
                for (int i = 11; i < 99; i++)
                {
                    var numStr = i.ToString();
                    if (!numStr.Contains("9"))
                    {
                        if (!numStr.Contains("0"))
                        {
                            AddToMatrix(numStr, i);
                        }
                    }
                    else if (i > 90)
                        AddToMatrix(numStr, i);
                }
            }
            else if (type == MachineType.STERNSAM)
            {
                for (int i = 1; i < 81; i++)
                {
                    if (i < 65)
                        AddToSwitchMatrix(i);

                    AddToLampMatrix(i);
                }
            }
        }

        /// <summary>
        /// Initializes the collections for Matrixs and tables
        /// </summary>
        private void InitializeCollections()
        {
            Switches = new ObservableCollection<SwitchViewModel>();
            Lamps = new ObservableCollection<LampViewModel>();
            Coils = new ObservableCollection<SolenoidFlasherViewModel>();

            DedicatedSwitches = new ObservableCollection<PRSwitch>();
            FlippersSwitches = new ObservableCollection<PRSwitch>();
        }

        private void SaveMachineConfig()
        {
            var mConfig = MachineConfig;
            mConfig.PRSwitches.Clear();
            mConfig.PRSwitches.AddRange(this.FlippersSwitches);
            mConfig.PRSwitches.AddRange(this.DedicatedSwitches);

            //Save switches
            foreach (var item in Switches)
            {
                if (item.Name != "NOT USED")
                {
                    var sw = new PRSwitch()
                    {
                        Name = item.Name,
                        Number = item.Number,
                        Tags = item.Tags,
                        SwitchType = item.Type
                    };

                    if (item.BallSearch.Any(x => x != null))
                        sw.BallSearch = $"{item.BallSearch[0]}, {item.BallSearch[1]}";

                    MachineConfig.PRSwitches.Add(sw);
                }
            }

            //Save Lamps
            mConfig.PRLamps.Clear();
            foreach (var item in Lamps)
            {
                if (item.Name != "NOT USED")
                {
                    var lamp = new PRLamp()
                    {
                        Name = item.Name,
                        Number = item.Number,
                        Tags = item.Tags,
                    };

                    MachineConfig.PRLamps.Add(lamp);
                }
            }

            mConfig.PRCoils.Clear();
            mConfig.PRFlippers.Clear();

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
                        SolenoidType = coil.SolenoidType
                    };

                    //Add to the PRFlippers list
                    if (newcoil.Name.Contains("Main"))
                    {
                        mConfig.PRFlippers.Add(newcoil.Name.Replace("Main", string.Empty));
                    }

                    MachineConfig.PRCoils.Add(newcoil);
                }
            }

            _skeletonGameProvider.SaveMachineConfig(mConfig);


        }
        #endregion
    }
}
