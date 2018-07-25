using Prism.Events;

namespace SkeletonGameManager.Base
{
    public class Events
    {
        public class LoadYamlFilesChanged : PubSubEvent<object> { }
        public class OnGameEndedEvent : PubSubEvent<bool> { }
        public class OnLaunchGameEvent : PubSubEvent<object> { }

        public class VideoSourceEvent : PubSubEvent<string> { }
    }
}
