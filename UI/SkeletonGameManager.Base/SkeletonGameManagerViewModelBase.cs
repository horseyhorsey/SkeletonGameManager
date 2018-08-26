using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SkeletonGameManager.Base
{
    public abstract class SkeletonGameManagerViewModelBase : BindableBase
    {
        protected IEventAggregator _eventAggregator;
        protected readonly ILoggerFacade _logger;

        #region Commands
        public DelegateCommand SaveCommand { get; set; }
        #endregion

        public SkeletonGameManagerViewModelBase(IEventAggregator eventAggregator, ILoggerFacade loggerFacade)
        {
            _eventAggregator = eventAggregator;
            _logger = loggerFacade;
        }

        #region Properties
        private string _title = "SkeletonGame Manager";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        #endregion

        #region Public Methods

        public void Log(string message, Category cat = Category.Debug, Priority priority = Priority.None, [CallerMemberName] string caller = "")
        {
            _logger.Log($"{this.GetType().Name} - {caller} | {message}", cat, priority);
        }

        public virtual Task OnLoadYamlFilesChanged()
        {
            return null;
        }

        #endregion
    }
}
