using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;
using SkeletonGameManager.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SkeletonGameManager.Module.LogViewer.ViewModels
{
    public class VpLogViewModel : LogViewModelBase
    {
        public VpLogViewModel(ISkeletonGameProvider skeletonGameProvider,
            IEventAggregator eventAggregator, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
            Title = "VP Log";

            LogPath = @"C:\P-ROC\Shared\log.txt";
            this.LogLines = new System.Collections.ObjectModel.ObservableCollection<LogViewer.Log>();

            GetLogs();
        }

        protected override void GetLogs()
        {
            Log("Populating Visual Pinball log");
            if (!File.Exists(LogPath))
            {
                Log($"Visual pinball log not available at {LogPath}", Category.Warn);
                return;
            }

            LogFiles = new System.Collections.ObjectModel.ObservableCollection<string>(new List<string>() { LogPath });
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
