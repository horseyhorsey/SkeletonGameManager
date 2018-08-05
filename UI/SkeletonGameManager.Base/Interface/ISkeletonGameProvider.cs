using SkeletonGame.Models;
using SkeletonGame.Models.Data;
using SkeletonGame.Models.Machine;
using SkeletonGame.Models.Score;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SkeletonGameManager.Base
{
    public interface ISkeletonGameProvider
    {        
        AssetsFile AssetsConfig { get; set; }
        SequenceYaml AttractConfig { get; set; }
        GameConfig GameConfig { get; set; }
        string GameFolder { get; set; }
        MachineConfig MachineConfig { get; set; }
        MachineConfigDict MachineConfigDict { get; set; }
        ScoreDisplay ScoreDisplayConfig { get; set; }
        ObservableCollection<string> SequenceYamls { get; set; }        
        TrophyData TrophyData { get; set; }

        #region Methods
        void ClearConfigs();
        SequenceYaml GetSequence(string yamlPath);
        Task LoadYamlEntriesAsync();
        void ExportVpScript(string exportType);
        void SaveAssetsFile(AssetsFile assetsFile);
        void SaveGameConfig(GameConfig config);
        void SaveMachineConfig(MachineConfig mConfig);
        void SaveScoreDsiplayFile(ScoreDisplay scoreDisplay);
        void SaveSequenceFile(SequenceYaml sequenceYaml, string saveFile);
        void SaveTrophyData(TrophyData trophyData);
        void ExportPyProcgame(string exportParam);
        #endregion
    }
}

