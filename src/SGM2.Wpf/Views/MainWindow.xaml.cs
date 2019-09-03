using System.Windows;

namespace SGM2.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Style _style = null;

            _style = (Style)Resources["ChromeWindow"];
            this.Style = _style;
        }

        private void Close_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            var result = MessageBox.Show("Sure you want to exit?", "Closing...", MessageBoxButton.YesNoCancel);

            if (result == MessageBoxResult.Yes)
                this.Close();
            else
                e.Handled = true;
        }
    }
}
