using System;
using Prism.Events;
using SkeletonGameManager.WPF.ViewModels.SceneEditViewModels;
using SkeletonGameManager.WPF.Model;

namespace SkeletonGameManager.WPF.Events
{
    public class LoadYamlFilesChanged : PubSubEvent<object> { }    
    public class OnGameEndedEvent : PubSubEvent<bool> { }
    public class OnLaunchGameEvent : PubSubEvent<object> { }
    public class VideoProcessItemRemoveEvent : PubSubEvent<SceneProcessItemViewModel> { }
    public class VideoProcessItemAddedEvent : PubSubEvent<TrimVideo> { }
    public class VideoSourceEvent : PubSubEvent<string> { }    
    
}
