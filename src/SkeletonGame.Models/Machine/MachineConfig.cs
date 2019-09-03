using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Machine
{    
    public class GameItemBase
    {
        [YamlMember(Alias = "name", ApplyNamingConventions = false, Order = 0)]
        public string Name { get; set; }

        private string _number;
        [YamlMember(Alias = "number", ApplyNamingConventions = false, Order = 1 )]
        public string Number { get { return _number; } set { _number = value.ToUpper(); } }

        [YamlMember(Alias = "tags", ApplyNamingConventions = false, Order = 2, SerializeAs = typeof(string))]
        public string Tags { get; set; } = string.Empty;

        [YamlMember(Alias = "label", ApplyNamingConventions = false, Order = 3)]
        public string Label { get; set; }
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class PRCoil : GameItemBase
    {

        [YamlMember(Alias = "pulseTime", ApplyNamingConventions = false)]
        public byte? PulseTime { get; set; }

        [YamlMember(Alias = "ballsearch", ApplyNamingConventions = false)]
        public bool BallSearch { get; set; }

        [YamlMember(Alias = "patterOnTime", ApplyNamingConventions = false)]
        public byte? PatterOnTime { get; set; }

        [YamlMember(Alias = "patterOffTime", ApplyNamingConventions = false)]
        public byte? PatterOffTime { get; set; }

        [YamlMember(Alias = "solenoid_type", ApplyNamingConventions = false)]
        public SolenoidType? SolenoidType { get; set; }
    }

    public class PRLampBase : GameItemBase
    {
        [YamlMember(Alias = "polarity", ApplyNamingConventions = false, ScalarStyle = YamlDotNet.Core.ScalarStyle.Plain, SerializeAs = typeof(string))]
        public bool Polarity { get; set; } = false;
    }

    public class PRLamp :  PRLampBase
    {
        
    }

    public class PRLed : PRLampBase
    {

    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class PRSwitch : GameItemBase
    {
        [YamlMember(Alias = "ballsearch", ApplyNamingConventions = false)]
        public string BallSearch { get; set; }

        [YamlMember(Alias = "type", ApplyNamingConventions = false, SerializeAs = typeof(string), ScalarStyle = YamlDotNet.Core.ScalarStyle.SingleQuoted)]
        public ProcSwitchType? SwitchType { get; set; }

        [YamlMember(Alias = "vp_switch_type", ApplyNamingConventions = false)]
        public VpSwitchType? VpSwitchType { get; set; }
    }

    public class PRGame
    {
        [YamlMember(Alias = "machineType", ApplyNamingConventions = false)]
        public string MachineType { get; set; }

        [YamlMember(Alias = "numBalls", ApplyNamingConventions = false)]
        public int NumBalls { get; set; }
    }   

    public class MachineConfig
    {        
        #region Properties
        [YamlMember(Alias = "PRGame", ApplyNamingConventions = false)]
        public PRGame PRGame { get; set; }

        [YamlMember(Alias = "PRBumpers", ApplyNamingConventions = false)]
        public List<string> PRBumpers { get; set; }

        [YamlMember(Alias = "PRFlippers", ApplyNamingConventions = false)]
        public List<string> PRFlippers { get; set; }

        [YamlMember(Alias = "PRSwitches", ApplyNamingConventions = false)]
        public List<PRSwitch> PRSwitches { get; set; }

        [YamlMember(Alias = "PRLamps", ApplyNamingConventions = false)]
        public List<PRLamp> PRLamps { get; set; } = new List<PRLamp>();

        [YamlMember(Alias = "PRCoils", ApplyNamingConventions = false)]
        public List<PRCoil> PRCoils { get; set; }

        [YamlMember(Alias = "PRLEDs", ApplyNamingConventions = false)]
        public List<PRLed> PRLeds { get; set; }

        [YamlMember(Alias = "PRDriverGlobals", ApplyNamingConventions = false)]
        public PRDriverGlobal PRDriverGlobals { get; set; }

        #endregion

        /// <summary>
        /// Gets the type of the configuration from machine types. wpc, stern or pdb
        /// </summary>
        /// <returns></returns>
        public string GetConfigType()
        {
            var machineType = GetMachineType();
            switch (machineType)
            {
                case MachineType.WPC:
                case MachineType.WPC95:
                case MachineType.WPCALPHANUMERIC:
                    return "wpc";
                case MachineType.STERNSAM:
                case MachineType.STERNWHITESTAR:
                    return "stern";
                case MachineType.PDB:
                    return "pdb";
            }

            return string.Empty;            
        }

        public MachineType GetMachineType()
        {
            return (MachineType)Enum.Parse(typeof(MachineType), PRGame?.MachineType.ToUpper());
        }

        #region Private Methods

        #endregion
    }


    public class PRDriverGlobal
    {
        [YamlMember(Alias = "lamp_matrix_strobe_time", Order = 0, ApplyNamingConventions = false)]
        public int MatrixStrobeTime { get; set; } = 200;

        [YamlMember(Alias = "watchdog_time", Order = 0, ApplyNamingConventions = false)]
        public int WatchdogTime { get; set; } = 1000;

        [YamlMember(Alias = "use_watchdog", Order = 0, ApplyNamingConventions = false)]
        public bool WatchdogEnabled { get; set; } = true;
    }

    public class MachineConfigDict
    {
        [YamlMember(Alias = "PRGame", Order = 0, ApplyNamingConventions = false)]
        public PRGame PRGame { get; set; }

        [YamlMember(Alias = "PRBumpers", ApplyNamingConventions = false)]
        public List<string> PRBumpers { get; set; }

        [YamlMember(Alias = "PRFlippers", ApplyNamingConventions = false)]
        public List<string> PRFlippers { get; set; }

        [YamlMember(Alias = "PRSwitches", Order = 3, ApplyNamingConventions = false)]
        public Dictionary<string, PRSwitch> PRSwitches { get; set; }

        [YamlMember(Alias = "PRLamps", Order = 4, ApplyNamingConventions = false)]
        public Dictionary<string, PRLamp> PRLamps { get; set; }

        [YamlMember(Alias = "PRCoils", Order = 5, ApplyNamingConventions = false)]        
        public Dictionary<string, PRCoil> PRCoils { get; set; }

        [YamlMember(Alias = "PRLEDs", Order = 6, ApplyNamingConventions = false)]
        public Dictionary<string, PRLed> PrLeds { get; set; }

    }
}
