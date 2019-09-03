using Prism.Events;
using SkeletonGameManager.Module.SceneGrab.Model;
using SkeletonGameManager.Module.SceneGrab.ViewModels;

namespace SkeletonGameManager.Module.SceneGrab.Events
{
    public static class ViewModelEvents
    {
        public class VideoProcessItemRemoveEvent : PubSubEvent<SceneProcessItemViewModel> { }
        public class VideoProcessItemAddedEvent : PubSubEvent<TrimVideo> { }
    }
}
