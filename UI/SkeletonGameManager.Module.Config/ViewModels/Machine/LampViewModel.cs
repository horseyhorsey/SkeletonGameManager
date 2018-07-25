namespace SkeletonGameManager.Module.Config.ViewModels.Machine
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class LampViewModel
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string Tags { get; set; }
        public string Label { get; set; }
    }
}
