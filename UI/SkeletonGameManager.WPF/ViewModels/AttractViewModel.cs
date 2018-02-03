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

namespace SkeletonGameManager.WPF.ViewModels
{
    public class AttractViewModel : SequenceViewModelBase, IDropTarget
    {
        public ISkeletonGameProvider _skeletonGameProvider { get; set; }
        private ISkeletonGameAttract _skeletonGameAttract;

        public DelegateCommand<object> AddNewSequenceCommand { get; set; }
        public DelegateCommand<List<Sequence>> AddToGroupLayerCommand { get; set; }
        
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
                Sequence seq = null;

                switch (seqType)
                {
                    case AttractSequenceType.LastScores:
                        var lastScores = new LastScores() { Name = "LastScores"};
                        seq = new Sequence() { LastScores = lastScores };                                                                        
                        Sequences.Add(seq.LastScores);
                        break;
                    case AttractSequenceType.Combo:
                        var combo = new Combo() { Name = "Combo"};                        
                        seq = new Sequence() { Combo = combo };                        
                        Sequences.Add(seq.Combo);
                        break;
                    case AttractSequenceType.text_layer:
                        var txtLayr = new TextLayer() { Name = "text_layer" };                        
                        seq = new Sequence() { text_layer = txtLayr };                                                
                        Sequences.Add(seq.text_layer);
                        break;
                    case AttractSequenceType.panning_layer:
                        var panning_layer = new PanningLayer() { Name = "panning_layer" };
                        seq = new Sequence() { panning_layer = panning_layer };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.panning_layer);
                        Sequences.Add(seq.panning_layer);
                        break;
                    case AttractSequenceType.RandomText:
                        var rndText = new RandomText() { Name = "RandomText" };                        
                        seq = new Sequence() { RandomText = rndText };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.RandomText);
                        Sequences.Add(seq.RandomText);
                        break;
                    case AttractSequenceType.Animation:
                        var attAnim = new AttractAnimation() { Name = "Animation" };                        
                        seq = new Sequence() { AttractAnimation = attAnim };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.AttractAnimation);
                        Sequences.Add(seq.AttractAnimation);
                        break;
                    case AttractSequenceType.HighScores:
                        var hiScore = new HighScores() { Name = "HighScores" };                        
                        seq = new Sequence() { HighScores = hiScore };
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.HighScores);
                        Sequences.Add(seq.HighScores);
                        break;
                    case AttractSequenceType.Credits:
                        var credits = new Credits() { Name = "Credits" };  
                        seq = new Sequence() { Credits = credits};                        
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.Credits);
                        Sequences.Add(seq.Credits);
                        break;
                    case AttractSequenceType.MarkupLayer:
                        var markup = new MarkupLayer() { Name = "MarkupLayer" };                        
                        seq = new Sequence() { MarkupLayer = markup };                        
                        _skeletonGameProvider.AttractConfig.Sequences.Add(seq.MarkupLayer);
                        Sequences.Add(seq.MarkupLayer);
                        break;
                    default:
                        break;
                }

                if (seq != null)
                {
                    _skeletonGameProvider.AttractConfig.AttractSequences.Add(seq);
                }
            });

            AddToGroupLayerCommand = new DelegateCommand<List<Sequence>>((x) =>
            {
                //Don't do anything if null list
                x?.Add(new Sequence() { LastScores = new LastScores() { Name = "LastScores" }});
            });
        }

        public virtual void DragOver(IDropInfo dropInfo)
        {
            //throw new System.NotImplementedException();
            if (dropInfo.IsSameDragDropContextAsSource)
                dropInfo.Effects = System.Windows.DragDropEffects.Move;
        }

        public virtual void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.IsSameDragDropContextAsSource)
            {
                this.Sequences.RemoveAt(dropInfo.DragInfo.SourceIndex);
                this.Sequences.Insert(dropInfo.InsertIndex, dropInfo.Data as SequenceBase);
            }
            //throw new System.NotImplementedException();
        }

        private AttractYaml attractConfig;
        public AttractYaml AttractConfig
        {
            get { return attractConfig; }
            set { SetProperty(ref attractConfig, value); }
        }

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
