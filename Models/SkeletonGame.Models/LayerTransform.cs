using YamlDotNet.Serialization;

namespace SkeletonGame.Models
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
