using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGame.Models.Layers;
using SkeletonGameManager.Base;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SkeletonGameManager.Module.SceneManage.ViewModels
{
    public class SequenceViewModelBase : SkeletonTabViewModel
    {
        private ISkeletonGameSerializer _skeletonGameSerializer;

        public ICommand ShowYamlStructCommand { get; set; }

        public SequenceViewModelBase(ISkeletonGameSerializer skeletonGameSerializer,
            IEventAggregator eventAggregator, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
            _skeletonGameSerializer = skeletonGameSerializer;

            ShowYamlStructCommand = new DelegateCommand<Content>(OnShowYamlStruct);
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

        #region Private Methods

        /// <summary>
        /// Shows user a YamlStruct so can be copied and used as python Dict to Generate Layers from Yaml.
        /// </summary>
        /// <param name="content">The content.</param>
        private void OnShowYamlStruct(Content content)
        {
            string yamlStruct = string.Empty;
            yamlStruct = GenerateYamlStruct(content);
        }

        private string GenerateYamlStruct(Content content)
        {
            string yamlStruct;
            if (content != null)
            {
                var layer = content.GetNotNullContentLayer();
                var vmLayer = new SequenceItemViewModel(layer);
                var yamlStrObj = _skeletonGameSerializer.ConvertToJson(vmLayer);
                yamlStruct = ReplaceType(yamlStrObj, layer.SeqType);
            }
            else
            {
                var yamlObj = _skeletonGameSerializer.ConvertToJson(SelectedSequence);
                yamlStruct = ReplaceType(yamlObj, SelectedSequence.Sequence.SeqType);
            }

            return yamlStruct;
        }

        private string ReplaceType(string yamlStruct, SequenceType seqType)
        {
            switch (seqType)
            {
                case SequenceType.Animation:
                case SequenceType.Combo:
                case SequenceType.HighScores:
                case SequenceType.LastScores:
                case SequenceType.RandomText:
                case SequenceType.ScriptedText:
                    yamlStruct = yamlStruct.Replace("Sequence", seqType.ToString());
                    break;
                case SequenceType.AnimationLayer:
                    yamlStruct = yamlStruct.Replace("Sequence", "animation_layer");
                    break;
                case SequenceType.Credits:
                    yamlStruct = yamlStruct.Replace("Sequence", "credits");
                    break;
                case SequenceType.GroupLayer:
                    yamlStruct = yamlStruct.Replace("Sequence", "group_layer");
                    break;
                case SequenceType.MarkupLayer:
                    yamlStruct = yamlStruct.Replace("Sequence", "markup_layer");
                    break;
                case SequenceType.MoveLayer:
                    yamlStruct = yamlStruct.Replace("Sequence", "move_layer");
                    break;
                case SequenceType.PanningLayer:
                    yamlStruct = yamlStruct.Replace("Sequence", "panning_layer");
                    break;
                case SequenceType.ParticleLayer:
                    yamlStruct = yamlStruct.Replace("Sequence", "particle_layer");
                    break;
                case SequenceType.TextLayer:
                    yamlStruct = yamlStruct.Replace("Sequence", "text_layer");
                    break;
                case SequenceType.ZoomLayer:
                    yamlStruct = yamlStruct.Replace("Sequence", "zoom_layer");
                    break;
                default:
                    break;
            }

            return yamlStruct;
        }

        private void Sequences_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }
        #endregion
    }
}
