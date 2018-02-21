using System.ComponentModel;

namespace SkeletonGame.Models.Machine
{
    /// <summary>
    /// Used for WPC games
    /// </summary>
    public enum FlipperCoils
    {
        [Description("flipperLwRMain")]
        FLRM = 29,
        [Description("flipperLwRHold")]
        FLRH = 30,
        [Description("flipperLwLMain")]
        FLLM = 31,
        [Description("flipperLwLHold")]
        FLLH = 32,
        [Description("flipperUpRMain")]        
        FURM = 33,
        [Description("flipperUpRHold")]        
        FURH = 34,        
        [Description("flipperUpLMain")]
        FULM = 35,
        [Description("flipperUpLHold")]        
        FULH = 36
    }
}
