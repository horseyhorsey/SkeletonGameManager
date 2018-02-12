using SkeletonGame.Models;
using SkeletonGame.Models.Layers;
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
        void GetAvailableSequences(SequenceYaml attractYaml);
    }

    public class SkeletonGameAttract : ISkeletonGameAttract
    {
        /// <summary>
        /// Gets the available sequences. A sequence contains all types, but only one type is set at a time. Get the sequence base value where not null in lists.
        /// </summary>
        /// <param name="attractYaml"></param>
        public void GetAvailableSequences(SequenceYaml attractYaml)
        {            
            attractYaml.Sequences.Clear();

            foreach (var seq in attractYaml.AttractSequences)
            {
                var notNullSequence = (SequenceBase)typeof(Sequence).GetProperties()
                          .Select(prop => prop.GetValue(seq, null))
                          .Where(val => val != null).First();

                if (notNullSequence != null)
                {
                    //Apply group layer
                    if (notNullSequence.GetType() == typeof(GroupLayer))
                    {
                        var group = notNullSequence as GroupLayer;
                        foreach (var item in group.Contents)
                        {                            
                            //notNullSequence.SeqType = item.SeqType;
                            if (item.markup_layer != null)
                            {                                
                                item.SeqType = item.markup_layer.SeqType;
                            }                                
                            else if (item.animation_layer != null)
                                item.SeqType = item.animation_layer.SeqType;
                            else if (item.text_layer != null)
                                item.SeqType = item.text_layer.SeqType;
                            else if (item.combo_layer != null)
                                item.SeqType = item.combo_layer.SeqType;
                            else if (item.move_layer != null)
                                item.SeqType = item.move_layer.SeqType;
                            else if (item.scripted_text_layer != null)
                                item.SeqType = item.scripted_text_layer.SeqType;

                            //Assign style name
                            item.SequenceName = item.SeqType + "SequenceStyle";
                        }                        
                    }

                    notNullSequence.SequenceName = notNullSequence.GetType().Name;
                    attractYaml.Sequences.Add(notNullSequence);
                }                
            }
        }
    }
}
