using SkeletonGame.Models.Machine;
using System.Collections.Generic;

namespace SkeletonGameManager.Module.Config.ViewModels.Machine
{
    /// <summary>
    /// Represents a coil..a Flasher or Std solenoid
    /// </summary>
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class SolenoidFlasherViewModel
    {        
        public SolenoidFlasherViewModel(PRCoil coil)
        {
            this.Number = coil.Number;
            this.Name = coil.Name;
            this.Tags = coil.Tags;
            this.Label = coil.Label;
            this.PatterOffTime = coil.PatterOffTime;
            this.PatterOnTime = coil.PatterOnTime;
            this.BallSearch = coil.BallSearch;
        }

        public SolenoidFlasherViewModel()
        {

        }

        public string Number { get; set; }

        public string Name { get; set; }

        public string Tags { get; set; }

        public string Label { get; set; }

        public byte? PulseTime { get; set; }

        public byte? PatterOnTime { get; set; }

        public byte? PatterOffTime { get; set; }

        public bool BallSearch { get; set; }

        public SolenoidType SolenoidType { get; set; }
        
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class FlipperCiruitCoilViewModel
    {
        public string Number { get; set; }

        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        public bool ReadOnly { get; set; }

        public string Tags { get; set; }
    }
}
