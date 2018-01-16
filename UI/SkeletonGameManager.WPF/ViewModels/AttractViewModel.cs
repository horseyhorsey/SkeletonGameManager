using System;
using Prism.Events;
using SkeletonGameManager.WPF.Events;
using SkeletonGame.Models;
using SkeletonGameManager.WPF.Providers;
using System.Collections.ObjectModel;
using SkeletonGame.Engine;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class AttractViewModel : SkeletonGameManagerViewModelBase
    {
        private ISkeletonGameProvider _skeletonGameProvider;
        private ISkeletonGameAttract _skeletonGameAttract;

        public AttractViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider) : base(eventAggregator)
        {            
            _skeletonGameProvider = skeletonGameProvider;
            _skeletonGameAttract = new SkeletonGameAttract();

            Sequences = new ObservableCollection<SequenceBase>();

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(x => OnLoadYamlFilesChanged());
        }

        private AttractYaml attractConfig;        
        public AttractYaml AttractConfig
        {
            get { return attractConfig; }
            set { SetProperty(ref attractConfig, value); }
        }

        private ObservableCollection<SequenceBase> sequences;
        public ObservableCollection<SequenceBase> Sequences
        {
            get { return sequences; }
            set { SetProperty(ref sequences, value); }
        }

        private SequenceBase selectedSequence;
        public SequenceBase SelectedSequence
        {
            get { return selectedSequence; }
            set { SetProperty(ref selectedSequence, value); }
        }

        public override void OnLoadYamlFilesChanged()
        {
            AttractConfig = _skeletonGameProvider.AttractConfig;

            if (AttractConfig != null)
            {
                var _sequences = _skeletonGameAttract.GetAvailableSequences(AttractConfig);
                Sequences = new ObservableCollection<SequenceBase>(_sequences);
            }            
        }
    }
}
