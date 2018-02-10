using PropertyChanged;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Animation : AssetKeyFile
    {
        [YamlMember(Alias = "frame_time", ApplyNamingConventions = false)]
        public int FrameTime { get; set; } = 1;

        [YamlMember(Alias = "repeatAnim", ApplyNamingConventions = false)]
        public bool Repeat { get; set; } = false;

        [YamlMember(Alias = "x_loc", ApplyNamingConventions = false)]
        public int XLoc { get; set; } = 0;

        [YamlMember(Alias = "y_loc", ApplyNamingConventions = false)]
        public int YLoc { get; set; } = 0;

        [YamlMember(Alias = "scale", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public decimal Scale { get; set; } = 1.0m;

        [YamlMember(Alias = "composite_op", ApplyNamingConventions = false)]
        public string CompositeOp { get; set; } = null;

        [YamlMember(Alias = "holdLastFrame", ApplyNamingConventions = false)]
        public bool HoldLastFrame { get; set; } = false;

        [YamlMember(Alias = "sequence", ApplyNamingConventions = false)]
        public List<int> Sequence { get; set; }

        [YamlMember(Alias = "streamingMovie", ApplyNamingConventions = false)]
        public bool StreamingMovie { get; set; } = false;

        [YamlMember(Alias = "skipPreLoading", ApplyNamingConventions = false)]
        public bool SkipPreLoading { get; set; } = false;

        [YamlMember(Alias = "streamingPNG", ApplyNamingConventions = false)]
        /// <summary>
        /// PNG frames will not be pre-loaded, but rather will be loaded from the disk every time they are requested
        /// </summary>
        public bool StreamingPng { get; set; } = false;

        [YamlMember(Alias = "streamingPNG_Cached", ApplyNamingConventions = false)]
        /// <summary>
        /// png frames will be loaded from the disk the first time they are requested, but cached thereafter
        /// </summary>
        public bool StreamingPngCached { get; set; } = false;
    }
}
