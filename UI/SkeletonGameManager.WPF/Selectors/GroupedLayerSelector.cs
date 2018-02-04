using SkeletonGame.Models;
using System.Windows;
using System.Windows.Controls;

namespace SkeletonGameManager.WPF.Selectors
{
    public class GroupedLayerSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate _template = null;

            if (item == null) return _template;
            var element = container as FrameworkElement;

            var groupedContent = item as Content;

            if (groupedContent != null)
            {
                if (groupedContent.animation_layer != null)
                    _template = element.FindResource("Content_AnimationLayer") as DataTemplate;
                else if (groupedContent.markup_layer != null)
                    _template = element.FindResource("Content_MarkupLayer") as DataTemplate;
                else if (groupedContent.text_layer != null)
                    _template = element.FindResource("Content_TextLayer") as DataTemplate;
                else if (groupedContent.combo_layer != null)
                    _template = element.FindResource("Content_Combo") as DataTemplate;
            }
            
            return _template;

            //return base.SelectTemplate(item, container);
        }
    }
}
