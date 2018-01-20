using Prism.Commands;
using Prism.Events;
using SkeletonGameManager.WPF.Events;
using System;
using System.Windows.Input;

namespace SkeletonGameManager.WPF.ViewModels.SceneEditViewModels
{
    public class SceneProcessItemViewModel
    {
        private IEventAggregator _eventAggregator;
        public ICommand RemoveCommand { get; set; }

        public SceneProcessItemViewModel(IEventAggregator ea)
        {
            _eventAggregator = ea;
            RemoveCommand = new DelegateCommand(Remove);
        }

        private void Remove()
        {
            _eventAggregator.GetEvent<VideoProcessItemRemoveEvent>().Publish(this);
        }

        public string SystemName { get; set; }
        public string File { get; set; }
        public bool Overwrite { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan Duration { get; set; }                
    }
}
