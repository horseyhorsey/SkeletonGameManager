﻿using SkeletonGameManager.WPF.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace SkeletonGameManager.WPF.Views
{
    /// <summary>
    /// Interaction logic for SceneMediaView.xaml
    /// </summary>
    public partial class SceneMediaView : UserControl, IMediaPlayer
    {
        #region Fields
        private bool isPlaying;
        private DispatcherTimer timer = new DispatcherTimer();
        public bool IsSeekingMedia { get; private set; }
        public double FrameRate { get; set; }
        public double FrameCount { get; set; }
        #endregion

        #region Constructor
        public SceneMediaView()
        {
            InitializeComponent();            
            MediaElement.ScrubbingEnabled = true;
            MediaElement.Pause();
        }
        #endregion

        #region Media Player Interface
        void IMediaPlayer.Pause()
        {
            if (MediaElement.CanPause)
            {
                MediaElement.Pause();

                isPlaying = false;

                StopTimer();
            }
        }

        void IMediaPlayer.Play()
        {
            if (!isPlaying)
            {
                StartTimer();

                //Play from sliders position
                if (timelineSlider.Value > 0)
                {
                    var secondsFromSlider = timelineSlider.Value / FrameRate;
                    var valu = TimeSpan.FromSeconds(secondsFromSlider);
                    MediaElement.Position = valu;
                }                    

                MediaElement.Play();

                isPlaying = true;
            }
        }

        void IMediaPlayer.Stop()
        {
            MediaElement.Stop();

            isPlaying = false;

            StopTimer();
        }

        TimeSpan IMediaPlayer.GetCurrentTime()
        {
            return MediaElement.Position;            
        }
        #endregion

        #region Time Slider
        private void timelineSlider_SeekStarted(object sender, DragStartedEventArgs e)
        {
            IsSeekingMedia = true;
        }

        private void timelineSlider_SeekCompleted(object sender, DragCompletedEventArgs e)
        {
            IsSeekingMedia = false;
            if (MediaElement.Source != null)
            {
                //MediaElement.Position = TimeSpan.FromSeconds(timelineSlider.Value);
                var secondsFromSlider = timelineSlider.Value / FrameRate;
                var valu = TimeSpan.FromSeconds(secondsFromSlider);
                MediaElement.Position = valu;
            }            
        }

        private void timelineSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //media_wheelTime.Text = TimeSpan.FromSeconds(timelineSlider.Value).ToString(@"hh\:mm\:ss");
        }

        private void SetSeekBarToZero()
        {
            var ts = new TimeSpan(0, 0, 0, 0);
            
            timelineSlider.Value = 0;
        }
        #endregion

        #region Media Events
        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            SetSeekBarToZero();

            try
            {
                timelineSlider.Maximum = FrameCount;
                //timelineSlider.Maximum = MediaElement.NaturalDuration.TimeSpan.TotalMilliseconds / 1000;
            }
            catch { }
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            SetSeekBarToZero();
            StopTimer();
            MediaElement.Stop();
            isPlaying = false;
            MediaElement.Position = TimeSpan.FromSeconds(timelineSlider.Value);            
        }
        #endregion

        #region Timer
        private void StartTimer()
        {
            timer.Interval = TimeSpan.FromMilliseconds(FrameRate / 1000);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void StopTimer()
        {
            timer.Tick -= timer_Tick;
            timer.Stop();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((MediaElement.Source != null) && (MediaElement.NaturalDuration.HasTimeSpan) && (!IsSeekingMedia))
            {
                //timelineSlider.Minimum = 0;
                var mediaPos = MediaElement.Position.TotalSeconds;
                timelineSlider.Value = mediaPos * FrameRate;
            }
        }

        /// <summary>
        /// Sets the position of the slider and media position
        /// </summary>
        /// <param name="framePos">The frame position.</param>
        public void SetPosition(int framePos)
        {
            if (MediaElement.Source != null)
            {
                var secondsFromSlider = framePos / FrameRate;
                var valu = TimeSpan.FromSeconds(secondsFromSlider);
                //MediaElement.Position = valu;
                timelineSlider.Value = framePos;
                MediaElement.Position = valu;
            }            
        }

        public TimeSpan GetCurrentPosition()
        {
            return MediaElement.Position;
        }

        #endregion
    }
}
