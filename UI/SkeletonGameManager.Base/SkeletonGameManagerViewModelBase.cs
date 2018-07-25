using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Threading.Tasks;

namespace SkeletonGameManager.Base
{
    public abstract class SkeletonGameManagerViewModelBase : BindableBase
    {
        protected IEventAggregator _eventAggregator;

        #region Commands
        public DelegateCommand SaveCommand { get; set; }
        #endregion

        public SkeletonGameManagerViewModelBase(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;            
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

        public virtual Task OnLoadYamlFilesChanged()
        {
            return null;
        }

        #endregion
    }
}
