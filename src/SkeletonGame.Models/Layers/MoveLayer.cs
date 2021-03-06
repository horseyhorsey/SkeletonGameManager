﻿using SkeletonGame.Models.Transforms;
using SkeletonGame.Models.Transition;
using System;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
    [Serializable]
    public class MoveLayer : TransitionBaseLayer, IMoveLayer
    {
        public MoveLayer()
        {
            this.SeqType = SequenceType.MoveLayer;
        }

        [YamlMember(Alias = "enabled", ApplyNamingConventions = false, Order = 0, SerializeAs = typeof(string))]
        public string IsEnabled { get; set; }

        [YamlMember(Alias = "frames", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public int frames { get; set; } = 15;

        [YamlMember(Alias = "loop", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public bool loop { get; set; } = false;

        [YamlMember(Alias = "target_x", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public string target_x { get; set; } = "0";

        [YamlMember(Alias = "target_y", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public string target_y { get; set; } = "0";

        [YamlMember(Alias = "start_x", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public string start_x { get; set; } = "0";

        [YamlMember(Alias = "start_y", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public string start_y { get; set; } = "0";

        //[YamlMember(Alias = "move_text", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        //public bool move_text { get; set; } = true;

        //[YamlMember(Alias = "move_anim", ApplyNamingConventions = false)]
        //public bool move_anim { get; set; } = true;
    }
}
