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

namespace SkeletonGameManager.WPF.ViewModels
{
    public class AttractViewModel : SequenceViewModelBase, IDropTarget
    {
        public ISkeletonGameProvider _skeletonGameProvider { get; set; }
        private ISkeletonGameAttract _skeletonGameAttract;

        public DelegateCommand<object> AddNewSequenceCommand { get; set; }
        public DelegateCommand<object> AddLayerCommand { get; set; }
        
        public DelegateCommand SaveAttractCommand { get; set; }

        public Sequence CreateSequenceFromBase(SequenceBase sequenceBase)
        {
            Sequence seq = new Sequence();

            switch (sequenceBase.SeqType)
            {
                case AttractSequenceType.Animation:
                    seq.AttractAnimation =(AttractAnimation)sequenceBase;
                    break;
                case AttractSequenceType.Combo:
                    seq.Combo = (Combo)sequenceBase;
                    break;
                case AttractSequenceType.Credits:
                    seq.Credits = (Credits)sequenceBase;
                    break;
                case AttractSequenceType.GroupLayer:
                    seq.GroupLayer = (GroupLayer)sequenceBase;
                    break;
                case AttractSequenceType.HighScores:
                    seq.HighScores = (HighScores)sequenceBase;
                    break;
                case AttractSequenceType.LastScores:
                    seq.LastScores = (LastScores)sequenceBase;
                    break;
                case AttractSequenceType.MarkupLayer:
                    seq.MarkupLayer = (MarkupLayer)sequenceBase;
                    break;
                case AttractSequenceType.PanningLayer:
                    seq.panning_layer = (PanningLayer)sequenceBase;
                    break;
                case AttractSequenceType.RandomText:
                    seq.RandomText = (RandomText)sequenceBase;
                    break;
                case AttractSequenceType.ScriptedText:
                    seq.ScriptedText = (ScriptedText)sequenceBase;
                    break;
                case AttractSequenceType.TextLayer:
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

            Sequences = new ObservableCollection<SequenceBase>();
            Sequences.CollectionChanged += Sequences_CollectionChanged; ;

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x => await OnLoadYamlFilesChanged());

            SaveAttractCommand = new DelegateCommand(() =>
            {
                //Clear old Sequence and replace with new.
                AttractConfig.AttractSequences.Clear();
                foreach (var item in Sequences)
                {
                    AttractConfig.AttractSequences.Add(new Sequence(item));
                }

                //Save to yaml
                skeletonGameProvider.SaveAttractFile();
            });

            AddNewSequenceCommand = new DelegateCommand<object>((x) =>
            {
                if (x == null) return;

                var seqType = (AttractSequenceType)x;
                Sequence seq = null;

                switch (seqType)
                {
                    case AttractSequenceType.LastScores:
                        var lastScores = new LastScores() { Name = "LastScores"};
                        seq = new Sequence() { LastScores = lastScores };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.LastScores);
                        break;
                    case AttractSequenceType.Combo:
                        var combo = new Combo() { Name = "Combo"};                        
                        seq = new Sequence() { Combo = combo };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.Combo);
                        break;
                    case AttractSequenceType.TextLayer:
                        var txtLayr = new TextLayer() { Name = "TextLayer" };                        
                        seq = new Sequence() { text_layer = txtLayr };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.text_layer);
                        break;
                    case AttractSequenceType.PanningLayer:
                        var panning_layer = new PanningLayer() { Name = "panning_layer" };
                        seq = new Sequence() { panning_layer = panning_layer };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.panning_layer);
                        break;
                    case AttractSequenceType.RandomText:
                        var rndText = new RandomText() { Name = "RandomText" };                        
                        seq = new Sequence() { RandomText = rndText };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.RandomText);
                        break;
                    case AttractSequenceType.Animation:
                        var attAnim = new AttractAnimation() { Name = "Animation" };                        
                        seq = new Sequence() { AttractAnimation = attAnim };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.AttractAnimation);
                        break;
                    case AttractSequenceType.HighScores:
                        var hiScore = new HighScores() { Name = "HighScores" };                        
                        seq = new Sequence() { HighScores = hiScore };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.HighScores);
                        break;
                    case AttractSequenceType.Credits:
                        var credits = new Credits() { Name = "Credits" };  
                        seq = new Sequence() { Credits = credits};                        
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.Credits);
                        break;
                    case AttractSequenceType.MarkupLayer:
                        var markup = new MarkupLayer() { Name = "MarkupLayer" };                        
                        seq = new Sequence() { MarkupLayer = markup };                        
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.MarkupLayer);
                        break;
                    case AttractSequenceType.ScriptedText:
                        var scriptedText = new ScriptedText() { Name = "ScriptedText" };
                        seq = new Sequence() { ScriptedText = scriptedText };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.ScriptedText);                        
                        break;
                    case AttractSequenceType.GroupLayer:
                        var groupLayer = new GroupLayer() { Name = "GroupLayer" };
                        seq = new Sequence() { GroupLayer = groupLayer };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.GroupLayer);
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
                        group.Contents.Add(new Content() { animation_layer = new AttractAnimation() });
                    else if (SelectedGroupLayerType == "1")
                        group.Contents.Add(new Content() { markup_layer = new MarkupLayer() });
                    else if (SelectedGroupLayerType == "2")
                        group.Contents.Add(new Content() { text_layer = new TextLayer() });
                    else if (SelectedGroupLayerType == "3")
                        group.Contents.Add(new Content() { combo_layer = new Combo() });
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
                    this.Sequences.Insert(dropInfo.InsertIndex, dropInfo.Data as SequenceBase);
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
            if (AttractConfig == null)
                AttractConfig = _skeletonGameProvider.AttractConfig;
            else
            {
                //_skeletonGameProvider.AttractConfig.Sequences.Clear();
            }

            //await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
            //{
            //    if (Sequences == null)
            //        Sequences = new System.Collections.ObjectModel.ObservableCollection<SequenceBase>();
            //    else
            //        Sequences.Clear();
            //});            

            if (AttractConfig != null)
            {
                AttractConfig = _skeletonGameProvider.AttractConfig;
                await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                {                    
                    Sequences?.Clear();

                    _skeletonGameAttract.GetAvailableSequences(AttractConfig);

                    Sequences = AttractConfig.Sequences;
                });
            }
        }
    }


}
