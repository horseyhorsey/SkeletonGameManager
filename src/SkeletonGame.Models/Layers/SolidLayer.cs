using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
    //TODO: Not sure what this is for, could just use the GroupLayers bakground.
    [Serializable]
    public class SolidLayer
    {
        [YamlMember(Alias = "width", ApplyNamingConventions = false)]
        public int Width { get; set; }

        [YamlMember(Alias = "height", ApplyNamingConventions = false)]
        public int Height { get; set; }

        [YamlMember(Alias = "color", ApplyNamingConventions = false)]
        public List<byte> Color { get; set; }
    }
}
