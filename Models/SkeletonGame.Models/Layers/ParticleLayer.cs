using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models.Layers
{
    public class ParticleLayer
    {
        [YamlMember(Alias = "width", ApplyNamingConventions = false)]
        public int Width { get; set; }

        [YamlMember(Alias = "height", ApplyNamingConventions = false)]
        public int Height { get; set; }

        [YamlMember(Alias = "emitters", ApplyNamingConventions = false)]
        public List<ParticleEmitter> Emitters { get; set; }

        [YamlIgnore]
        public SequenceType SeqType { get; set; }

        [YamlIgnore]
        public string SequenceName { get; set; }

        public ParticleLayer()
        {
            this.SeqType = SequenceType.ParticleLayer;
            SequenceName = this.SeqType + "SequenceStyle";
        }
    }

    public class ParticleEmitter
    {
        [YamlMember(Alias = "x", ApplyNamingConventions = false)]
        public int X { get; set; } = 450;

        [YamlMember(Alias = "y", ApplyNamingConventions = false)]
        public int Y { get; set; } = 225;

        [YamlMember(Alias = "max_life", ApplyNamingConventions = false)]
        public int MaxLife { get; set; } = 20;

        [YamlMember(Alias = "max_particles", ApplyNamingConventions = false)]
        public int MaxParticles { get; set; } = 200;

        [YamlMember(Alias = "particles_per_update", ApplyNamingConventions = false)]
        public int ParticlesPerUpdate { get; set; } = 40;

        [YamlMember(Alias = "total_creations", ApplyNamingConventions = false)]
        public int? TotalCreations { get; set; } = null;

        [YamlMember(Alias = "particle_class", ApplyNamingConventions = false)]
        public string ParticleClass { get; set; } = "SnowParticle";
    }
}
