using SkeletonGame.Models.Machine;
using System.Linq;

namespace SkeletonGameManager.Module.Config.ViewModels.Machine
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class SwitchViewModel
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string Tags { get; set; }
        public string Label { get; set; }
        public ProcSwitchType? Type { get; set; }
        public VpSwitchType? VpSwitchType { get; set; }
        public bool State { get; set; }
        public int PosTop { get; set; }
        public int PosLeft { get; set; }

        public string[] BallSearch = new string[2];

        private bool reset;
        public bool Reset
        {
            get { return reset; }
            set
            {
                reset = value;
                if (Reset) BallSearch[1] = "reset";
                else BallSearch[1] = null;
            }
        }



        public SwitchViewModel(PRSwitch _switch)
        {
            this.Name = _switch.Name;
            this.Number = _switch.Number;
            this.VpSwitchType = _switch.VpSwitchType;
            this.Tags = _switch.Tags;
            this.Label = _switch.Label;

            if (_switch.BallSearch != null)
            {
                var _search = _switch.BallSearch.Split(',');
                try
                {
                    if (_search[0].Trim() == "stop")
                        Stop = true;
                    if (_search[1].Trim() == "reset")
                        Reset = true;
                }
                catch { }
            }
        }

        public SwitchViewModel()
        {

        }

        private bool stop;
        public bool Stop
        {
            get { return stop; }
            set
            {
                stop = value;
                if (Stop) BallSearch[0] = "stop";
                else BallSearch[0] = null;
            }
        }
    }
}
