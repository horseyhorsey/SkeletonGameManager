using Prism.Events;
using Prism.Logging;
using SkeletonGameManager.Base;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static SkeletonGameManager.Base.Events;

namespace SkeletonGameManager.Module.LogViewer.ViewModels
{
    public class SkeletonGameLogViewModel : LogViewModelBase
    {
        private ISkeletonGameProvider _skeletonGameProvider;

        public SkeletonGameLogViewModel(ISkeletonGameProvider skeletonGameProvider, IEventAggregator eventAggregator, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
            Title = "Game logs";
            this.LogLines = new System.Collections.ObjectModel.ObservableCollection<LogViewer.Log>();

            _skeletonGameProvider = skeletonGameProvider;

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x => await OnLoadYamlFilesChanged());
        }

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

        protected override void GetLogs()
        {
            Log("Populating logs");

            if (!Directory.Exists(LogPath))
                Directory.CreateDirectory(LogPath);

            LogFiles = new System.Collections.ObjectModel
                .ObservableCollection<string>(Directory.EnumerateFiles(LogPath, "*.log").Select(x => Path.GetFileName(x)));
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
    }
}
