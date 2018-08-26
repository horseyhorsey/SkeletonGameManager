using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SkeletonGame.Engine;
using SkeletonGame.Models.Machine;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using static SkeletonGameManager.Base.Events;

namespace SkeletonGameManager.Module.Menus.ViewModels
{
    public class CreateNewGameWindowViewModel : BindableBase
    {
        private IEventAggregator _eventAggregator;
        private WebClient _client;

        public DelegateCommand CreateNewSkeletonGame { get; set; }

        public CreateNewGameWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
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

        private string selectedTemplate = "EmptyGameVP";
        public string SelectedTemplate
        {
            get { return selectedTemplate; }
            set { SetProperty(ref selectedTemplate, value); }
        }

        private MachineType selectedMachineType;
        public MachineType SelectedMachineType
        {
            get { return selectedMachineType; }
            set { SetProperty(ref selectedMachineType, value); }
        }

        private int ballInMachine = 4;
        public int BallsInMachine
        {
            get { return ballInMachine; }
            set { SetProperty(ref ballInMachine, value); }
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
                    _eventAggregator.GetEvent<ErrorMessageEvent>().Publish($"Game folder already exists!..{gamePath}");
                    return;
                }

                //Create temp folder for the download. Ask user if they want to download again if already exists
                Directory.CreateDirectory("Temp");
                var fileDownload = Path.Combine("Temp", "dev.zip");

                if (File.Exists(fileDownload))
                {
                    //TODO: Add confirm dialogs
                    var result = MessageBox.Show("Download already exists, download and overwrite?", "Download", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        DownloadSkeletonGame(fileDownload);
                    }
                }
                else
                {
                    DownloadSkeletonGame(fileDownload);
                }

                //Create game from download.
                var creator = new CreateSkeletonGame();
                creator.CreateNewGameEntry(GameName, SelectedTemplate, ProcPath, fileDownload);
                
                //Machine file to copy and use as default
                var machineYaml = Path.Combine(gamePath, "config", "machine.yaml");
                //Change default machine.yaml if using PDB, copy backup, delete old
                if (SelectedMachineType == MachineType.PDB)
                {
                    File.Copy(machineYaml, Path.Combine(gamePath, "config", "machine_standard.yaml"));
                    File.Delete(machineYaml);
                    machineYaml = Path.Combine(gamePath, "config", "PDB_Sample.yaml");
                }
                
                //Add the machinetype and balls in machine
                var machineConfigString = File.ReadAllText(machineYaml);
                machineConfigString = machineConfigString.Replace("  machineType: wpc", $" machineType: {SelectedMachineType.ToString().ToLower()}");
                machineConfigString = machineConfigString.Replace("  numBalls: 6", $" numBalls: {BallsInMachine}");

                //Rewrite the text back to machine.yaml
                machineYaml = Path.Combine(gamePath, "config", "machine.yaml");
                File.WriteAllText(machineYaml, machineConfigString);

                _eventAggregator.GetEvent<ErrorMessageEvent>().Publish($"New game: {GameName} succesfully created at {gamePath}");
            }
            catch (System.Exception ex)
            {
                _eventAggregator.GetEvent<ErrorMessageEvent>().Publish($"Error creating game {ex.Message}");
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
