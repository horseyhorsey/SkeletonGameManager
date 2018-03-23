using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models
{
    public class GameData
    {
        [YamlMember(Alias = "Audits", ApplyNamingConventions = false)]
        public Dictionary<string, string> Audits { get; set; }

        [YamlMember(Alias = "ClassicHighScores", ApplyNamingConventions = false)]
        public List<HighScoreInit> ClassicHighScores { get; set; }

        [YamlMember(Alias = "LoopsHighScoreData", ApplyNamingConventions = false)]
        public List<HighScoreInit> LoopsHighScoreData { get; set; }
    }    

    public class HighScoreInit
    {
        [YamlMember(Alias = "inits", ApplyNamingConventions = false)]
        public string Inits { get; set; }

        [YamlMember(Alias = "score", ApplyNamingConventions = false)]
        public string Score { get; set; }
    }
}
