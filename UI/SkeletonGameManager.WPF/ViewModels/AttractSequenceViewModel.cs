using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using SkeletonGame.Models;
using SkeletonGameManager.WPF.Events;
using SkeletonGameManager.WPF.Providers;
using SkeletonGameManager.WPF.Views;
using System.Windows.Input;

namespace SkeletonGameManager.WPF.ViewModels
{
    public class AttractSequenceViewModel : BindableBase
    {
        public ISkeletonGameProvider _skeletonGameProvider { get; set; }

        public ICommand SetAttractValueCommand { get; set; }

        public AttractSequenceViewModel(SequenceBase sequence, ISkeletonGameProvider skeletonGameProvider)
        {
            _skeletonGameProvider = skeletonGameProvider;
            
            var type = sequence.GetType();
            Name = type.Name;
            Sequence = sequence;

            SetAttractValueCommand = new DelegateCommand<string>((name) =>
            {
                var meh = Sequence;                
                //AssetSelectDialogView dialog = new AssetSelectDialogView();

                
                //var vm = uc.Resolve(typeof(AssetSelectDialogViewModel));
                //AssetSelectDialogViewModel vm = new AssetSelectDialogViewModel();
                //dialog.ShowDialog();
            });
        }
        
        public string Name { get; set; }
        public SequenceBase Sequence { get; set; }        
    }
}
