using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using SkeletonGame.Models;
using SkeletonGameManager.WPF.Events;
using SkeletonGameManager.WPF.Providers;
using SkeletonGameManager.WPF.Views;
using System.Collections.ObjectModel;
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

            if (type == typeof(Combo))
            {
                var combo = Sequence as Combo;
                TextOptions = new ObservableCollection<TestViewModel>()
                {
                    new TestViewModel{Meh = combo.TextList[0]},
                    new TestViewModel{Meh = combo.TextList[1]},
                    new TestViewModel{Meh = combo.TextList[2]}
                };
            }

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

        //public SequenceBase Sequence { get; set; }        

        private SequenceBase sequence;
        public SequenceBase Sequence
        {
            get { return sequence; }
            set { SetProperty(ref sequence, value); }
        }

        private ObservableCollection<TestViewModel> textOptions;
        public ObservableCollection<TestViewModel> TextOptions
        {
            get { return textOptions; }
            set { SetProperty(ref textOptions, value); }
        }
    }

    public class TestViewModel : BindableBase
    {
        private string meh;
        public string Meh
        {
            get { return meh; }
            set { SetProperty(ref meh, value); }
        }
    }
}
