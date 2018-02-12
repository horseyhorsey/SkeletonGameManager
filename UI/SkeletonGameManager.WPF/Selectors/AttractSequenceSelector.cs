using SkeletonGame.Models;
using SkeletonGame.Models.Layers;
using SkeletonGameManager.WPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SkeletonGameManager.WPF.Selectors
{
    public class AttractSequenceSelector : DataTemplateSelector
    {
        public static SequenceType AttractSequenceType { get; set; }

        /// <summary>
        /// Gets a template based on the Sequence type for building attract modes
        /// </summary>
        /// <param name="item">The data object for which to select the template.</param>
        /// <param name="container">The data-bound object.</param>
        /// <returns>
        /// Returns a <see cref="T:System.Windows.DataTemplate" /> or <see langword="null" />. The default value is <see langword="null" />.
        /// </returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate _template = null;

            if (item == null) return _template;                       

            var sequenceItem = item as SequenceItemViewModel;
            if (sequenceItem != null)
            {
                var type = sequenceItem.GetType();
                return GetTemplate(type, container);
            }
            else
            {
                return GetTemplate(item.GetType(), container);
            }
        }

        private DataTemplate GetTemplate(Type type, DependencyObject container)
        {
            var element = container as FrameworkElement;

            if (type == typeof(LastScores))
                return element.FindResource("Sequence_LastScores") as DataTemplate;
            else if (type == typeof(HighScores))
                return element.FindResource("Sequence_HighScores") as DataTemplate;
            else if (type == typeof(Combo))
                return element.FindResource("Sequence_Combo") as DataTemplate;
            else if (type == typeof(TextLayer))
                return element.FindResource("Sequence_TextLayer") as DataTemplate;
            else if (type == typeof(RandomText))
                return element.FindResource("Sequence_RandomText") as DataTemplate;
            else if (type == typeof(AttractAnimation))
                return element.FindResource("Sequence_Animation") as DataTemplate;
            else if (type == typeof(PanningLayer))
                return element.FindResource("Sequence_Panning") as DataTemplate;
            else if (type == typeof(MarkupLayer))
                return element.FindResource("Sequence_Markup") as DataTemplate;
            else if (type == typeof(ScriptedText))
                return element.FindResource("Sequence_ScriptedText") as DataTemplate;
            else if (type == typeof(GroupLayer))
                return element.FindResource("Sequence_GroupLayer") as DataTemplate;
            else if (type == typeof(MoveLayer))
                return element.FindResource("Sequence_MoveLayer") as DataTemplate;

            else
                return null;
        }
    }
}
