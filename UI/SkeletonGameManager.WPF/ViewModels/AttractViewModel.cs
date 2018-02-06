using Prism.Events;
using SkeletonGameManager.WPF.Events;
using SkeletonGame.Models;
using SkeletonGameManager.WPF.Providers;
using SkeletonGame.Engine;
using System.Threading.Tasks;
using Prism.Commands;
using SkeletonGame.Models.Attract;
using System.Windows.Threading;
using GongSolutions.Wpf.DragDrop;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class AttractViewModel : SequenceViewModelBase, IDropTarget
    {
        public ISkeletonGameProvider _skeletonGameProvider { get; set; }
        private ISkeletonGameAttract _skeletonGameAttract;

        public DelegateCommand<object> AddNewSequenceCommand { get; set; }
        public DelegateCommand<object> DuplicateSequenceCommand { get; set; }        
        public DelegateCommand<object> AddLayerCommand { get; set; }
        
        public DelegateCommand SaveAttractCommand { get; set; }

        public Sequence CreateSequenceFromBase(SequenceBase sequenceBase)
        {
            Sequence seq = new Sequence();

            switch (sequenceBase.SeqType)
            {
                case SequenceType.Animation:
                    seq.AttractAnimation =(AttractAnimation)sequenceBase;
                    break;
                case SequenceType.Combo:
                    seq.Combo = (Combo)sequenceBase;
                    break;
                case SequenceType.Credits:
                    seq.Credits = (Credits)sequenceBase;
                    break;
                case SequenceType.GroupLayer:
                    seq.GroupLayer = (GroupLayer)sequenceBase;
                    break;
                case SequenceType.HighScores:
                    seq.HighScores = (HighScores)sequenceBase;
                    break;
                case SequenceType.LastScores:
                    seq.LastScores = (LastScores)sequenceBase;
                    break;
                case SequenceType.MarkupLayer:
                    seq.MarkupLayer = (MarkupLayer)sequenceBase;
                    break;
                case SequenceType.PanningLayer:
                    seq.panning_layer = (PanningLayer)sequenceBase;
                    break;
                case SequenceType.RandomText:
                    seq.RandomText = (RandomText)sequenceBase;
                    break;
                case SequenceType.ScriptedText:
                    seq.ScriptedText = (ScriptedText)sequenceBase;
                    break;
                case SequenceType.TextLayer:
                    seq.text_layer = (TextLayer)sequenceBase;
                    break;
                default:
                    break;
            }

            return seq;
        }

        public AttractViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider) : base(eventAggregator)
        {
            _skeletonGameProvider = skeletonGameProvider;
            _skeletonGameAttract = new SkeletonGameAttract();

            Sequences = new ObservableCollection<SequenceItemViewModel>();
            Sequences.CollectionChanged += Sequences_CollectionChanged;

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x => await OnLoadYamlFilesChanged());

            SaveAttractCommand = new DelegateCommand(() =>
            {
                //Clear old Sequence and replace with new.
                AttractConfig.AttractSequences.Clear();
                foreach (var sequence in Sequences)
                {
                    AttractConfig.AttractSequences.Add(new Sequence(sequence.Sequence));
                }

                //Save to yaml
                skeletonGameProvider.SaveAttractFile();
            });

            DuplicateSequenceCommand = new DelegateCommand<object>((x) =>
            {
                var seqbase = x as SequenceBase;                

                if (seqbase != null)
                {
                    if (seqbase.SeqType == SequenceType.GroupLayer)
                    {
                        var  groupBase =  (GroupLayer)seqbase;

                        var newGroup = new GroupLayer();

                        foreach (var item in groupBase.Contents.ToList())
                        {
                            newGroup.Contents.Add(new Content
                            {
                                animation_layer = item.animation_layer
                            });
                        }                            

                        this.Sequences.Add(new SequenceItemViewModel(newGroup));
                    }
                }
                    
            });

            AddNewSequenceCommand = new DelegateCommand<object>((x) =>
            {
                if (x == null) return;

                var seqType = (SequenceType)x;
                Sequence seq = null;

                switch (seqType)
                {
                    case SequenceType.LastScores:
                        var lastScores = new LastScores() { SequenceName = "LastScores"};
                        seq = new Sequence() { LastScores = lastScores };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.LastScores);
                        Sequences.Add(new SequenceItemViewModel(seq.LastScores));
                        break;
                    case SequenceType.Combo:
                        var combo = new Combo() { SequenceName = "Combo"};                        
                        seq = new Sequence() { Combo = combo };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.Combo);
                        Sequences.Add(new SequenceItemViewModel(seq.Combo));
                        break;
                    case SequenceType.TextLayer:
                        var txtLayr = new TextLayer() { SequenceName = "TextLayer" };                        
                        seq = new Sequence() { text_layer = txtLayr };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.text_layer);
                        Sequences.Add(new SequenceItemViewModel(seq.text_layer));
                        break;
                    case SequenceType.PanningLayer:
                        var panning_layer = new PanningLayer() { SequenceName = "panning_layer" };
                        seq = new Sequence() { panning_layer = panning_layer };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.panning_layer);
                        Sequences.Add(new SequenceItemViewModel(seq.panning_layer));
                        break;
                    case SequenceType.RandomText:
                        var rndText = new RandomText() { SequenceName = "RandomText" };                        
                        seq = new Sequence() { RandomText = rndText };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.RandomText);
                        Sequences.Add(new SequenceItemViewModel(seq.RandomText));
                        break;
                    case SequenceType.Animation:
                        var attAnim = new AttractAnimation() { Name = "Animation" };                        
                        seq = new Sequence() { AttractAnimation = attAnim };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.AttractAnimation);
                        Sequences.Add(new SequenceItemViewModel(seq.AttractAnimation));
                        break;
                    case SequenceType.HighScores:
                        var hiScore = new HighScores() { SequenceName = "HighScores" };                        
                        seq = new Sequence() { HighScores = hiScore };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.HighScores);
                        Sequences.Add(new SequenceItemViewModel(seq.HighScores));
                        break;
                    case SequenceType.Credits:
                        var credits = new Credits() { SequenceName = "Credits" };  
                        seq = new Sequence() { Credits = credits};                        
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.Credits);
                        Sequences.Add(new SequenceItemViewModel(seq.Credits));
                        break;
                    case SequenceType.MarkupLayer:
                        var markup = new MarkupLayer() { SequenceName = "MarkupLayer" };                        
                        seq = new Sequence() { MarkupLayer = markup };                        
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.MarkupLayer);
                        Sequences.Add(new SequenceItemViewModel(seq.MarkupLayer));
                        break;
                    case SequenceType.ScriptedText:
                        var scriptedText = new ScriptedText() { Name = "ScriptedText" };
                        seq = new Sequence() { ScriptedText = scriptedText };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.ScriptedText);
                        Sequences.Add(new SequenceItemViewModel(seq.ScriptedText));
                        break;
                    case SequenceType.GroupLayer:
                        var groupLayer = new GroupLayer() { Name = "GroupLayer" };
                        seq = new Sequence() { GroupLayer = groupLayer };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.GroupLayer);
                        Sequences.Add(new SequenceItemViewModel(seq.GroupLayer));
                        break;
                    case SequenceType.MoveLayer:
                        var moveLayer = new MoveLayer() { SequenceName = "MoveLayer" };
                        seq = new Sequence() { MoveLayer = moveLayer };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.MoveLayer);
                        Sequences.Add(new SequenceItemViewModel(seq.MoveLayer));
                        break;
                    default:
                        break;
                }

                if (seq != null)
                {
                    _skeletonGameProvider.AttractConfig.AttractSequences.Add(seq);
                }
            });

            AddLayerCommand = new DelegateCommand<object>((x) =>
            {
                //if (x == "Animation")
                //var meh = x.DataContext as GroupLayer;
                GroupLayer group = x as GroupLayer;

                if (group != null)
                {
                    if (SelectedGroupLayerType == "0")
                        group.Contents.Add(new Content() { animation_layer = new AttractAnimation() , SeqType = SequenceType.Animation});
                    else if (SelectedGroupLayerType == "1")
                        group.Contents.Add(new Content() { markup_layer = new MarkupLayer(), SeqType = SequenceType.MarkupLayer });
                    else if (SelectedGroupLayerType == "2")
                        group.Contents.Add(new Content() { text_layer = new TextLayer(), SeqType = SequenceType.TextLayer });
                    else if (SelectedGroupLayerType == "3")
                        group.Contents.Add(new Content() { combo_layer = new Combo(), SeqType = SequenceType.Combo });
                    else if (SelectedGroupLayerType == "4")
                        group.Contents.Add(new Content() { scripted_text_layer= new ScriptedText(), SeqType = SequenceType.ScriptedText });                    

                }
                
                //x.DataContext as DataContext
                //Don't do anything if null list
                //x?.Add(new Sequence() { LastScores = new LastScores() { Name = "LastScores" }});
            });
        }

        private void Sequences_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
        }

        public virtual void DragOver(IDropInfo dropInfo)
        {
            //throw new System.NotImplementedException();
            if (dropInfo.IsSameDragDropContextAsSource)
                dropInfo.Effects = System.Windows.DragDropEffects.Move;
        }

        public virtual void Drop(IDropInfo dropInfo)
        {
            try
            {
                if (dropInfo.IsSameDragDropContextAsSource)
                {
                    this.Sequences.RemoveAt(dropInfo.DragInfo.SourceIndex);
                    this.Sequences.Insert(dropInfo.InsertIndex, dropInfo.Data as SequenceItemViewModel);
                }
            }
            catch { }
            
        }

        private AttractYaml attractConfig;
        public AttractYaml AttractConfig
        {
            get { return attractConfig; }
            set { SetProperty(ref attractConfig, value); }
        }

        private string selectedGroupLayerType;
        public string SelectedGroupLayerType
        {
            get { return selectedGroupLayerType; }
            set { SetProperty(ref selectedGroupLayerType, value); }
        }

        /// <summary>
        /// Called when [load yaml files changed]. Gets available sequences from the attract file.
        /// </summary>
        /// <returns></returns>
        public async override Task OnLoadYamlFilesChanged()
        {
            //Assign the attract config
            if (AttractConfig == null)
                AttractConfig = _skeletonGameProvider.AttractConfig;

            if (AttractConfig != null)
            {
                //Assign the attract config
                AttractConfig = _skeletonGameProvider.AttractConfig;

                //Clear sequences and convert the attract configs values to sequence item view models
                await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                {                    
                    Sequences?.Clear();

                    _skeletonGameAttract.GetAvailableSequences(AttractConfig);

                    foreach (var attractSeq in AttractConfig.Sequences)
                    {
                        Sequences.Add(new SequenceItemViewModel(attractSeq));
                    }                    
                });
            }
        }
    }


}
