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
using System.Linq;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class AttractViewModel : SkeletonGameManagerViewModelBase
    {
        public ISkeletonGameProvider _skeletonGameProvider { get; set; }
        private ISkeletonGameAttract _skeletonGameAttract;

        public DelegateCommand<object> AddNewSequenceCommand { get; set; }
        public DelegateCommand SaveAttractCommand { get; set; }

        public AttractViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider) : base(eventAggregator)
        {
            _skeletonGameProvider = skeletonGameProvider;
            _skeletonGameAttract = new SkeletonGameAttract();

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x => await OnLoadYamlFilesChanged());

            SaveAttractCommand = new DelegateCommand(() =>
            {

                skeletonGameProvider.SaveAttractFile();

            });

            AddNewSequenceCommand = new DelegateCommand<object>((x) =>
            {
                if (x == null) return;

                var seqType = (AttractSequenceType)x;

                switch (seqType)
                {
                    case AttractSequenceType.LastScores:
                        Sequences.Add(new LastScores() { Name = "LastScores"});
                        _skeletonGameProvider.AttractConfig.AttractSequences.Add(new Sequence(){LastScores = (LastScores)Sequences.Last()});
                        break;
                    case AttractSequenceType.Combo:
                        Sequences.Add(new Combo() { Name = "Combo"});
                        _skeletonGameProvider.AttractConfig.AttractSequences.Add(new Sequence() { Combo = (Combo)Sequences.Last()});
                        break;
                    case AttractSequenceType.text_layer:
                        Sequences.Add(new TextLayer() { Name = "TextLayer" });
                        _skeletonGameProvider.AttractConfig.AttractSequences.Add(new Sequence() { text_layer = (TextLayer)Sequences.Last()});
                        break;
                    case AttractSequenceType.panning_layer:
                        Sequences.Add(new PanningLayer() { Name = "PanningLayer" });
                        _skeletonGameProvider.AttractConfig.AttractSequences.Add(new Sequence() { panning_layer = (PanningLayer)Sequences.Last() });
                        break;
                    case AttractSequenceType.RandomText:
                        Sequences.Add(new RandomText() { Name = "RandomText" });
                        _skeletonGameProvider.AttractConfig.AttractSequences.Add(new Sequence() { RandomText = (RandomText)Sequences.Last() });
                        break;
                    case AttractSequenceType.Animation:
                        Sequences.Add(new AttractAnimation() { Name = "AttractAnimation" });
                        _skeletonGameProvider.AttractConfig.AttractSequences.Add(new Sequence() { Animation = (AttractAnimation)Sequences.Last()});
                        break;
                    case AttractSequenceType.HighScores:
                        Sequences.Add(new HighScores() { Name = "HighScores" });
                        _skeletonGameProvider.AttractConfig.AttractSequences.Add(new Sequence() { HighScores = (HighScores)Sequences.Last() });
                        break;
                    case AttractSequenceType.Credits:
                        Sequences.Add(new Credits() { Name = "Credits" });
                        _skeletonGameProvider.AttractConfig.AttractSequences.Add(new Sequence() { Credits = (Credits)Sequences.Last() });
                        break;
                    case AttractSequenceType.MarkupLayer:
                        Sequences.Add(new MarkupLayer() { Name = "MarkupLayer" });
                        _skeletonGameProvider.AttractConfig.AttractSequences.Add(new Sequence() { MarkupLayer = (MarkupLayer)Sequences.Last() });
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

        public async override Task OnLoadYamlFilesChanged()
        {
            AttractConfig = _skeletonGameProvider.AttractConfig;
            if (AttractConfig != null)
            {
                _skeletonGameAttract.GetAvailableSequences(AttractConfig);

                await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                {
                    Sequences = _skeletonGameProvider.AttractConfig.Sequences;
                });
            }
        }
    }
}
