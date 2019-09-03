namespace SkeletonGame.Models.Transforms
{
    public interface IZoomLayer
    {
        bool Hold { get; set; }
        int FramesPerZoom { get; set; }
        int TotalZooms { get; set; }
        decimal ScaleStart { get; set; }
        decimal ScaleStop { get; set; }
    }
}
