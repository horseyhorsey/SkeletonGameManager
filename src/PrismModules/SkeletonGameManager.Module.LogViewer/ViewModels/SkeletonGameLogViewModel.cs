using Prism.Events;
using Prism.Logging;
using SkeletonGameManager.Base;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using static SkeletonGameManager.Base.Events;

namespace SkeletonGameManager.Module.LogViewer.ViewModels
{
    public class SkeletonGameLogViewModel : LogViewModelBase
    {
        #region Fields
        private ISkeletonGameProvider _skeletonGameProvider; 
        #endregion

        #region Constructors
        public SkeletonGameLogViewModel(ISkeletonGameProvider skeletonGameProvider, IEventAggregator eventAggregator, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
            Title = "Game logs";
            this.LogLines = new System.Collections.ObjectModel.ObservableCollection<LogViewer.Log>();
            LogLinesCollectionView = new ListCollectionView(this.LogLines);

            _skeletonGameProvider = skeletonGameProvider;

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x => await OnLoadYamlFilesChanged());

            _eventAggregator.GetEvent<OnGameEndedEvent>().Subscribe(OnGameEnded);
        } 
        #endregion

        #region Properties
        public ICollectionView LogLinesCollectionView { get; }

        private bool _caseSensitiveFilter = false;
        /// <summary>
        /// Gets or sets a value indicating whether using a case sensitive filter search
        /// </summary>
        public bool CaseSensitiveFilter
        {
            get { return _caseSensitiveFilter; }
            set { SetProperty(ref _caseSensitiveFilter, value); }
        }

        private string _logLineFilterText;
        /// <summary>
        /// Gets or sets the log line filter text.
        /// </summary>
        public string LogLineFilterText
        {
            get { return _logLineFilterText; }
            set
            {
                var set = SetProperty(ref _logLineFilterText, value);
                if (set)
                {
                    UpdateLogLinesFilter();
                }
            }
        }

        #endregion

        #region Public methods
        /// <summary>
        /// Called when /[load yaml files changed]. Clears the logLines, populates logs from directory.
        /// </summary>
        /// <returns></returns>
        public override Task OnLoadYamlFilesChanged()
        {
            this.LogLines?.Clear();
            LogPath = Path.Combine(_skeletonGameProvider.GameFolder, "logs");
            GetLogs();

            return Task.CompletedTask;
        } 
        #endregion

        #region Private Methods
        protected override void GetLogs()
        {
            Log("Populating logs");

            if (!Directory.Exists(LogPath))
                Directory.CreateDirectory(LogPath);

            LogFiles = new System.Collections.ObjectModel
                .ObservableCollection<string>(Directory.EnumerateFiles(LogPath, "*.log").Select(x => Path.GetFileName(x)));
        }

        private void OnGameEnded(bool obj)
        {
            try
            {
                Log("Updating SkeletonGame logs after launching game");
                this.LogFiles.Clear();
                this.GetLogs();
            }
            catch (Exception ex)
            {
                Log($"Error refreshing logs. {ex.Message}", Category.Exception);
            }
        }

        protected override void UpdateFromSelected()
        {
            try
            {
                this.LogLines?.Clear();
                var content = File.ReadLines(Path.Combine(this.LogPath, this.SelectedLogFile));
                foreach (var line in content)
                {
                    LogLines.Add(new LogViewer.Log(line));
                }
            }
            catch (Exception ex)
            {
                Log("Read log failed" + ex.Message, Category.Warn);

                LogLines?.Clear();
            }
        }

        private void UpdateLogLinesFilter()
        {
            this.LogLinesCollectionView.Filter = (f) =>
            {
                string line = string.Empty;
                if (CaseSensitiveFilter)
                    line = (f as Log).Line;
                else
                    line = (f as Log).Line.ToUpper();

                if (CaseSensitiveFilter)
                {
                    if (line.Contains(LogLineFilterText))
                        return true;
                    else
                        return false;
                }
                else
                {
                    if (line.Contains(LogLineFilterText.ToUpper()))
                        return true;
                    else
                        return false;
                }
            };

            this.LogLinesCollectionView.Refresh();
        } 
        #endregion
    }
}
