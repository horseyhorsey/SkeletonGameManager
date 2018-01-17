using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models
{
    public class UserInterface
    {
        [YamlMember(Alias = "splash_screen", ApplyNamingConventions = false)]
        public string SplashScreen { get; set; }

        [YamlMember(Alias = "progress_bar", ApplyNamingConventions = false)]
        public ProgressBar ProgressBar { get; set; }

        [YamlMember(Alias = "text", ApplyNamingConventions = false)]
        public Text Text { get; set; }

    }

    public class ProgressBar
    {
        [YamlMember(Alias = "x_center", ApplyNamingConventions = false)]
        public float XCenter { get; set; }

        [YamlMember(Alias = "y_center", ApplyNamingConventions = false)]
        public float YCenter { get; set; }

        [YamlMember(Alias = "width", ApplyNamingConventions = false)]
        public float Width { get; set; }

        [YamlMember(Alias = "height", ApplyNamingConventions = false)]
        public float Height { get; set; }

        [YamlMember(Alias = "border", ApplyNamingConventions = false)]
        public List<byte>Border { get; set; }

        [YamlMember(Alias = "fill", ApplyNamingConventions = false)]
        public List<byte> Fill { get; set; }

    }
}
