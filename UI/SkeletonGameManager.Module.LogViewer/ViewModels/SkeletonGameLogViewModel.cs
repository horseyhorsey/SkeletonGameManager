using Prism.Events;
using Prism.Logging;
using SkeletonGameManager.Base;
using System;
using System.IO;
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

            _skeletonGameProvider = skeletonGameProvider;

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x => await OnLoadYamlFilesChanged());
        }

        public override Task OnLoadYamlFilesChanged()
        {
            LogPath = Path.Combine(_skeletonGameProvider.GameFolder, "logs");
            GetLogs();

            return Task.CompletedTask;
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
