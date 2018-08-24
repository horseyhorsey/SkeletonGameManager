using Prism.Interactivity.InteractionRequest;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SkeletonGameManager.Module.SceneManage.Views
{
    /// <summary>
    /// Interaction logic for YamlStructDialog.xaml
    /// </summary>
    public partial class YamlStructDialog : UserControl, IInteractionRequestAware
    {
        public YamlStructDialog()
        {
            InitializeComponent();
        }

        public Action FinishInteraction { get; set; }
        public INotification Notification { get; set; }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(YamlTextBlock.Text);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            FinishInteraction?.Invoke();
        }
    }
}
