using Prism.Logging;
using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGame.Models.Data;
using SkeletonGame.Models.Machine;
using SkeletonGame.Models.Score;
using SkeletonGameManager.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SkeletonGameManager.Module.Services
{
    /// <summary>
    /// Provides data from the yaml files from SkeletonGame
    /// </summary>
    /// <seealso cref="SkeletonGameManager.WPF.Providers.ISkeletonGameProvider" />
    public class SkeletonGameProvider : ISkeletonGameProvider
    {
        #region Fields
        private ISkeletonGameSerializer _skeletonGameSerializer;
        private ISkeletonGameFiles _skeletonGameFiles;
        private IVpScriptExporter _vpScriptExporter;
        private ISkeletonGameExport _skeletonGameExport;
        private ILoggerFacade _loggerFacade;

        public event EventHandler<ProviderUpdatedEventArgs> StatusChanged;
        #endregion

        #region Constructors

        public SkeletonGameProvider(ISkeletonGameSerializer skeletonGameSerializer,
            ISkeletonGameFiles skeletonGameFiles,
            IVpScriptExporter vpScriptExporter, ISkeletonGameExport skeletonGameExport, ILoggerFacade loggerFacade)
        {
            _skeletonGameSerializer = skeletonGameSerializer;
            _skeletonGameFiles = skeletonGameFiles;
            _vpScriptExporter = vpScriptExporter;
            _skeletonGameExport = skeletonGameExport;
            _loggerFacade = loggerFacade;
        }

        #endregion

        #region Properties        

        public string GameFolder { get; set; }

        public GameConfig GameConfig { get; set; }
        public SequenceYaml AttractConfig { get; set; }
        public ObservableCollection<string> SequenceYamls { get; set; } = new ObservableCollection<string>();
        public AssetsFile AssetsConfig { get; set; }
        public ScoreDisplay ScoreDisplayConfig { get; set; }
        public MachineConfig MachineConfig { get; set; }
        public MachineConfigDict MachineConfigDict { get; set; }
        public TrophyData TrophyData { get; set; }

        public IReadOnlyList<string> YamlFiles { get; } = new List<string>()
        { Constants.FILE_CONFIG, Constants.FILE_ASSETS, Constants.FILE_ATTRACT, Constants.FILE_NEWSCOREDISPLAY, Constants.FILE_SCOREDISPLAY, Constants.FILE_MACHINE, Constants.FILE_TROPHY};

        #endregion

        #region Public Methods

        public void ClearConfigs()
        {
            GameConfig?.KeyboardSwitchMap?.Clear();
            GameConfig = null;
            //AssetsConfig = null;
            ScoreDisplayConfig = null;
            AttractConfig = null;
            MachineConfig = null;
            this.TrophyData = null;
            this.AssetsConfig = null;
        }

        public Task LoadYamlEntriesAsync()
        {
            if (string.IsNullOrWhiteSpace(this.GameFolder))
                throw new System.NullReferenceException("Game folder cannot be an empty string");

            if (!Directory.Exists(GameFolder))
                throw new FileNotFoundException($"Cannot find game folder: {GameFolder}");

            ClearConfigs();

            return Task.Run(async () =>
            {
                var errored = false;
                var args = new ProviderUpdatedEventArgs() { Status = 2 };
                this.OnProviderFinished(args);

                try
                {
                    GameConfig = _skeletonGameSerializer.DeserializeSkeletonYaml<GameConfig>(Path.Combine(GameFolder, YamlFiles[0]));

                    if (GameConfig == null)
                        throw new NullReferenceException("Config.yaml returned null, please check your config.yaml");


                    AssetsConfig = null;
                    if (File.Exists(Path.Combine(GameFolder, YamlFiles[1])))
                        AssetsConfig = _skeletonGameSerializer.DeserializeSkeletonYaml<AssetsFile>(Path.Combine(GameFolder, YamlFiles[1]));

                    AttractConfig = GetSequence(Path.Combine(GameFolder, YamlFiles[2]));

                    var newScoreDisplayYaml = Path.Combine(GameFolder, YamlFiles[3]);
                    var scoreDisplayYaml = Path.Combine(GameFolder, YamlFiles[4]);

                    //Deal with the updated score display
                    if (File.Exists(newScoreDisplayYaml))
                        ScoreDisplayConfig = _skeletonGameSerializer.DeserializeSkeletonYaml<ScoreDisplay>(newScoreDisplayYaml);
                    else if (File.Exists(scoreDisplayYaml))
                        ScoreDisplayConfig = _skeletonGameSerializer.DeserializeSkeletonYaml<ScoreDisplay>(scoreDisplayYaml);

                    try
                    {
                        var machineConfig = Path.Combine(GameFolder, YamlFiles[5]);
                        MachineConfig = _skeletonGameSerializer.DeserializeSkeletonYaml<MachineConfig>(machineConfig);
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
                        var msg = $"Failed loading Game. \r\n {ex.Data["yaml"]} {ex.Message}";
                        msg += $"\n\r {ex.Data["err"]}";
                        Log(msg, Category.Exception);
                        try
                        {
                            Dispatcher.CurrentDispatcher.Invoke(() =>
                                            {
                                                ParseMachineConfigWithKeyValues(ex);
                                            });
                        }
                        catch (Exception exee)
                        {
                            msg = $"Failed loading Game. \r\n {exee.Data["yaml"]} {exee.Message}";
                            msg += $"\n\r {exee.Data["err"]}";

                            Log(msg);
                            throw;
                        }
                    }

                    SequenceYamls.Clear();

                    //Add the base sequences.yaml found in config
                    var baseSeqFile = Path.Combine(GameFolder, @"config\sequences.yaml");
                    if (File.Exists(baseSeqFile))
                        SequenceYamls.Add(baseSeqFile);

                    //Get sequence files and add to list       
                    var seqDir = Path.Combine(GameFolder, @"config\sequences");
                    Directory.CreateDirectory(seqDir);
                    var seqFiles = await _skeletonGameFiles.GetFilesAsync(seqDir, AssetTypes.Sequences);
                    foreach (var item in seqFiles)
                    {
                        SequenceYamls.Add(item);
                    }

                    Directory.CreateDirectory(GameFolder + @"\recordings");
                    RecordingManager.GetPlaybackFiles(GameFolder + @"\recordings");

                    //Trophy Data
                    var trophydefaultData = Path.Combine(GameFolder, YamlFiles[6]);
                    if (!File.Exists(trophydefaultData))
                    {                        
                        System.Windows.MessageBox.Show($"Couldn't find default trophy data at {trophydefaultData}");
                    }
                    else
                    {
                        TrophyData = _skeletonGameSerializer.DeserializeSkeletonYaml<TrophyData>(trophydefaultData);
                    }                        
                }
                catch (System.Exception)
                {
                    ClearConfigs();
                    errored = true;

                    throw;
                }
                finally
                {
                    if (errored)
                        args.Status = 0;
                    else
                        args.Status = -1;

                    OnProviderFinished(args);
                }

            });
        }

        protected virtual void OnProviderFinished(ProviderUpdatedEventArgs e)
        {
            EventHandler<ProviderUpdatedEventArgs> handler = this.StatusChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        /// <summary>
        /// Exports the vp script.
        /// </summary>
        /// <param name="exportType">Type of the machine item eg Switch, Coil, ScriptFull</param>
        public void ExportVpScript(string exportType)
        {
            var scriptString = string.Empty;
            var scriptFileName = $"{GameFolder}\\VP_{exportType}.txt";

            //Delete exisitng scripts
            if (File.Exists(scriptFileName))
                File.Delete(scriptFileName);

            if (exportType == "switch")
                scriptString = _vpScriptExporter.ExportMachineValuesToScript(this.MachineConfig, SkeletonGame.Models.VpScriptExportType.Switch);
            else if (exportType == "coil")
                scriptString = _vpScriptExporter.ExportMachineValuesToScript(this.MachineConfig, SkeletonGame.Models.VpScriptExportType.Coil);
            else if (exportType == "ScriptFull")
            {
                scriptString = _vpScriptExporter.CreateVisualPinballScript(this.MachineConfig,
                    Path.GetFileName(GameFolder));
            }
            else
            {
                throw new InvalidDataException($"Invalid VP  script option. {exportType}");
            }

            //Write the script
            using (var sw = File.CreateText(scriptFileName))
            {
                sw.Write(scriptString);
            }
        }

        public SequenceYaml GetSequence(string yamlPath)
        {
            return _skeletonGameSerializer.DeserializeSkeletonYaml<SequenceYaml>(yamlPath);
        }

        public void SaveAssetsFile(AssetsFile assetsFile)
        {
            var yamlFile = Path.Combine(GameFolder, YamlFiles[1]);
            _skeletonGameSerializer.SerializeYaml(yamlFile, assetsFile);
        }

        public void SaveSequenceFile(SequenceYaml sequenceYaml, string saveFile)
        {
            var combo = sequenceYaml.AttractSequences
                .Select(x => x.Combo);

            var move = sequenceYaml.AttractSequences
                .Select(x => x.MoveLayer);

            var grouped = sequenceYaml.AttractSequences
                .Select(x => x.GroupLayer);

            var markup = sequenceYaml.AttractSequences
                .Select(x => x.MarkupLayer);

            var scripted = sequenceYaml.AttractSequences
                        .Select(x => x.ScriptedText);

            var randomtxt = sequenceYaml.AttractSequences
            .Select(x => x.RandomText);

            foreach (var group in grouped.Where(x => x != null))
            {
                foreach (var item in group.Contents.Where(x => x.markup_layer != null))
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

                foreach (var item in group.Contents.Where(x => x.scripted_text_layer != null))
                {
                    foreach (var item2 in item.scripted_text_layer.TextOptions)
                    {
                        item2.TextList = item2.TextEntries.Select(x => x.TextLine).ToList();
                    }
                }

                //foreach (var item in group.Contents.Where(x => x.move_layer != null))
                //{
                //    if (item.move_layer.TextList != null)
                //        item.move_layer.TextList.Clear();

                //    item.move_layer.duration = null;
                //    item.move_layer.TextList = item.move_layer.TextEntries.Select(x => x.TextLine).ToList();
                //}
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
                    item2.TextList = item2.TextEntries.Select(x => x.TextLine).ToList();
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

            foreach (var item in combo.Where(x => x != null))
            {

                if (item.TextList != null)
                    item.TextList.Clear();

                item.TextList = item.TextEntries.Select(x => x.TextLine).ToList();

            }

            //foreach (var item in move.Where(x => x != null))
            //{

            //    if (item.TextList != null)
            //        item.TextList.Clear();

            //    item.TextList = item.TextEntries.Select(x => x.TextLine).ToList();
            //}
            //AttractConfig.Sequences
            _skeletonGameSerializer.SerializeYaml(saveFile, sequenceYaml);

        }

        public void SaveGameConfig(GameConfig config)
        {
            var yamlFile = Path.Combine(GameFolder, YamlFiles[0]);
            _skeletonGameSerializer.SerializeYaml(yamlFile, config);
        }

        public void SaveMachineConfig(MachineConfig mConfig)
        {
            try
            {
                Log($"Saving {GameFolder + "\\" + YamlFiles[5]}");
                _skeletonGameSerializer.SerializeYaml(GameFolder + "\\" + YamlFiles[5], mConfig);
            }
            catch (Exception ex)
            {
                Log(ex.Message, Category.Exception);
            }
        }

        public void SaveScoreDsiplayFile(ScoreDisplay scoreDisplay)
        {
            var yamlFile = Path.Combine(GameFolder, YamlFiles[3]);
            _skeletonGameSerializer.SerializeYaml(yamlFile, scoreDisplay);
        }

        public void SaveTrophyData(TrophyData trophyData)
        {
            var yamlFile = Path.Combine(GameFolder, YamlFiles[6]);
            _skeletonGameSerializer.SerializeYaml(yamlFile, trophyData);
        }

        #endregion

        #region Private Properties

        public void ExportPyProcgame(string exportParam)
        {

            switch (exportParam)
            {
                case "switchPy":
                    _skeletonGameExport.ExportToPyprocgameSwitchHits(
                        this.MachineConfig.PRSwitches,
                        GameFolder);
                    break;
                case "lampshowUi":
                    _skeletonGameExport.ExportLampsToLampshowUI(
                    this.MachineConfig.PRLamps,
                    Path.GetFileName(GameFolder), GameFolder);
                    break;
                default:
                    break;
            }

        }

        private void Log(string message, Category category = Category.Debug)
        {
            _loggerFacade.Log(message, category, Priority.None);
        }

        /// <summary>
        /// Parses the machine configuration to MachineConfigDict then to a listed MachineConfig
        /// </summary>
        /// <param name="ex">The ex.</param>
        private void ParseMachineConfigWithKeyValues(System.Exception ex)
        {
            var result = System.Windows.MessageBox.Show
            ($"Error parsing machine.yaml \n\r Yaml entries must be converted to list before use here\n\r See EmptyGames machine.yaml\n\r {ex.Message}", "Couldn't parse machine, Convert and backup?", System.Windows.MessageBoxButton.YesNo);

            //Parse with different object
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                try
                {
                    var machineConfigFile = Path.Combine(GameFolder, YamlFiles[5]);
                    MachineConfigDict = _skeletonGameSerializer.DeserializeSkeletonYaml<MachineConfigDict>(machineConfigFile);

                    Log($"Saving machine.yaml to {machineConfigFile}.bak");
                    File.Delete(machineConfigFile + ".bak");
                    File.Copy(machineConfigFile, machineConfigFile + ".bak");

                    if (MachineConfigDict != null)
                    {
                        Log("Converting machine.yaml dictionary to list");

                        foreach (var coil in MachineConfigDict.PRCoils.Select(x => x))
                        {
                            coil.Value.Name = coil.Key;
                        }
                        Log($"Coils converted = {string.Join(", ", MachineConfigDict.PRCoils.Select(x => x.Value.Name))}");

                        foreach (var lamp in MachineConfigDict.PRLamps.Select(x => x))
                        {
                            lamp.Value.Name = lamp.Key;
                        }
                        Log($"Lamps converted = {string.Join(", ", MachineConfigDict.PRLamps.Select(x => x.Value.Name))}");

                        foreach (var sw in MachineConfigDict.PRSwitches.Select(x => x))
                        {
                            sw.Value.Name = sw.Key;
                        }
                        Log($"Lamps converted = {string.Join(", ", MachineConfigDict.PRSwitches.Select(x => x.Value.Name))}");

                        MachineConfig = new MachineConfig()
                        {
                            PRGame = MachineConfigDict.PRGame,
                            PRBumpers = MachineConfigDict.PRBumpers,
                            PRFlippers = MachineConfigDict.PRFlippers,
                            PRCoils = MachineConfigDict.PRCoils.Select(x => x.Value).ToList(),
                            PRLamps = MachineConfigDict.PRLamps.Select(x => x.Value).ToList(),
                            PRSwitches = MachineConfigDict.PRSwitches.Select(x => x.Value).ToList()
                        };

                        this.SaveMachineConfig(MachineConfig);
                        MachineConfigDict = null;
                    }
                }
                catch (Exception)
                {                    
                    throw;
                }
            }
        }        

        #endregion
    }
}

