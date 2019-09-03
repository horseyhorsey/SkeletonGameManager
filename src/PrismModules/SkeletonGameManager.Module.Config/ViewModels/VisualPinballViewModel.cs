using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGameManager.Base;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using static SkeletonGameManager.Base.Events;

namespace SkeletonGameManager.Module.Config.ViewModels
{
    public class VisualPinballViewModel : SkeletonTabViewModel
    {
        private IVisualPinball _visualPinball;
        private IVpGameMapper _vpGameMapper;
        #region Commands
        public DelegateCommand<string> LaunchVpTableCommand { get; set; }
        public DelegateCommand SaveVpConfigCommand { get; set; }  
        #endregion

        public VisualPinballViewModel(IEventAggregator eventAggregator, ILoggerFacade loggerFacade, IVisualPinball visualPinball, IVpGameMapper vpGameMapper) : 
            base(eventAggregator, loggerFacade)
        {
            Title = "Visual Pinball";
            _visualPinball = visualPinball;
            _vpGameMapper = vpGameMapper;

            VpExecutable = ConfigurationManager.AppSettings["visualPinball"];

            SaveCommand = new DelegateCommand(OnSaveMapping);
            SaveVpConfigCommand = new DelegateCommand(OnSaveVpConfig);
            LaunchVpTableCommand = new DelegateCommand<string>(OnLaunchVp);
            VpGameMaps = new ObservableCollection<VpGameMap>(_vpGameMapper.GetMappings());
        }

        private void OnLaunchVp(string tablePath)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(tablePath))
                    _visualPinball.LoadTable(tablePath);
            }
            catch (Exception ex)
            {
                Log(ex.Message, Category.Exception);
                _eventAggregator.GetEvent<ErrorMessageEvent>().Publish($"Failed to launch table. {ex.Message}");
            }
        }

        private void OnSaveVpConfig()
        {
            var fd = new OpenFileDialog();
            fd.Filter = "visual pinball exe files(*.exe)|*.exe"; ;
            var result = fd.ShowDialog();
            if (result.HasValue && result.Value)
            {
                VpExecutable = fd.FileName;                
                ConfigurationManager.AppSettings.Set("visualPinball", VpExecutable);
                _visualPinball.VpExecutable = VpExecutable;
            }
        }

        private void OnSaveMapping()
        {
            try
            {
                _vpGameMapper.SaveMappings(VpGameMaps.AsEnumerable());
            }
            catch (Exception ex)
            {
                Log(ex.Message, Category.Exception);
                Log(ex.InnerException?.Message, Category.Exception);
                _eventAggregator.GetEvent<ErrorMessageEvent>().Publish($"Failed to save vp_game_map. {ex.Message}");
            }            
        }

        #region Properties
        private string _vpExecutable;
        public string VpExecutable
        {
            get { return _vpExecutable; }
            set { SetProperty(ref _vpExecutable, value); }
        }
        private string _vpTable;
        public string VpTable
        {
            get { return _vpTable; }
            set { SetProperty(ref _vpTable, value); }
        }

        private ObservableCollection<VpGameMap> _vpGameMaps;
        public ObservableCollection<VpGameMap> VpGameMaps
        {
            get { return _vpGameMaps; }
            set { SetProperty(ref _vpGameMaps, value); }
        }

        
        #endregion
    }
}
