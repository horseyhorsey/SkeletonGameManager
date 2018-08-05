using Prism.Events;
using Prism.Logging;
using SkeletonGame.Models;
using SkeletonGameManager.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SkeletonGameManager.Module.SceneManage.ViewModels
{
    public class SequenceViewModelBase : SkeletonGameManagerViewModelBase
    {
        public SequenceViewModelBase(IEventAggregator eventAggregator, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {

        }

        private void Sequences_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
        }

        private ObservableCollection<SequenceItemViewModel> sequences;
        public ObservableCollection<SequenceItemViewModel> Sequences
        {
            get { return sequences; }
            set { SetProperty(ref sequences, value); }
        }

        private SequenceItemViewModel selectedSequence;
        public SequenceItemViewModel SelectedSequence
        {
            get { return selectedSequence; }
            set { SetProperty(ref selectedSequence, value); }
        }

    }
}
