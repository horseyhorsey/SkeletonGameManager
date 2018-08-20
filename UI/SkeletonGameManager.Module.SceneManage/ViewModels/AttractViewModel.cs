using Prism.Events;
using SkeletonGame.Models;
using SkeletonGame.Engine;
using System.Threading.Tasks;
using Prism.Commands;
using System.Windows.Threading;
using GongSolutions.Wpf.DragDrop;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using SkeletonGame.Models.Layers;
using System.Windows;
using SkeletonGameManager.Base;
using static SkeletonGameManager.Base.Events;
using Prism.Logging;
using System.IO;

namespace SkeletonGameManager.Module.SceneManage.ViewModels
{
    public class AttractViewModel : SequenceViewModelBase, IDropTarget
    {
        #region Fields
        public ISkeletonGameProvider _skeletonGameProvider { get; set; }
        private ISkeletonGameAttract _skeletonGameAttract; 
        #endregion

        #region Commands
        public DelegateCommand<object> AddNewSequenceCommand { get; set; }
        public DelegateCommand<object> DuplicateSequenceCommand { get; set; }
        public DelegateCommand<object> AddLayerCommand { get; set; }
        public DelegateCommand SaveAttractCommand { get; set; }
        #endregion

        //TODO: Add back in sequences file..Add option to create other sequence files. Duplicate layers

        #region Constructors
        public AttractViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
            Title = "Sequences";

            _skeletonGameProvider = skeletonGameProvider;
            _skeletonGameAttract = new SkeletonGameAttract();

            Sequences = new ObservableCollection<SequenceItemViewModel>();
            Sequences.CollectionChanged += Sequences_CollectionChanged;
            SequenceYamls.CollectionChanged += SequenceYamls_CollectionChanged;

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x => await OnLoadYamlFilesChanged());

            SaveAttractCommand = new DelegateCommand(() =>
            {
                OnSaveAttract();
            });

            DuplicateSequenceCommand = new DelegateCommand<object>((x) =>
            {
                OnDuplicateSequence(x);
            });

            AddNewSequenceCommand = new DelegateCommand<object>((x) =>
            {
                OnAddSequence(x);
            });

            AddLayerCommand = new DelegateCommand<object>((x) =>
            {
                OnAddLayer(x);

            });
        }
        #endregion

        #region Properties
        private SequenceYaml attractConfig;
        public SequenceYaml AttractConfig
        {
            get { return attractConfig; }
            set { SetProperty(ref attractConfig, value); }
        }

        private ObservableCollection<SequenceYamlItemViewModel> sequenceYamls = new ObservableCollection<SequenceYamlItemViewModel>();
        public ObservableCollection<SequenceYamlItemViewModel> SequenceYamls
        {
            get { return sequenceYamls; }
            set { SetProperty(ref sequenceYamls, value); }
        }

        private string selectedGroupLayerType;
        public string SelectedGroupLayerType
        {
            get { return selectedGroupLayerType; }
            set { SetProperty(ref selectedGroupLayerType, value); }
        }

        private SequenceYamlItemViewModel selectedSequenceFile;
        public SequenceYamlItemViewModel SelectedSequenceFile
        {
            get { return selectedSequenceFile; }
            set
            {
                SetProperty(ref selectedSequenceFile, value);
                OnUpdateSequenceYamlItem();
            }
        }
        #endregion

        #region Public Methods
        public Sequence CreateSequenceFromBase(SequenceBase sequenceBase)
        {
            Sequence seq = new Sequence();

            switch (sequenceBase.SeqType)
            {
                case SequenceType.Animation:
                    seq.AttractAnimation = (AttractAnimation)sequenceBase;
                    break;
                case SequenceType.AnimationLayer:
                    seq.animation_layer = (AttractAnimation)sequenceBase;
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

        /// <summary>
        /// Called when [load yaml files changed]. Gets available sequences from the attract file.
        /// </summary>
        /// <returns></returns>
        public async override Task OnLoadYamlFilesChanged()
        {
            //Assign the attract config
            if (AttractConfig == null)
                AttractConfig = _skeletonGameProvider.AttractConfig;

            _skeletonGameProvider.SequenceYamls.Clear();
            SequenceYamls?.Clear();

            if (AttractConfig != null)
            {
                //Assign the attract config
                AttractConfig = _skeletonGameProvider.AttractConfig;

                //add attract file to provider                
                var attractFile = _skeletonGameProvider.GameFolder + @"\config\attract.yaml";
                var defaultSequenceFile = _skeletonGameProvider.GameFolder + @"\config\sequences.yaml";
                _skeletonGameProvider.SequenceYamls.Add(attractFile);
                _skeletonGameProvider.SequenceYamls.Add(defaultSequenceFile);

                //Assign from all files found in config/sequences
                foreach (var item in _skeletonGameProvider.SequenceYamls)
                {
                    bool seqAdded = false;

                    try
                    {
                        if (!File.Exists(item))
                            throw new FileNotFoundException(item);

                        SequenceYamls.Add(new SequenceYamlItemViewModel(item, _skeletonGameProvider.GetSequence(item)));

                        seqAdded = true;
                        _skeletonGameAttract.GetAvailableSequences(SequenceYamls.Last().SequenceYaml);
                    }
                    catch (Exception ex)
                    {
                        if (seqAdded)
                            SequenceYamls.Remove(SequenceYamls.Last());

                        Log($"Error deserializing sequence file. {item}");
                        _eventAggregator.GetEvent<ErrorMessageEvent>().Publish($"Sequences loading error. {ex.Message}");
                    }
                }

                this.SelectedSequenceFile = SequenceYamls[0];

            }
        }
        #endregion

        #region DragDrop Handlers
        public void DragOver(IDropInfo dropInfo)
        {
            SequenceItemViewModel sourceItem = dropInfo.Data as SequenceItemViewModel;
            SequenceItemViewModel targetItem = dropInfo.TargetItem as SequenceItemViewModel;

            if (sourceItem != null)
            {                
                dropInfo.Effects = DragDropEffects.Move;
            }
        }

        /// <summary>
        /// Performs a drag drop on the Sequences. User can then order the sequences.
        /// </summary>
        /// <param name="dropInfo">Information about the drop.</param>
        public void Drop(IDropInfo dropInfo)
        {
            SequenceItemViewModel sourceItem = dropInfo.Data as SequenceItemViewModel;
            SequenceItemViewModel targetItem = dropInfo.TargetItem as SequenceItemViewModel;

            var srcIndex = dropInfo.DragInfo.SourceIndex;
            var destIndex = dropInfo.InsertIndex -1;

            //Remove and insert the vm item.
            this.Sequences.RemoveAt(srcIndex);

            if (destIndex < 0)
                destIndex = 0;
            else if(destIndex > this.Sequences.Count - 1)
                destIndex = Sequences.Count;

            this.Sequences.Insert(destIndex, sourceItem);
        }

        #endregion

        #region Private Methods

        private void OnAddLayer(object x)
        {
            GroupLayer group = x as GroupLayer;

            if (group != null)
            {
                //Get enum from selectedIndex of combobox
                SequenceType seqType = (SequenceType)Convert.ToInt32(SelectedGroupLayerType);

                //Add layer depending if we got type or not
                switch (seqType)
                {
                    case SequenceType.Animation:
                        group.Contents.Add(new Content()
                        {
                            animation_layer = new AttractAnimation(),
                            SeqType = seqType,
                            SequenceName = seqType + "SequenceStyle"
                        });
                        break;
                    case SequenceType.Combo:
                        break;
                    case SequenceType.Credits:
                        break;
                    case SequenceType.GroupLayer:
                        break;
                    case SequenceType.HighScores:
                        break;
                    case SequenceType.LastScores:
                        group.Contents.Add(new Content()
                        {
                            last_scores = new LastScores(),
                            SeqType = seqType,
                            SequenceName = seqType + "SequenceStyle"
                        });
                        break;
                    case SequenceType.MarkupLayer:
                        group.Contents.Add(new Content()
                        {
                            markup_layer = new MarkupLayer(),
                            SeqType = seqType,
                            SequenceName = seqType + "SequenceStyle"
                        });
                        break;
                    case SequenceType.PanningLayer:
                        break;
                    case SequenceType.RandomText:
                        break;
                    case SequenceType.ScriptedText:
                        group.Contents.Add(new Content()
                        {
                            scripted_text_layer = new ScriptedText(),
                            SeqType = seqType,
                            SequenceName = seqType + "SequenceStyle"
                        });
                        break;
                    case SequenceType.TextLayer:
                        group.Contents.Add(new Content()
                        {
                            text_layer = new TextLayer(),
                            SeqType = seqType,
                            SequenceName = seqType + "SequenceStyle"
                        });
                        break;
                    case SequenceType.MoveLayer:
                        group.Contents.Add(new Content()
                        {
                            move_layer = new MoveLayer(),
                            SeqType = seqType,
                            SequenceName = seqType + "SequenceStyle"
                        });
                        break;
                    case SequenceType.ParticleLayer:
                        group.Contents.Add(new Content()
                        {
                            particle_layer = new ParticleLayer()
                            {
                                Emitters = new System.Collections.Generic.List<ParticleEmitter>(){
                                    new ParticleEmitter()}
                            },
                            SeqType = seqType,
                            SequenceName = seqType + "SequenceStyle"
                        });
                        break;
                    default:
                        break;
                }

            }
        }

        private void OnAddSequence(object x)
        {
            if (x == null) return;

            var seqType = (SequenceType)x;
            Sequence seq = null;

            switch (seqType)
            {
                case SequenceType.LastScores:
                    var lastScores = new LastScores() { SequenceName = "LastScores", duration = 3.0m };
                    seq = new Sequence() { LastScores = lastScores };
                    this.SelectedSequenceFile.SequenceYaml.Sequences.Add(seq.LastScores);
                    Sequences.Add(new SequenceItemViewModel(seq.LastScores));
                    break;
                case SequenceType.Combo:
                    var combo = new Combo() { SequenceName = "Combo", duration = 3.0m };
                    seq = new Sequence() { Combo = combo };
                    this.SelectedSequenceFile.SequenceYaml.Sequences.Add(seq.Combo);
                    Sequences.Add(new SequenceItemViewModel(seq.Combo));
                    break;
                case SequenceType.TextLayer:
                    var txtLayr = new TextLayer() { SequenceName = "TextLayer", duration = 3.0m };
                    seq = new Sequence() { text_layer = txtLayr };
                    this.SelectedSequenceFile.SequenceYaml.Sequences.Add(seq.text_layer);
                    Sequences.Add(new SequenceItemViewModel(seq.text_layer));
                    break;
                case SequenceType.PanningLayer:
                    var panning_layer = new PanningLayer() { SequenceName = "panning_layer", duration = 3.0m };
                    seq = new Sequence() { panning_layer = panning_layer };
                    this.SelectedSequenceFile.SequenceYaml.Sequences.Add(seq.panning_layer);
                    Sequences.Add(new SequenceItemViewModel(seq.panning_layer));
                    break;
                case SequenceType.RandomText:
                    var rndText = new RandomText() { SequenceName = "RandomText", duration = 3.0m };
                    seq = new Sequence() { RandomText = rndText };
                    this.SelectedSequenceFile.SequenceYaml.Sequences.Add(seq.RandomText);
                    Sequences.Add(new SequenceItemViewModel(seq.RandomText));
                    break;
                case SequenceType.Animation:
                    var attAnim = new AttractAnimation() { AnimName = "Animation", duration = 3.0m };
                    seq = new Sequence() { AttractAnimation = attAnim };
                    this.SelectedSequenceFile.SequenceYaml.Sequences.Add(seq.AttractAnimation);
                    Sequences.Add(new SequenceItemViewModel(seq.AttractAnimation));
                    break;
                case SequenceType.HighScores:
                    var hiScore = new HighScores() { SequenceName = "HighScores", duration = 1.0m };
                    seq = new Sequence() { HighScores = hiScore };
                    this.SelectedSequenceFile.SequenceYaml.Sequences.Add(seq.HighScores);
                    Sequences.Add(new SequenceItemViewModel(seq.HighScores));
                    break;
                case SequenceType.Credits:
                    var credits = new Credits() { SequenceName = "Credits", duration = 3.0m };
                    seq = new Sequence() { Credits = credits };
                    this.SelectedSequenceFile.SequenceYaml.Sequences.Add(seq.Credits);
                    Sequences.Add(new SequenceItemViewModel(seq.Credits));
                    break;
                case SequenceType.MarkupLayer:
                    var markup = new MarkupLayer() { SequenceName = "MarkupLayer", duration = 3.0m };
                    seq = new Sequence() { MarkupLayer = markup };
                    this.SelectedSequenceFile.SequenceYaml.Sequences.Add(seq.MarkupLayer);
                    Sequences.Add(new SequenceItemViewModel(seq.MarkupLayer));
                    break;
                case SequenceType.ScriptedText:
                    var scriptedText = new ScriptedText() { Name = "ScriptedText", duration = 3.0m };
                    seq = new Sequence() { ScriptedText = scriptedText };
                    this.SelectedSequenceFile.SequenceYaml.Sequences.Add(seq.ScriptedText);
                    Sequences.Add(new SequenceItemViewModel(seq.ScriptedText));
                    break;
                case SequenceType.GroupLayer:
                    var groupLayer = new GroupLayer() { Name = "GroupLayer", duration = 3.0m };
                    seq = new Sequence() { GroupLayer = groupLayer };
                    this.SelectedSequenceFile.SequenceYaml.Sequences.Add(seq.GroupLayer);
                    Sequences.Add(new SequenceItemViewModel(seq.GroupLayer));
                    break;
                case SequenceType.MoveLayer:
                    var moveLayer = new MoveLayer() { SequenceName = "MoveLayer", duration = 3.0m };
                    seq = new Sequence() { MoveLayer = moveLayer };
                    this.SelectedSequenceFile.SequenceYaml.Sequences.Add(seq.MoveLayer);
                    Sequences.Add(new SequenceItemViewModel(seq.MoveLayer));
                    break;
                default:
                    break;
            }
        }

        private void OnDuplicateSequence(object x)
        {
            var seqbase = x as SequenceBase;

            if (seqbase != null)
            {
                if (seqbase.SeqType == SequenceType.GroupLayer)
                {
                    var groupBase = (GroupLayer)seqbase;

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
        }

        private void OnSaveAttract()
        {
            SelectedSequenceFile.SequenceYaml.Sequences.Clear();
            SelectedSequenceFile.SequenceYaml.AttractSequences.Clear();

            foreach (var item in Sequences)
            {
                SelectedSequenceFile.SequenceYaml.AttractSequences.Add(new Sequence(item.Sequence));
            }

            //Save sequence to yaml
            _skeletonGameProvider.SaveSequenceFile(SelectedSequenceFile.SequenceYaml, SelectedSequenceFile.Filename);
        }

        private async void OnUpdateSequenceYamlItem()
        {
            if (SelectedSequenceFile == null)
            {
                Log("Sequence File Doesn't exist", Category.Warn);
                return;
            }

            Log("Updating sequences.");

            try
            {
                //Clear sequences and convert the attract configs values to sequence item view models
                await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                {
                    Sequences?.Clear();
                    _skeletonGameAttract.GetAvailableSequences(SelectedSequenceFile.SequenceYaml);

                    foreach (var sequence in SelectedSequenceFile.SequenceYaml.Sequences)
                    {
                        Sequences.Add(new SequenceItemViewModel(sequence));
                    }
                });
            }
            catch (Exception ex)
            {
                Log(ex.Message, Category.Exception);
                throw;
            }
        }

        private void SequenceYamls_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }

        private void Sequences_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                //Sequences.Insert(e.NewStartingIndex, (SequenceItemViewModel)e.NewItems[0]);
                try
                {
                    this.SelectedSequenceFile.SequenceYaml.Sequences.RemoveAt(e.OldStartingIndex);
                    this.SelectedSequenceFile.SequenceYaml.AttractSequences.RemoveAt(e.OldStartingIndex);
                }
                catch { }
            }
        }
        #endregion
    }
}
