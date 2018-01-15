using Prism.Commands;
using Prism.Mvvm;
using SkeletonGameManager.WPF.Providers;
using System.Windows.Forms;
using System.Windows.Input;
using System;
using System.IO;
using System.Threading.Tasks;
using Prism.Events;
using SkeletonGameManager.WPF.Events;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class MainWindowViewModel : SkeletonGameManagerViewModelBase
    {
        #region Fields
        private ISkeletonGameProvider _skeletonGameProvider; 
        #endregion

        #region Commands
        public ICommand SetDirectoryCommand { get; set; }
        public DelegateCommand RefreshObjectsCommand { get; set; }
        #endregion

        #region Constructors

        public MainWindowViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider) : base(eventAggregator)
        {
            _skeletonGameProvider = skeletonGameProvider;            

            SetDirectoryCommand = new DelegateCommand(() => OnSetDirectory());

            RefreshObjectsCommand = new DelegateCommand(async () => await OnRefreshSkeletonGameObjects(), () => IsValidGameFolder());
        }

        #endregion

        #region Properties

        private string gameFolder;
        /// <summary>
        /// Gets or sets the game folder and applies it to the underlying provider
        /// </summary>
        public string GameFolder
        {
            get { return gameFolder; }
            set
            {
                SetProperty(ref gameFolder, value);
                _skeletonGameProvider.GameFolder = value;
                RefreshObjectsCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region Private Methods        

        /// <summary>
        /// Determines whether [is valid game folder] by making sure folder exists and has a valid config.yaml
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is valid game folder]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidGameFolder()
        {
            if (Directory.Exists(GameFolder))
                if (File.Exists(Path.Combine(GameFolder, "config.yaml")))
                    return true;

            return false;
        }

        /// <summary>
        /// Called when [refresh skeleton game objects], loads all yaml files into the provider
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private async Task OnRefreshSkeletonGameObjects()
        {
            try
            {
                await _skeletonGameProvider.LoadYamlEntriesAsync();

                _eventAggregator.GetEvent<LoadYamlFilesChanged>().Publish(null);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Failed loading skeleton game files. {ex.Data["yaml"]} {ex.Message} {ex.InnerException.Message}");
            }
        }

        /// <summary>
        /// Called when /[set directory], sets the current skeleton game folder path
        /// </summary>
        private void OnSetDirectory()
        {
            var dlg = new FolderBrowserDialog();
            dlg.SelectedPath = @"C:\P-ROC";
            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                GameFolder = dlg.SelectedPath;                
            };      
        } 
        #endregion
    }
}
