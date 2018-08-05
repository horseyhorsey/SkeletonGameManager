using Prism.Commands;
using System.Windows.Forms;
using System;
using System.IO;
using System.Threading.Tasks;
using Prism.Events;
using SkeletonGameManager.WPF.Views;
using System.Diagnostics;
using System.Windows;
using SkeletonGame.Engine;
using System.Collections.Generic;
using SkeletonGameManager.Base;
using static SkeletonGameManager.Base.Events;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class MainWindowViewModel : SkeletonGameManagerViewModelBase
    {
        #region Fields
        private ISkeletonGameProvider _skeletonGameProvider;
        private ISkeletonLogger _skeletonLogger;
        #endregion

        #region Commands

        
        #endregion

        #region Constructors

        public MainWindowViewModel(IEventAggregator eventAggregator, ISkeletonGameProvider skeletonGameProvider, ISkeletonLogger skeletonLogger) : base(eventAggregator)
        {
            _skeletonGameProvider = skeletonGameProvider;
            _skeletonLogger = skeletonLogger;
            IsGameRunning = false;
        }

        #endregion

        #region Properties

        private bool isMainTabEnabled = false;
        /// <summary>
        /// Gets or sets the IsMainTabEnabled to enable the main tab. Should be disabled when user hasn't loaded a games folder config.yaml
        /// </summary>
        public bool IsMainTabEnabled
        {
            get { return isMainTabEnabled; }
            set
            {
                SetProperty(ref isMainTabEnabled, value);
            }
        }

        private bool isMachineConfigEnabled = false;
        /// <summary>
        /// Gets or sets the IsMachineConfigEnabled to enable the machine config tab. Should be disabled when a machine.yaml fails to parse
        /// </summary>
        public bool IsMachineConfigEnabled
        {
            get { return isMachineConfigEnabled; }
            set
            {
                SetProperty(ref isMachineConfigEnabled, value);
            }
        }

        private bool isGameRunning = false;

        /// <summary>
        /// Gets or sets the IsGameRunning.
        /// </summary>
        public bool IsGameRunning
        {
            get { return isGameRunning; }
            set
            {
                SetProperty(ref isGameRunning, value);
            }
        }

        /// <summary>
        /// The last game launched log
        /// </summary>
        private IList<string> _lastgameLog;

        #endregion
    }
}

