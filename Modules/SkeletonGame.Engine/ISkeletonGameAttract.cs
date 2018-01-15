using SkeletonGame.Models;
using System.Collections.Generic;
using System.Linq;

namespace SkeletonGame.Engine
{
    public interface ISkeletonGameAttract
    {
        /// <summary>
        /// Gets the available sequences. A sequence contains all types, but only one type is set at a time. Get the value where not null. Yaml sucks
        /// </summary>
        /// <param name="sequence">The sequence.</param>
        /// <returns></returns>
        IEnumerable<SequenceBase> GetAvailableSequences(AttractYaml attractYaml);
    }

    public class SkeletonGameAttract : ISkeletonGameAttract
    {
        public IEnumerable<SequenceBase> GetAvailableSequences(AttractYaml attractYaml)
        {
            List<SequenceBase> sequences = new List<SequenceBase>();

            foreach (var seq in attractYaml.Sequence)
            {
                var notNullSequence = (SequenceBase)typeof(Sequence).GetProperties()
                          .Select(prop => prop.GetValue(seq, null))
                          .Where(val => val != null).First();

                if (notNullSequence != null)
                    sequences.Add(notNullSequence);
            }

            return sequences;                 
        }
    }
}
