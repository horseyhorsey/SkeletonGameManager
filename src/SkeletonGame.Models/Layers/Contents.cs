﻿using System;
using System.Collections.ObjectModel;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
    [Serializable]
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Contents
    {
        [YamlMember(Alias = "contents", ApplyNamingConventions = false)]
        public ObservableCollection<Content> Content { get; set; } = new ObservableCollection<Content>();
    }
}
