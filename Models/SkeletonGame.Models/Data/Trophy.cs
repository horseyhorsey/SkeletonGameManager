using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Data
{
    public class Trophy
    {
        [YamlMember(Alias = "description", ApplyNamingConventions = false)]
        public string Description { get; set; }

        [YamlMember(Alias = "completedDate", ApplyNamingConventions = false)]
        public string CompletedDate { get; set; }

        [YamlMember(Alias = "icon", ApplyNamingConventions = false)]
        public string Icon { get; set; }
    }

    public class TrophyData
    {
        [YamlMember(Alias = "Trophys", ApplyNamingConventions = false)]
        public ObservableDictionary<string, Trophy> Trophys { get; set; }
    }


}
