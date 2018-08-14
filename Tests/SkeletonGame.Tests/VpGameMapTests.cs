using SkeletonGame.Engine;
using System.Linq;
using Xunit;

namespace SkeletonGame.Tests
{
    public class VpGameMapTests
    {
        IVpGameMapper _vpGameMap;

        public VpGameMapTests()
        {
            _vpGameMap = new VpGameMapper();
        }

        /// <summary>
        /// Gets the mapping for the vp_game_map.yaml
        /// </summary>
        [Fact]
        public void GetMapping()
        {
            var mappings = _vpGameMap.GetMappings(@"TestData\vp_game_map.yaml");            
            Assert.NotNull(mappings);
            Assert.True(mappings.Count() > 3);
            Assert.True(!string.IsNullOrEmpty(mappings.ElementAt(0).Kls));
        }
    }
}
