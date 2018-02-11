using System.Collections.Generic;

namespace SkeletonGame.Engine
{
    public interface ISkeletonLogger
    {
        IList<string> LogData { get; set; }
    }

    public class SkeletonLogger : ISkeletonLogger
    {
        public IList<string> LogData { get; set; } = new List<string>();
    }
}
