using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGameManager.Base;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SkeletonGameManager.Module.Assets.ViewModels
{
    public class FontsViewModel : AssetFileBaseViewModel
    {
        #region Fields
        private readonly ISkeletonGameProvider _skeletonGameProvider;
        private Uri _fontPath;
        private readonly ISkeletonGameFiles _skeletonGameFiles;
        #endregion

        #region Constructors
        public FontsViewModel(ISkeletonGameFiles skeletonGameFiles, ISkeletonGameProvider skeletonGameProvider, IEventAggregator eventAggregator, ILoggerFacade loggerFacade)
    : base(eventAggregator, loggerFacade)
        {
            _skeletonGameProvider = skeletonGameProvider;
            _skeletonGameFiles = skeletonGameFiles;

            Title = "Fonts";

            OpenDirectoryCommand = new DelegateCommand(() => OpenDirectory(_fontPath.AbsolutePath));
        } 
        #endregion

        #region Properties
        private ObservableCollection<HdFont> hdFonts;
        public ObservableCollection<HdFont> HdFonts
        {
            get { return hdFonts; }
            set { SetProperty(ref hdFonts, value); }
        }

        private ObservableCollection<FontStyle> fontStyles;
        public ObservableCollection<FontStyle> FontStyles
        {
            get { return fontStyles; }
            set { SetProperty(ref fontStyles, value); }
        }
        #endregion

        #region Public Methods
        public async override Task GetFiles()
        {
            try
            {
                Log("Populating font files");
                FontStyles = _skeletonGameProvider.AssetsConfig?.Fonts.FontStyles;
                HdFonts = _skeletonGameProvider.AssetsConfig?.Fonts.HdFonts;

                var hdFontPath = _skeletonGameProvider.GameConfig.HdFontPath;
                if (hdFontPath.Contains('.'))
                    _fontPath = new Uri(Path.Combine(_skeletonGameProvider.GameFolder, hdFontPath), UriKind.RelativeOrAbsolute);
                else
                    _fontPath = new Uri(hdFontPath);

                //Get font files and select the filename for assets list
                var fontFiles = await _skeletonGameFiles.GetFilesAsync(_fontPath.AbsolutePath, AssetTypes.HdFonts);
                this.AssetFiles = new ObservableCollection<string>(fontFiles.Select(x => Path.GetFileName(x)));

                Log($"Font file count: {AssetFiles.Count}");
            }
            catch (Exception ex)
            {
                Log($"Error populating fonts: {ex.Message}", Category.Warn);
            }
        } 
        #endregion
    }
}
