using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGame.Models.Score;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SkeletonGameManager.WPF.Providers
{
    public interface ISkeletonGameProvider
    {
        /// <summary>
        /// Gets or sets the current skeleton game folder.
        /// </summary>
        string GameFolder { get; set; }

        /// <summary>
        /// Loads the yaml entries and deserializes
        /// </summary>
        Task LoadYamlEntriesAsync();

        /// <summary>
        /// Gets or sets the game configuration.
        /// </summary>
        GameConfig GameConfig { get; set; }

        AssetsFile AssetsConfig { get; set; }

        AttractYaml AttractConfig { get; set; }

        ScoreDisplay ScoreDisplayConfig { get; set; }

        void SaveAssetsFile(AssetsFile assetsFile);

        void SaveGameConfig(GameConfig config);
    }

    /// <summary>
    /// Provides data from the yaml files from SkeletonGame
    /// </summary>
    /// <seealso cref="SkeletonGameManager.WPF.Providers.ISkeletonGameProvider" />
    public class SkeletonGameProvider : ISkeletonGameProvider
    {
        #region Fields
        private ISkeletonGameSerializer _skeletonGameSerializer; 
        #endregion

        #region Constructors

        public SkeletonGameProvider(ISkeletonGameSerializer skeletonGameSerializer)
        {
            _skeletonGameSerializer = skeletonGameSerializer;
        }

        #endregion

        #region Properties
        public IReadOnlyList<string> YamlFiles { get; } = new List<string>() { "config.yaml", "config/asset_list.yaml", "config/attract.yaml", "config/new_score_display.yaml", "config/score_display.yaml" };

        public string GameFolder { get; set; }

        public GameConfig GameConfig { get; set; }

        public AttractYaml AttractConfig { get; set; }

        public AssetsFile AssetsConfig { get; set; }

        public ScoreDisplay ScoreDisplayConfig { get; set; }

        #endregion

        #region Public Methods

        public async Task LoadYamlEntriesAsync()
        {
            if (string.IsNullOrWhiteSpace(this.GameFolder))
                throw new System.NullReferenceException("Game folder cannot be an empty string");

            if (!Directory.Exists(GameFolder))
                throw new FileNotFoundException($"Cannot find game folder: {GameFolder}");

            await Task.Run(() =>
            {
                try
                {
                    GameConfig = _skeletonGameSerializer.DeserializeSkeletonYaml<GameConfig>(Path.Combine(GameFolder, YamlFiles[0]));
                    AssetsConfig = _skeletonGameSerializer.DeserializeSkeletonYaml<AssetsFile>(Path.Combine(GameFolder, YamlFiles[1]));
                    AttractConfig = _skeletonGameSerializer.DeserializeSkeletonYaml<AttractYaml>(Path.Combine(GameFolder, YamlFiles[2]));

                    var newScoreDisplayYaml = Path.Combine(GameFolder, YamlFiles[3]);
                    var scoreDisplayYaml = Path.Combine(GameFolder, YamlFiles[4]);

                    //Deal with the updated score display
                    if (File.Exists(scoreDisplayYaml))
                        ScoreDisplayConfig = _skeletonGameSerializer.DeserializeSkeletonYaml<ScoreDisplay>(newScoreDisplayYaml);
                    else if (File.Exists(scoreDisplayYaml))
                        ScoreDisplayConfig = _skeletonGameSerializer.DeserializeSkeletonYaml<ScoreDisplay>(scoreDisplayYaml);

                }
                catch (System.Exception ex)
                {                             
                    throw;
                }
                
            });            
        }

        public void SaveAssetsFile(AssetsFile assetsFile)
        {
            var yamlFile = Path.Combine(GameFolder, YamlFiles[1]);
            _skeletonGameSerializer.SerializeYaml(yamlFile, assetsFile);
        }

        public void SaveGameConfig(GameConfig config)
        {
            var yamlFile = Path.Combine(GameFolder, YamlFiles[0]);
            _skeletonGameSerializer.SerializeYaml(yamlFile, config);
        }

        #endregion
    }
}
