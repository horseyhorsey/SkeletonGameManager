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

    public class HighScoreData
    {
        //public Dictionary<object, List<HighScoreInit>> HighScoreCategories { get; set; }
        [YamlMember(Alias = "HiscoreCategories", ApplyNamingConventions = false)]
        public object HiscoreCategories { get; set; }
    }

    public class HighScoreInit
    {
        /// <summary>
        /// Gets or sets the inits. The players name.
        /// </summary>
        [YamlMember(Alias = "inits", ApplyNamingConventions = false)]
        public string Inits { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        [YamlMember(Alias = "score", ApplyNamingConventions = false)]
        public long Score { get; set; }
    }
}
