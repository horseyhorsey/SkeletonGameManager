using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGameManager.WPF.Providers;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SkeletonGameManager.WPF.ViewModels.AssetViewModels
{
    public class FontsViewModel : AssetFileBaseViewModel
    {
        private readonly ISkeletonGameProvider _skeletonGameProvider;
        private Uri _fontPath;
        private readonly ISkeletonGameFiles _skeletonGameFiles;

        public FontsViewModel(ISkeletonGameFiles skeletonGameFiles, ISkeletonGameProvider skeletonGameProvider)
        {
            _skeletonGameProvider = skeletonGameProvider;
            _skeletonGameFiles = skeletonGameFiles;

            FontStyles = _skeletonGameProvider.AssetsConfig.Fonts.FontStyles;

            HdFonts = _skeletonGameProvider.AssetsConfig.Fonts.HdFonts;

        }

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

        public async override Task GetFiles()
        {
            var hdFontPath = _skeletonGameProvider.GameConfig.HdFontPath;

            if (hdFontPath.Contains('.'))
                _fontPath = new Uri(Path.Combine(_skeletonGameProvider.GameFolder, hdFontPath), UriKind.RelativeOrAbsolute);
            else
                _fontPath = new Uri(hdFontPath);

            var fontFiles = await _skeletonGameFiles.GetFilesAsync(_fontPath.AbsolutePath, AssetTypes.HdFonts);

            this.AssetFiles = new System.Collections.ObjectModel.ObservableCollection<string>(fontFiles);
        }
    }
}
