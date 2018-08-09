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

            GetLogs();
        }

        protected override void GetLogs()
        {
            Log("Populating logs");
            LogFiles = new System.Collections.ObjectModel.ObservableCollection<string>(Directory.EnumerateFiles(LogPath, "*.log"));
        }

        protected override void UpdateFromSelected()
        {
            try
            {
                var content = File.ReadLines(this.SelectedLogFile);
                this.LogLines = new System.Collections.ObjectModel.ObservableCollection<string>(content);
            }
            catch (Exception ex)
            {
                Log("Read log failed" + ex.Message, Category.Warn);

                LogLines?.Clear();
            }
        }
    }
}
