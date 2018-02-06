﻿using Prism.Mvvm;
using SkeletonGame.Models;

namespace SkeletonGameManager.WPF.ViewModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class SequenceItemViewModel : BindableBase
    {
        public SequenceItemViewModel(SequenceBase sequence)
        {
            Sequence = sequence;

            Sequence.SequenceName = Sequence.SeqType + "SequenceStyle";
        }

        public SequenceBase Sequence { get; set; }

        public string Name { get; private set; }
    }
}