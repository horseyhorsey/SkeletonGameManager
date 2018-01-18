using Prism.Events;
using SkeletonGameManager.WPF.Events;
using SkeletonGame.Models;
using SkeletonGameManager.WPF.Providers;
using System.Collections.ObjectModel;
using SkeletonGame.Engine;
using System.Threading.Tasks;
using Prism.Commands;
using SkeletonGame.Models.Attract;
using System.Windows.Threading;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class AttractViewModel : SkeletonGameManagerViewModelBase
    {
        private ISkeletonGameProvider _skeletonGameProvider;
        private ISkeletonGameAttract _skeletonGameAttract;

        public DelegateCommand<object> AddNewSequenceCommand { get; set; }

        public AttractViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider) : base(eventAggregator)
        {            
            _skeletonGameProvider = skeletonGameProvider;
            _skeletonGameAttract = new SkeletonGameAttract();

            Sequences = new ObservableCollection<AttractSequenceViewModel>();

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x => await OnLoadYamlFilesChanged());

            AddNewSequenceCommand = new DelegateCommand<object>((x) =>
            {
                if (x == null) return;

                var seqType = (AttractSequenceType)x;

                switch (seqType)
                {
                    case AttractSequenceType.LastScores:
                        Sequences.Add(new AttractSequenceViewModel(new LastScores(), _skeletonGameProvider));
                        break;
                    case AttractSequenceType.Combo:
                        Sequences.Add(new AttractSequenceViewModel(new Combo(), _skeletonGameProvider));
                        break;
                    case AttractSequenceType.text_layer:
                        Sequences.Add(new AttractSequenceViewModel(new TextLayer(), _skeletonGameProvider));
                        break;
                    case AttractSequenceType.panning_layer:
                        Sequences.Add(new AttractSequenceViewModel(new PanningLayer(), _skeletonGameProvider));
                        break;
                    case AttractSequenceType.RandomText:
                        Sequences.Add(new AttractSequenceViewModel(new RandomText(), _skeletonGameProvider));
                        break;
                    case AttractSequenceType.Animation:
                        Sequences.Add(new AttractSequenceViewModel(new AttractAnimation(), _skeletonGameProvider));
                        break;
                    case AttractSequenceType.HighScores:
                        Sequences.Add(new AttractSequenceViewModel(new HighScores(),_skeletonGameProvider));
                        break;
                    case AttractSequenceType.Credits:
                        Sequences.Add(new AttractSequenceViewModel(new Credits(),_skeletonGameProvider));
                        break;
                    default:
                        break;
                }                
            });
        }

        private AttractYaml attractConfig;        
        public AttractYaml AttractConfig
        {
            get { return attractConfig; }
            set { SetProperty(ref attractConfig, value); }
        }

        private ObservableCollection<AttractSequenceViewModel> sequences;
        public ObservableCollection<AttractSequenceViewModel> Sequences
        {
            get { return sequences; }
            set { SetProperty(ref sequences, value); }
        }

        private AttractSequenceViewModel selectedSequence;
        public AttractSequenceViewModel SelectedSequence
        {
            get { return selectedSequence; }
            set { SetProperty(ref selectedSequence, value); }
        }

        public async override Task OnLoadYamlFilesChanged()
        {            
                AttractConfig = _skeletonGameProvider.AttractConfig;

                if (AttractConfig != null)
                {
                    var _sequences = _skeletonGameAttract.GetAvailableSequences(AttractConfig);

                    await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                    {
                        Sequences = new ObservableCollection<AttractSequenceViewModel>();

                        foreach (var sequence in _sequences)
                        {
                            Sequences.Add(new AttractSequenceViewModel(sequence, _skeletonGameProvider));
                        }

                    });
                    
                }
        }
    }
}
