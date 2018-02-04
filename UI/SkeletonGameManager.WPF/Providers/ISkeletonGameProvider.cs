using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGame.Models.Machine;
using SkeletonGame.Models.Score;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SkeletonGameManager.WPF.Providers
{
    public interface ISkeletonGameProvider
    {
        void ClearConfigs();

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

        MachineConfig MachineConfig { get; set; }

        void SaveAssetsFile(AssetsFile assetsFile);

        void SaveGameConfig(GameConfig config);

        void SaveAttractFile();

        void SaveMachineConfig(MachineConfig mConfig);
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
        public IReadOnlyList<string> YamlFiles { get; } = new List<string>() { "config.yaml", "config/asset_list.yaml", "config/attract.yaml", "config/new_score_display.yaml", "config/score_display.yaml", "config/machine.yaml" };

        public string GameFolder { get; set; }

        public GameConfig GameConfig { get; set; }

        public AttractYaml AttractConfig { get; set; }

        public AssetsFile AssetsConfig { get; set; }

        public ScoreDisplay ScoreDisplayConfig { get; set; }

        public MachineConfig MachineConfig { get; set; }

        #endregion

        #region Public Methods

        public void ClearConfigs()
        {
            GameConfig = null;
            AssetsConfig = null;
            ScoreDisplayConfig = null;
            ScoreDisplayConfig = null;
        }

        public async Task LoadYamlEntriesAsync()
        {
            if (string.IsNullOrWhiteSpace(this.GameFolder))
                throw new System.NullReferenceException("Game folder cannot be an empty string");

            if (!Directory.Exists(GameFolder))
                throw new FileNotFoundException($"Cannot find game folder: {GameFolder}");

            await Task.Run(async () =>
            {
                try
                {
                    GameConfig = _skeletonGameSerializer.DeserializeSkeletonYaml<GameConfig>(Path.Combine(GameFolder, YamlFiles[0]));
                    AssetsConfig = _skeletonGameSerializer.DeserializeSkeletonYaml<AssetsFile>(Path.Combine(GameFolder, YamlFiles[1]));                    
                    AttractConfig = _skeletonGameSerializer.DeserializeSkeletonYaml<AttractYaml>(Path.Combine(GameFolder, YamlFiles[2]));

                    var newScoreDisplayYaml = Path.Combine(GameFolder, YamlFiles[3]);
                    var scoreDisplayYaml = Path.Combine(GameFolder, YamlFiles[4]);

                    //Deal with the updated score display
                    if (File.Exists(newScoreDisplayYaml))
                        ScoreDisplayConfig = _skeletonGameSerializer.DeserializeSkeletonYaml<ScoreDisplay>(newScoreDisplayYaml);
                    else if (File.Exists(scoreDisplayYaml))
                        ScoreDisplayConfig = _skeletonGameSerializer.DeserializeSkeletonYaml<ScoreDisplay>(scoreDisplayYaml);

                    try
                    {
                        MachineConfig = _skeletonGameSerializer.DeserializeSkeletonYaml<MachineConfig>(Path.Combine(GameFolder, YamlFiles[5]));
                    }
                    catch (FileNotFoundException ex)
                    {
                        Dispatcher.CurrentDispatcher.Invoke(() =>
                        {
                            System.Windows.MessageBox.Show($"{ex.Message}");
                        });
                    }
                    catch (System.Exception ex)
                    {
                        Dispatcher.CurrentDispatcher.Invoke(() =>
                        {
                            System.Windows.MessageBox.Show($"Error parsing machine.yaml \n\r Yaml entries must be converted to list before use here\n\r See EmptyGames machine.yaml\n\r {ex.Message}");
                        });
                    }

                }
                catch (System.Exception ex)
                {
                    ClearConfigs();

                    throw;
                }

            });
        }

        public void SaveAssetsFile(AssetsFile assetsFile)
        {
            var yamlFile = Path.Combine(GameFolder, YamlFiles[1]);
            _skeletonGameSerializer.SerializeYaml(yamlFile, assetsFile);
        }

        public void SaveAttractFile()
        {
            var combo = AttractConfig.AttractSequences
                .Select(x => x.Combo);

            var grouped = AttractConfig.AttractSequences
                .Select(x => x.GroupLayer);

            var markup = AttractConfig.AttractSequences
                .Select(x => x.MarkupLayer);

            var scripted = AttractConfig.AttractSequences
                        .Select(x => x.ScriptedText);

            var randomtxt = AttractConfig.AttractSequences
            .Select(x => x.RandomText);

            foreach (var group in grouped.Where(x => x != null))
            {
                foreach (var item in group.Contents.Where(x => x.markup_layer !=null))
                {
                    item.markup_layer.TextList.Clear();                    
                    if (item.markup_layer.TextList != null)
                        item.markup_layer.TextList.Clear();

                    item.markup_layer.duration = null;
                    item.markup_layer.TextList = item.markup_layer.TextEntries.Select(x => x.TextLine).ToList();
                }

                foreach (var item in group.Contents.Where(x => x.combo_layer != null))
                {                    
                    if (item.combo_layer.TextList != null)
                        item.combo_layer.TextList.Clear();

                    item.combo_layer.duration = null;
                    item.combo_layer.TextList = item.combo_layer.TextEntries.Select(x => x.TextLine).ToList();
                }
            }

            var yamlFile = Path.Combine(GameFolder, YamlFiles[2]);

            //Assign markup  text lists
            foreach (var item in markup.Where(x => x != null))
            {
                if (item.TextList != null)
                    item.TextList.Clear();

                item.TextList = item.TextEntries.Select(x => x.TextLine).ToList();
            }

            //Assign scripted text lists
            foreach (var item in scripted.Where(x => x != null))
            {
                foreach (var item2 in item.TextOptions)
                {
                    item2.TextList = item2.TextEntries.Select(x=> x.TextLine).ToList();
                }
            }

            //Assign random text lists
            foreach (var item in randomtxt.Where(x => x != null))
            {
                foreach (var item2 in item.TextOptions)
                {
                    item2.TextList = item2.TextEntries.Select(x => x.TextLine).ToList();
                }
            }

            foreach (var item in combo.Where(x => x!= null))
            {

                if (item.TextList != null)
                    item.TextList.Clear();

                item.TextList = item.TextEntries.Select(x => x.TextLine).ToList();
                       
            }                  
            //AttractConfig.Sequences
            _skeletonGameSerializer.SerializeYaml(yamlFile, AttractConfig);

        }

        public void SaveGameConfig(GameConfig config)
        {
            var yamlFile = Path.Combine(GameFolder, YamlFiles[0]);
            _skeletonGameSerializer.SerializeYaml(yamlFile, config);
        }

        public void SaveMachineConfig(MachineConfig mConfig)
        {
            //var yamlFile = Path.Combine(GameFolder, YamlFiles[6]);
            _skeletonGameSerializer.SerializeYaml("machine.yaml", mConfig);
            //_skeletonGameSerializer.SerializeYaml(yamlFile, mConfig);
        }

        #endregion
    }
}

