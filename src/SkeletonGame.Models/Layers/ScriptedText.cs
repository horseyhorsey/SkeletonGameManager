﻿using SkeletonGame.Models.Transition;
using System;
using System.Collections.ObjectModel;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
    [Serializable]
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ScriptedText : SequenceTextAnimationBase, ITransitionLayer
    {
        [YamlMember(Alias = "Name", ApplyNamingConventions = false)]
        public string Name { get; set; }

        [YamlMember(Alias = "flashing", ApplyNamingConventions = false)]
        public bool Flashing { get; set; }

        [YamlMember(Alias = "transition", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public TransitionType Transition { get; set; } = TransitionType.None;

        [YamlMember(Alias = "trans_param", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public TransitionParam TransParam { get; set; } = TransitionParam.None;

        [YamlMember(Alias = "trans_length", ApplyNamingConventions = false)]
        public int TransLength { get; set; } = 30;

        [YamlMember(Alias = "transition_out", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public TransitionType TransitionOut { get; set; } = TransitionType.None;

        [YamlMember(Alias = "trans_out_param", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public TransitionParam TransOutParam { get; set; } = TransitionParam.None;

        [YamlMember(Alias = "trans_out_length", ApplyNamingConventions = false)]
        public int TransOutLength { get; set; } = 30;

        [YamlMember(Alias = "trans_out_duration", ApplyNamingConventions = false, SerializeAs = typeof(string))]
        public decimal TransOutDuration { get; set; } = 1.0m;

        [YamlMember(Alias = "TextOptions", ApplyNamingConventions = false)]
        public ObservableCollection<TextOption> TextOptions { get; set; } = new ObservableCollection<TextOption>();

        public ScriptedText()
        {
            this.SeqType = SequenceType.ScriptedText;            
        }
    }
}
