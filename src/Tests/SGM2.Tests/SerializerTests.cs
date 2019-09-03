using SkeletonGame.Engine;
using SkeletonGame.Models;
using SkeletonGame.Models.Score;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace SGM2.Tests
{
    public class SerializerTests
    {
        ISkeletonGameSerializer _skeleSerializer;
        public SerializerTests()
        {
            _skeleSerializer = new SkeletonGameSerializer();
        }

        [Theory()]
        [InlineData("SampleGame")]
        public void DeseralizeSkeletonGameConfig(string gameFolder)
        {
            var gameConfig = _skeleSerializer
                .DeserializeSkeletonYaml<GameConfig>(@"TestData/" + gameFolder + "//" + Constants.FILE_CONFIG);

            Assert.NotNull(gameConfig);
        }


        [Theory()]
        [InlineData("SampleGame")]
        public void DeseralizeSkeletonGameAssets(string gameFolder)
        {
            var assets = _skeleSerializer.DeserializeSkeletonYaml<AssetsFile>(@"TestData/" + gameFolder + "//" + Constants.FILE_ASSETS);

            Assert.NotNull(assets);
        }

        [Theory()]
        [InlineData("SampleGame")]
        public void DeserializeSkeletonAttract(string gameFolder)
        {
            var attractFilePath = "TestData/" + gameFolder + "/config/attract.yaml";

            var attract = _skeleSerializer.DeserializeSkeletonYaml<SequenceYaml>(@"TestData/" + gameFolder + "//" + Constants.FILE_ATTRACT);

            Assert.NotNull(attract);
        }

        [Theory()]        
        [InlineData("SampleGame")]
        public void DeserializeSkeletonScoreDisplay(string gameFolder)
        {
            var scoreDisplayYaml = Path.Combine("TestData", gameFolder, Constants.FILE_SCOREDISPLAY);

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
    }
}
