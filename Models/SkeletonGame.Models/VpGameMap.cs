namespace SkeletonGame.Models
{
    public class VpGameMap
    {
        public string Kls { get; set; }
        public string Path { get; set; }
        public string Yaml { get; set; }

        public VpGameMap(string path, string kls, string yaml)
        {
            Path = path;
            Kls = kls;
            Yaml = yaml;
        }

        public VpGameMap()
        {

        }
    }
}
