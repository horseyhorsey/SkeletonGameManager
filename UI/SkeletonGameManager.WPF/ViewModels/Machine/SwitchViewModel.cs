namespace SkeletonGameManager.WPF.ViewModels.Machine
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class SwitchViewModel
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string Tags { get; set; }
        public string Type { get; set; }
        public bool State { get; set; }
        public int PosTop { get; set; }
        public int PosLeft { get; set; }

        public string[] BallSearch = new string[] { null, null };

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
