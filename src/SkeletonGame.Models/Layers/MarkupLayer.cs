﻿using SkeletonGame.Models.Transition;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
    [Serializable]
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class MarkupLayer : TransitionBaseLayer
    {
        [YamlMember(Alias = "width", ApplyNamingConventions = false)]
        public string Width { get; set; } = "100";

        [YamlMember(Alias = "Bold", ApplyNamingConventions = false)]
        public Bold Bold { get; set; } = new Bold() { Font = "default" };

        [YamlMember(Alias = "Normal", ApplyNamingConventions = false)]
        public Normal Normal { get; set; } = new Normal() { Font = "default" };

        private List<string> _textList = new List<string>() { "" };
        [YamlMember(Alias = "Message", ApplyNamingConventions = false)]
        public List<string> TextList
        {
            get
            {
                return _textList;
            }

            set
            {
                _textList = value;

                if (_textList != null)
                {
                    TextEntries.Clear();
                    foreach (var textLine in _textList)
                    {
                        TextEntries.Add(new TextListViewModel { TextLine = textLine });
                    }
                }
                else
                {
                    TextEntries.Clear();
                }
            }
        }

        [YamlIgnore]
        public ObservableCollection<TextListViewModel> TextEntries { get; set; } = new ObservableCollection<TextListViewModel>();

        [YamlMember(Alias = "Animation", ApplyNamingConventions = false)]
        public string Animation { get; set; }

        public MarkupLayer()
        {
            this.SeqType = SequenceType.MarkupLayer;
        }
    }

    [Serializable]
    public class Bold
    {
        [YamlMember(Alias = "Font", ApplyNamingConventions = false)]
        public string Font { get; set; } = "default";

        [YamlMember(Alias = "FontStyle", ApplyNamingConventions = false)]
        public string FontStyle { get; set; } = "default";
    }

    [Serializable]
    public class Normal
    {
        [YamlMember(Alias = "Font", ApplyNamingConventions = false)]
        public string Font { get; set; } = "default";

        [YamlMember(Alias = "FontStyle", ApplyNamingConventions = false)]
        public string FontStyle { get; set; } = "default";
    }
}
