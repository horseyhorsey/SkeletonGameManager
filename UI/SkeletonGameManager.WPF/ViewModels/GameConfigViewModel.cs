using System;
using Prism.Events;
using Prism.Mvvm;
using SkeletonGameManager.WPF.Events;
using SkeletonGameManager.WPF.Providers;
using SkeletonGame.Models;
using System.Windows.Input;
using Prism.Commands;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class GameConfigViewModel : SkeletonGameManagerViewModelBase
    {        
        private ISkeletonGameProvider _skeletonGameProvider;        

        #region Constructors

        public GameConfigViewModel(IEventAggregator ea, ISkeletonGameProvider skeletonGameProvider) : base(ea)
        {            
            _skeletonGameProvider = skeletonGameProvider;
            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(x => OnLoadYamlFilesChanged());

            SaveCommand = new DelegateCommand(() =>
            {
                _skeletonGameProvider.SaveGameConfig(GameConfigModel);
            },() => GameConfigModel == null ? false : true);
        }

        #endregion

        private GameConfig gameConfig;
        public GameConfig GameConfigModel
        {
            get { return gameConfig; }
            set { SetProperty(ref gameConfig, value); }
        }

        #region Private Methods

        public override Task OnLoadYamlFilesChanged()
        {
            return Task.Run(async () =>
            {
                GameConfigModel = _skeletonGameProvider.GameConfig;

                 await Dispatcher.CurrentDispatcher.InvokeAsync(() =>
                {
                    SaveCommand.RaiseCanExecuteChanged();
                });
                
            });
        }

        #endregion        
    }
}
