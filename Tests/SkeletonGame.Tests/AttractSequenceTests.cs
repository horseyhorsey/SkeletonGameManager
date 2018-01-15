using SkeletonGame.Engine;
using SkeletonGame.Models;
using System.Linq;
using Xunit;

namespace SkeletonGame.Tests
{
    public class AttractSequenceTests
    {
        private ISkeletonGameSerializer _skeleSerializer;

        public AttractSequenceTests()
        {
            _skeleSerializer = new SkeletonGameSerializer();
        }

        [Fact]
        public void GetAvailableSequencesFromEmptyGame()
        {
            var attractFile = "TestData/EmptyGame/config/attract.yaml";

            var attract = _skeleSerializer.DeserializeSkeletonYaml<AttractYaml>(attractFile);

            Assert.NotNull(attract);

            ISkeletonGameAttract skeletonGameAttract = new SkeletonGameAttract();

            var sequences = skeletonGameAttract.GetAvailableSequences(attract);

            Assert.True(sequences.Count() > 0);
        }
    }
}
