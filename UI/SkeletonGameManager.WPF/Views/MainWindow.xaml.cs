using System.Windows;

namespace SkeletonGameManager.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Style _style = null;
            
            _style = (Style)Resources["ChromeWindow"];
            this.Style = _style;
        }
    }
}
