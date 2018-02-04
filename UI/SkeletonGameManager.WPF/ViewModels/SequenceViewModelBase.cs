using Prism.Events;
using SkeletonGame.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class SequenceViewModelBase : SkeletonGameManagerViewModelBase
    {
        public SequenceViewModelBase(IEventAggregator eventAggregator) : base(eventAggregator)
        {

        }

        private void Sequences_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
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

    }
}
