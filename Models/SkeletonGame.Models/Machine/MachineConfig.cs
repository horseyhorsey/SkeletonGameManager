using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Machine
{
    public class GameItemBase
    {
        [YamlMember(Alias = "name", ApplyNamingConventions = false)]
        public string Name { get; set; }

        [YamlMember(Alias = "number", ApplyNamingConventions = false)]
        public string Number { get; set; }

        [YamlMember(Alias = "tags", ApplyNamingConventions = false)]
        public string Tags { get; set; }
    }

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

    public class PRLamp : GameItemBase { }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class PRSwitch : GameItemBase
    {
        [YamlMember(Alias = "type", ApplyNamingConventions = false)]
        public string SwitchType { get; set; }

        [YamlMember(Alias = "ballsearch", ApplyNamingConventions = false)]
        public string BallSearch { get; set; }
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
        [YamlMember(Alias = "PRGame", ApplyNamingConventions = false)]
        public PRGame PRGame { get; set; }

        [YamlMember(Alias = "PRBumpers", ApplyNamingConventions = false)]
        public List<string> PRBumpers { get; set; }

        [YamlMember(Alias = "PRFlippers", ApplyNamingConventions = false)]
        public List<string> PRFlippers { get; set; }

        [YamlMember(Alias = "PRSwitches", ApplyNamingConventions = false)]
        public List<PRSwitch> PRSwitches { get; set; }

        [YamlMember(Alias = "PRLamps", ApplyNamingConventions = false)]
        public List<PRLamp> PRLamps { get; set; }

        [YamlMember(Alias = "PRCoils", ApplyNamingConventions = false)]
        public List<PRCoil> PRCoils { get; set; }
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

    }
}
