using SkeletonGame.Models;
using System.Windows;
using System.Windows.Controls;

namespace SkeletonGameManager.WPF.Selectors
{
    public class AttractSequenceSelector : DataTemplateSelector
    {
        public static AttractSequenceType AttractSequenceType { get; set; }

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
            
            var element = container as FrameworkElement;

            var sequenceItem = item as SequenceBase;

            var type = sequenceItem.GetType();

            if (type == typeof(LastScores))
                _template = element.FindResource("SequenceTemplate") as DataTemplate;

            return _template;
        }
    }
}
