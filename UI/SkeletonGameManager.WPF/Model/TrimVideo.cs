using System;

namespace SkeletonGameManager.WPF.Model
{
    public class TrimVideo
    {
        /// <summary>
        /// Gets or sets the file for converting from
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        public int StartFrame { get; set; }

        /// <summary>
        /// Gets or sets the frames to take
        /// </summary>
        public int Frames { get; set; }

        /// <summary>
        /// Gets the frame rate of the video
        /// </summary>
        public double FrameRate { get; internal set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        public TimeSpan EndTime { get; set; }
    }
}
