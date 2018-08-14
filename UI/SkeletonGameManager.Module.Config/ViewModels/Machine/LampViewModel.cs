using SkeletonGame.Models.Machine;

namespace SkeletonGameManager.Module.Config.ViewModels.Machine
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class LampViewModel
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string Tags { get; set; }
        public string Label { get; set; }

        public LampViewModel(PRLamp lamp)
        {
            Name = lamp.Name;
            Number = lamp.Number;
            Tags = lamp.Tags;
            Label = lamp.Label;
        }

        public LampViewModel(PRLed led)
        {
            Name = led.Name;
            Number = led.Number;
            Tags = led.Tags;
            Label = led.Label;
        }

        public LampViewModel()
        {
                
        }                
    }
}
