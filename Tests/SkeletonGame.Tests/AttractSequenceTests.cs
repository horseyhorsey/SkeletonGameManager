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
            var attractFile = "TestData/SampleGame/config/attract.yaml";

            var attract = _skeleSerializer.DeserializeSkeletonYaml<AttractYaml>(attractFile);

            Assert.NotNull(attract);

            ISkeletonGameAttract skeletonGameAttract = new SkeletonGameAttract();

            skeletonGameAttract.GetAvailableSequences(attract);

            Assert.True(attract.Sequences.Count() > 0);            
        }
    }
}
