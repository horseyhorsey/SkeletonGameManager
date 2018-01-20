using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SkeletonGame.Engine;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class CreateNewGameWindowViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        private WebClient _client;

        public DelegateCommand CreateNewSkeletonGame { get; set; }

        public CreateNewGameWindowViewModel()
        {
            _client = new WebClient();

            CreateNewSkeletonGame = new DelegateCommand(DownloadAndSetupNewGame, () => !string.IsNullOrEmpty(GameName));
        }

        #region Properties
        private string gameName;

        public string GameName
        {
            get { return gameName; }
            set {
                SetProperty(ref gameName, value);

                CreateNewSkeletonGame.RaiseCanExecuteChanged();
            }
        }

        private string procPath = @"C:\P-ROC\Games";
        public string ProcPath
        {
            get { return procPath; }
            set { SetProperty(ref procPath, value); }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Downloads the and setups a new game.
        /// </summary>
        private void DownloadAndSetupNewGame()
        {
            try
            {
                var gamePath = Path.Combine(ProcPath, GameName);
                if (Directory.Exists(gamePath))
                {
                    MessageBox.Show($"Game folder already exists!..{gamePath}");
                    return;
                }

                //Create temp folder for the download. Ask user if they want to download again if already exists
                Directory.CreateDirectory("Temp");
                var fileDownload = Path.Combine("Temp", "dev.zip");

                if (File.Exists(fileDownload))
                {
                    var result = MessageBox.Show("Download already exists, download and overwrite?", "Download", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        DownloadSkeletonGame(fileDownload);
                    }
                }

                //Create game from download.
                var creator = new CreateSkeletonGame();
                creator.CreateNewGameEntry(GameName, ProcPath, fileDownload);

                MessageBox.Show($"New game: {GameName} succesfully created at {gamePath}");                
            }
            catch (System.Exception ex)
            {

                MessageBox.Show($"Error creating game {ex.Message}");
            }
        }

        /// <summary>
        /// Downloads the skeleton game from GitHub
        /// </summary>
        /// <param name="fileDownload">The file download.</param>
        private void DownloadSkeletonGame(string fileDownload)
        {
            _client.DownloadProgressChanged += _client_DownloadProgressChanged;

            //Download dev branch
            _client.DownloadFile(
                @"https://github.com/horseyhorsey/PyProcGameHD-SkeletonGame/archive/dev.zip",
                fileDownload
                );
        }

        private void _client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {

        } 

        #endregion

    }
}
