using SkeletonGame.Models;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class SequenceYamlItemViewModel
    {
        public SequenceYamlItemViewModel(string fileName, SequenceYaml sequenceYaml)
        {
            Filename = fileName;
            SequenceYaml = sequenceYaml;
        }

        public SequenceYamlItemViewModel()
        {

        }

        public string Filename { get; set; }

        public SequenceYaml SequenceYaml { get; set; }
    }
}