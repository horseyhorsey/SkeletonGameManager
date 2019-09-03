using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SkeletonGameManager.Module.LogViewer.ViewModels
{
    public class AppLogViewModel : LogViewModelBase
    {
        public AppLogViewModel(IEventAggregator eventAggregator, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
            Title = "Sgm Log";

            LogPath = Path.Combine(Environment.CurrentDirectory, "logs");
            this.LogLines = new System.Collections.ObjectModel.ObservableCollection<LogViewer.Log>();

            GetLogs();
        }

        protected override void GetLogs()
        {
            Log("Populating logs");
            if (!Directory.Exists(LogPath))
                Directory.CreateDirectory(LogPath);

            LogFiles = new System.Collections.ObjectModel.ObservableCollection<string>(
                Directory.EnumerateFiles(LogPath, "*.log").Select(x => Path.GetFileName(x)));
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
