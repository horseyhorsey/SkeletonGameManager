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
        public double XCenter { get; set; } = 0.0d;

        [YamlMember(Alias = "y_center", ApplyNamingConventions = false)]
        public double YCenter { get; set; } = 0.0d;

        [YamlMember(Alias = "width", ApplyNamingConventions = false)]
        public double Width { get; set; } = 0.0d;

        [YamlMember(Alias = "height", ApplyNamingConventions = false)]
        public double Height { get; set; } = 0.0d;

        [YamlMember(Alias = "border", ApplyNamingConventions = false)]
        public List<byte>Border { get; set; }

        [YamlMember(Alias = "fill", ApplyNamingConventions = false)]
        public List<byte> Fill { get; set; }

    }
}
