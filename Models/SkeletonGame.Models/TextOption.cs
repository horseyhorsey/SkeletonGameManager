using System.Collections.Generic;
using System.Collections.ObjectModel;
using YamlDotNet.Serialization;

namespace SkeletonGame.Models
{
    /// <summary>
    /// Interface for sequences with text lists.
    /// </summary>
    public interface ITextEntries
    {
        List<string> TextList { get; set; }

        ObservableCollection<TextListViewModel> TextEntries { get; set; }
    }

    public class Message
    {
        [YamlMember(Alias = "TextOptions", ApplyNamingConventions = false)]
        public List<TextOption> TextOptions { get; set; }
    }

    public class TextListViewModel
    {
        public string TextLine { get; set; } = "";
    }

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class TextOption : ITextEntries
    {
        private List<string> _textList;
        [YamlMember(Alias = "Text", ApplyNamingConventions = false)]
        public List<string> TextList
        {
            get
            {
                return _textList;
            }

            set
            {
                _textList = value;

                if (_textList != null)
                {
                    TextEntries.Clear();
                    foreach (var textLine in _textList)
                    {
                        TextEntries.Add(new TextListViewModel { TextLine = textLine });
                    }
                }
                else
                {
                    TextEntries.Clear();
                }
            }
        }

        [YamlIgnore]
        public ObservableCollection<TextListViewModel> TextEntries { get; set; } =
        new ObservableCollection<TextListViewModel>();
    }
}
