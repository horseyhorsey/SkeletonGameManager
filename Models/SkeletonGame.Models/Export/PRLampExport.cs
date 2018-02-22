using System.Collections.Generic;

namespace SkeletonGame.Models.Export
{
    public class PRLampExport
    {
        public string PlayfieldImage { get; set; }
        public double PlayfieldToLedScale { get; set; } = 0.25;
        public List<LED> Leds { get; set; }
        public List<Group> Groups { get; set; }
    }

    public class LED
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSingleColor { get; set; } = true;
        public string SingleColor { get; set; } = "#FFFFFFFF";
        public double LocationX { get; set; } = 5.0;
        public double LocationY { get; set; } = -2.0;
        public double Angle { get; set; } = 0.0;
        public double Scale { get; set; } = 1.0;
        public int Shape { get; set; } = 1;
    }

    public class Group
    {
        public string Name { get; set; }
        public List<int> Leds { get; set; }
    }
}
