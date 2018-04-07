using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGame.Models.Data;
using SkeletonGame.Models.Machine;
using SkeletonGame.Models.Score;
using System;
using System.IO;
using Xunit;
using YamlDotNet.Serialization;

namespace SkeletonGame.Tests
{
    public class SkeletonGameEngineTests
    {
        ISkeletonGameSerializer _skeleSerializer;

        public SkeletonGameEngineTests()
        {
            _skeleSerializer = new SkeletonGameSerializer();
        }

        [Theory()]
        //[InlineData("EmptyGame")]
        [InlineData("SampleGame")]
        //[InlineData("EvilDead")]
        public void DeseralizeSkeletonGameConfig(string gameFolder)
        {
            var gameConfig = _skeleSerializer.DeserializeSkeletonYaml<GameConfig>(@"TestData/" + gameFolder + "/config.yaml");

            Assert.NotNull(gameConfig);
        }

        [Theory()]
        [InlineData("EmptyGame")]
        [InlineData("SampleGame")]
        [InlineData("EvilDead")]
        public void DeseralizeSkeletonGameAssets(string gameFolder)
        {
            var assets = _skeleSerializer.DeserializeSkeletonYaml<AssetsFile>(@"TestData/" + gameFolder + "/config/asset_list.yaml");

            Assert.NotNull(assets);
        }

        [Theory()]
        [InlineData("EmptyGame")]
        [InlineData("SampleGame")]
        [InlineData("EvilDead")]
        public void DeserializeSkeletonAttract(string gameFolder)
        {
            var attractFilePath = "TestData/" + gameFolder + "/config/attract.yaml";

            var attract = _skeleSerializer.DeserializeSkeletonYaml<SequenceYaml>(@"TestData/" + gameFolder + "/config/attract.yaml");

            Assert.NotNull(attract);
        }

        [Theory()]
        [InlineData("EmptyGame")]
        [InlineData("SampleGame")]
        [InlineData("EvilDead")]
        public void DeserializeSkeletonScoreDisplay(string gameFolder)
        {
            var scoreDisplayYaml = @"TestData/" + gameFolder + "/config/new_score_display.yaml";

            ScoreDisplay ScoreDisplayConfig = null;

            //Deal with the updated score display
            if (File.Exists(scoreDisplayYaml))
                ScoreDisplayConfig = _skeleSerializer.DeserializeSkeletonYaml<ScoreDisplay>(scoreDisplayYaml);
            else
            {
                scoreDisplayYaml = @"TestData/" + gameFolder + "/config/score_display.yaml";

                if (File.Exists(scoreDisplayYaml))
                    ScoreDisplayConfig = _skeleSerializer.DeserializeSkeletonYaml<ScoreDisplay>(scoreDisplayYaml);
            }

            Assert.NotNull(ScoreDisplayConfig);
        }

        [Fact]
        public void DeserializeMachineYaml()
        {
            //var machine = _skeleSerializer.DeserializeSkeletonYaml<MachineConfig>(@"TestData/EmptyGame/config/machine.yaml");
            var machine = _skeleSerializer.DeserializeSkeletonYaml<MachineConfig>(@"TestData/EmptyGame/config/machine.yaml");            
        }

        [Fact]
        public void DeserializeMachineYamlKeyValues()
        {
            //var machine = _skeleSerializer.DeserializeSkeletonYaml<MachineConfig>(@"TestData/SampleGame/config/machine.yaml");
            var machine = _skeleSerializer
                .DeserializeSkeletonYaml<MachineConfigDict>(@"C:\P-ROC\Games\jaws\config\machine_jaws.yaml");
        }

        [Fact]
        public void DeserializeGameData()
        {
            //var machine = _skeleSerializer.DeserializeSkeletonYaml<MachineConfig>(@"TestData/SampleGame/config/machine.yaml");
            var gData = _skeleSerializer
                .DeserializeSkeletonYaml<GameData>(@"TestData/EmptyGame/config/game_default_data.yaml");
        }

        [Fact]
        public void DeserializeHighScoreDefaults()
        {
            //var machine = _skeleSerializer.DeserializeSkeletonYaml<MachineConfig>(@"TestData/SampleGame/config/machine.yaml");
            var gData = _skeleSerializer
                .DeserializeSkeletonYaml<HighScoreData>(@"TestData/EmptyGame/config/hiscore_default_data.yaml");
        }

        [Fact]
        public void DeserializeTrophyData()
        {
            //var machine = _skeleSerializer.DeserializeSkeletonYaml<MachineConfig>(@"TestData/SampleGame/config/machine.yaml");
            var tData = _skeleSerializer
                .DeserializeSkeletonYaml<TrophyData>(@"TestData/EmptyGame/config/trophy_default_data.yaml");
        }

        [Fact]
        public void YamlToJson()
        {
            var attract = _skeleSerializer.ConvertToJson(@"TestData/EmptyGame/config.yaml");
        }

        [Fact(Skip = "Just used for creating the extraction method")]
        public void ExtractZipArchive()
        {
            var createSkele = new CreateSkeletonGame();
            var zip = @"C:\Users\funktub\Documents\Visual Studio 2017\Projects\Pinball\SkeletonGameManager\UI\SkeletonGameManager.WPF\bin\Debug\Temp\dev.zip";

            createSkele.CreateNewGameEntry("MyNewGame", "EmptyGame", Environment.CurrentDirectory, zip);
        }

        [Fact]
        public void ExportPrLampsToJsonLampshowUI()
        {
            var machine = _skeleSerializer.DeserializeSkeletonYaml<MachineConfig>(@"TestData/EmptyGame/config/machine.yaml");
            ISkeletonGameExport export = new SkeletonGameExport();
            export.ExportLampsToLampshowUI(machine.PRLamps, "EmptyGame", @"TestData/EmptyGame");
        }

        [Fact]
        public void ExportToPyprocgameSwitchHits()
        {
            var machine = _skeleSerializer.DeserializeSkeletonYaml<MachineConfig>(@"TestData/EmptyGame/config/machine.yaml");
            ISkeletonGameExport export = new SkeletonGameExport();
            export.ExportToPyprocgameSwitchHits(machine.PRSwitches, @"TestData/EmptyGame");
        }
    }
}
