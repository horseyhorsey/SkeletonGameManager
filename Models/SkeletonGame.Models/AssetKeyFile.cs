using PropertyChanged;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// Base model to hold key file values for most classes.
    /// </summary>
    public abstract class AssetKeyFile
    {
        [YamlMember(Alias = "key", ApplyNamingConventions = false)]
        public string Key { get; set; }

        [YamlMember(Alias = "file", ApplyNamingConventions = false)]
        public string File { get; set; }
    }
}
