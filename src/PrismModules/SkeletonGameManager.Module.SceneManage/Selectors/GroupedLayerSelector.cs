using SkeletonGame.Models.Layers;
using System.Windows;
using System.Windows.Controls;

namespace SkeletonGameManager.Module.SceneManage.Selectors
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
                else if (groupedContent.scripted_text_layer != null)
                    _template = element.FindResource("Content_ScriptedText") as DataTemplate;
                else if (groupedContent.group_layer != null)
                    _template = element.FindResource("Content_GroupLayer") as DataTemplate;
                else if (groupedContent.move_layer != null)
                    _template = element.FindResource("Content_MoveLayer") as DataTemplate;
                else if (groupedContent.particle_layer != null)
                    _template = element.FindResource("Content_ParticleEmitter") as DataTemplate;                
            }
            
            return _template;
        }
    }
}
