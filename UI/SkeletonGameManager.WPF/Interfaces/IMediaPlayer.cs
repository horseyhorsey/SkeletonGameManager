using System;

namespace SkeletonGameManager.WPF.Interfaces
{
    public interface IMediaPlayer
    {
        void Pause();

        void Play();

        void Stop();

        TimeSpan GetCurrentTime();

        void SetPosition(TimeSpan ts);
    }
}
