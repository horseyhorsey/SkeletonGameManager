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
        [InlineData("TotalSickness")]
        //[InlineData("EvilDead")]
        //[InlineData("Bottom")]
        public void DeseralizeSkeletonGameConfig(string gameFolder)
        {
            var gameConfig = _skeleSerializer.DeserializeSkeletonYaml<GameConfig>(@"TestData/" + gameFolder + "//" + Constants.FILE_CONFIG);

            Assert.NotNull(gameConfig);
        }

        [Theory()]
        [InlineData("EmptyGame")]
        [InlineData("SampleGame")]
        [InlineData("EvilDead")]
        [InlineData("Bottom")]
        [InlineData("TotalSickness")]
        public void DeseralizeSkeletonGameAssets(string gameFolder)
        {
            var assets = _skeleSerializer.DeserializeSkeletonYaml<AssetsFile>(@"TestData/" + gameFolder + "//" + Constants.FILE_ASSETS);

            Assert.NotNull(assets);
        }

        [Theory()]
        //[InlineData("EmptyGame")]
        //[InlineData("SampleGame")]
        //[InlineData("EvilDead")]        
        [InlineData("TotalSickness")]
        public void DeserializeSkeletonAttract(string gameFolder)
        {
            var attractFilePath = "TestData/" + gameFolder + "/config/attract.yaml";

            var attract = _skeleSerializer.DeserializeSkeletonYaml<SequenceYaml>(@"TestData/" + gameFolder + "//" + Constants.FILE_ATTRACT);

            Assert.NotNull(attract);
        }

        [Theory()]
        //[InlineData("EmptyGame")]
        //[InlineData("SampleGame")]
        //[InlineData("EvilDead")]
        [InlineData("Bottom")]
        public void DeserializeSkeletonScoreDisplay(string gameFolder)
        {
            var scoreDisplayYaml = @"TestData/" + gameFolder + Constants.FILE_SCOREDISPLAY;

            ScoreDisplay ScoreDisplayConfig = null;

            //Deal with the updated score display
            if (File.Exists(scoreDisplayYaml))
                ScoreDisplayConfig = _skeleSerializer.DeserializeSkeletonYaml<ScoreDisplay>(scoreDisplayYaml);
            else
            {
                scoreDisplayYaml = @"TestData/" + gameFolder + Constants.FILE_SCOREDISPLAY;

                if (File.Exists(scoreDisplayYaml))
                    ScoreDisplayConfig = _skeleSerializer.DeserializeSkeletonYaml<ScoreDisplay>(scoreDisplayYaml);
            }

            Assert.NotNull(ScoreDisplayConfig);
        }

        [Fact]
        public void DeserializeMachineYaml()
        {
            //var machine = _skeleSerializer.DeserializeSkeletonYaml<MachineConfig>(@"TestData/EmptyGame/config/machine.yaml");
            var machine = _skeleSerializer.DeserializeSkeletonYaml<MachineConfig>(@"TestData/EmptyGame/" + Constants.FILE_MACHINE);

            Assert.NotNull(machine);
        }

        [Fact]
        public void DeserializeGameData()
        {
            //var machine = _skeleSerializer.DeserializeSkeletonYaml<MachineConfig>(@"TestData/SampleGame/config/machine.yaml");
            var gData = _skeleSerializer
                .DeserializeSkeletonYaml<GameData>(@"TestData/EmptyGame/config/game_default_data.yaml");

            Assert.NotNull(gData);
        }

        [Fact]
        public void DeserializeHighScoreDefaults()
        {
            //var machine = _skeleSerializer.DeserializeSkeletonYaml<MachineConfig>(@"TestData/SampleGame/config/machine.yaml");
            var gData = _skeleSerializer
                .DeserializeSkeletonYaml<HighScoreData>(@"TestData/EmptyGame/config/hiscore_default_data.yaml");

            Assert.NotNull(gData);
        }

        [Fact]
        public void DeserializeTrophyData()
        {
            //var machine = _skeleSerializer.DeserializeSkeletonYaml<MachineConfig>(@"TestData/SampleGame/config/machine.yaml");
            var tData = _skeleSerializer
                .DeserializeSkeletonYaml<TrophyData>(@"TestData/EmptyGame/config/trophy_default_data.yaml");

            Assert.NotNull(tData);
        }

        [Fact]
        public void YamlToJson()
        {
            var attract = _skeleSerializer.ConvertToJson(@"TestData/EmptyGame/config.yaml");

            Assert.NotNull(attract);
        }
        
        [Fact(Skip = "Integration")]
        public void ExtractZipArchive()
        {
            var createSkele = new CreateSkeletonGame();
            var zip = @"C:\Users\funktub\Documents\Visual Studio 2017\Projects\Pinball\SkeletonGameManager\UI\SkeletonGameManager.WPF\bin\Debug\Temp\dev.zip";

            createSkele.CreateNewGameEntry("MyNewGame", "EmptyGame", Environment.CurrentDirectory, zip);
        }

        [Fact(Skip = "Integration")]
        public void ExportPrLampsToJsonLampshowUI()
        {
            var machine = _skeleSerializer.DeserializeSkeletonYaml<MachineConfig>(@"TestData/EmptyGame/config/machine.yaml");
            ISkeletonGameExport export = new SkeletonGameExport();
            export.ExportLampsToLampshowUI(machine.PRLamps, "EmptyGame", @"TestData/EmptyGame");
        }

        [Fact(Skip ="Integration")]
        public void ExportToPyprocgameSwitchHits()
        {
            var machine = _skeleSerializer.DeserializeSkeletonYaml<MachineConfig>(@"TestData/EmptyGame/config/machine.yaml");
            ISkeletonGameExport export = new SkeletonGameExport();
            export.ExportToPyprocgameSwitchHits(machine.PRSwitches, @"TestData/EmptyGame");
        }
    }
}
