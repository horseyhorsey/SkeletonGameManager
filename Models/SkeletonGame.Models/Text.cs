using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models
{
    public class Text
    {
        [YamlMember(Alias = "color", ApplyNamingConventions = false)]
        public List<int> Color { get; set; }

        [YamlMember(Alias = "y_center", ApplyNamingConventions = false)]
        public float YCenter { get; set; }
    }
}
