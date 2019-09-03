using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using SkeletonGameManager.Base;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SkeletonGameManager.Module.LogViewer.ViewModels
{
    public class LogViewModelBase : SkeletonTabViewModel
    {
        #region Commands
        public ICommand SelectedLogChanged { get; set; } 
        #endregion

        public LogViewModelBase(IEventAggregator eventAggregator, ILoggerFacade loggerFacade) : base(eventAggregator, loggerFacade)
        {
            SelectedLogChanged = new DelegateCommand<object>(OnSelectedLogChanged);
        }

        #region Properties
        public string LogPath { get; set; }
        public string SelectedLogFile { get; set; } = string.Empty;

        private ObservableCollection<string> _logs;
        public ObservableCollection<string> LogFiles
        {
            get { return _logs; }
            set { SetProperty(ref _logs, value); }
        }

        private ObservableCollection<Log> _logLines;
        public ObservableCollection<Log> LogLines
        {
            get { return _logLines; }
            set { SetProperty(ref _logLines, value); }
        }
        #endregion

        #region Virtual Methods
        protected virtual void GetLogs()
        {
            throw new NotImplementedException();
        }

        protected virtual void UpdateFromSelected()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods
        private void OnSelectedLogChanged(object x)
        {
            try
            {
                LogLines?.Clear();
                SelectedLogFile = (x as object[])[0].ToString();
                UpdateFromSelected();
            }
            catch (Exception ex)
            {
                Log(ex.Message, Category.Warn);
            }
        } 
        #endregion
    }
}
