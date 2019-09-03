namespace SkeletonGame.Models
{
    public class VpGameMap
    {
        public string Rom { get; set; }
        public string Kls { get; set; }
        public string Path { get; set; }
        public string Yaml { get; set; }
        public string Table { get; set; }

        public VpGameMap(string rom, string path, string kls, string yaml, string table = null)
        {
            Rom = rom;
            Path = path;
            Kls = kls;
            Yaml = yaml;
            Table = table;
        }

        public VpGameMap()
        {

        }
    }
}
