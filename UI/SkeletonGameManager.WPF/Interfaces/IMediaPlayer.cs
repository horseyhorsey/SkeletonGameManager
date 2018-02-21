using System;

namespace SkeletonGameManager.WPF.Interfaces
{
    public interface IMediaPlayer
    {
        void Pause();

        void Play();

        void Stop();

        TimeSpan GetCurrentTime();

        TimeSpan GetCurrentPosition();

        void SetPosition(int framePos);

        double FrameRate { get; set; }

        double FrameCount { get; set; }
    }
}
