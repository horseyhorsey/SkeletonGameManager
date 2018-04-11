using Prism.Commands;
using Prism.Events;
using SkeletonGame.Models.Data;
using SkeletonGameManager.WPF.Events;
using SkeletonGameManager.WPF.Providers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class TrophyDataViewModel : SkeletonGameManagerViewModelBase
    {

        #region Commands
        public ICommand CreateTrophyCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        #endregion

        #region Fields
        public ISkeletonGameProvider _skeletonGameProvider { get; set; }    
        #endregion

        #region Constructors
        public TrophyDataViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider) : base(eventAggregator)
        {
            _skeletonGameProvider = skeletonGameProvider;            

            _eventAggregator.GetEvent<LoadYamlFilesChanged>().Subscribe(async x => await OnLoadYamlFilesChanged());

            CreateTrophyCommand = new DelegateCommand(() =>
            {
                OnCreateTrophy();
            });

            SaveCommand = new DelegateCommand(() =>
            {
                _skeletonGameProvider.SaveTrophyData(TrophyData);
            });            
        }
        #endregion

        #region Public Methods
        public override async Task OnLoadYamlFilesChanged()
        {
            TrophyData = _skeletonGameProvider.TrophyData;

            //UiIcon
            foreach ( var trophy in TrophyData.Trophys.Values)
            {
                //Assign the default trophy icon if value is empty for an icon
                var iconKey = string.Empty;
                if (string.IsNullOrWhiteSpace(trophy.Icon))
                    iconKey = "trophy";
                else
                    iconKey = trophy.Icon;

                //Get the animation from the assets
                var trophyAnim = _skeletonGameProvider.AssetsConfig.Animations.FirstOrDefault(x => x.Key == iconKey);

                //Create a URI to view in the UI
                if (trophyAnim != null)
                {
                    trophy.UiIcon = new Uri(Path.Combine(_skeletonGameProvider.GameFolder, _skeletonGameProvider.GameConfig.DmdPath, trophyAnim.File), UriKind.RelativeOrAbsolute).AbsolutePath;
                }                
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Called when [create trophy]. Adds the trophy to the disctionary if key doesn't exist
        /// </summary>
        private void OnCreateTrophy()
        {
            if (!TrophyData.Trophys.ContainsKey(this.NewTrophyName))
            {
                TrophyData.Trophys.Add(this.NewTrophyName, new Trophy
                {
                    Description = this.NewTrophyDesc                    
                });
            }            
        }

        #endregion

        #region Properties

        private TrophyData trophyData;
        public TrophyData TrophyData
        {
            get { return trophyData; }
            set { SetProperty(ref trophyData, value); }
        }

        private string newTrophyName;
        public string NewTrophyName
        {
            get { return newTrophyName; }
            set { SetProperty(ref newTrophyName, value); }
        }

        private string newTrophyDesc;
        public string NewTrophyDesc
        {
            get { return newTrophyDesc; }
            set { SetProperty(ref newTrophyDesc, value); }
        } 
        #endregion
    }
}
