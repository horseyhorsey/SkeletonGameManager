namespace SkeletonGame.Models.Transforms
{
    public interface IMoveLayer
    {
        int frames { get; set; }
        bool loop { get; set; }
        string start_x { get; set; }
        string start_y { get; set; }
        string target_x { get; set; }
        string target_y { get; set; }
    }
}
