using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SkeletonGameManager.Base.Interface
{
    public interface IAppSettingsModel
    {
        ObservableCollection<string> RecentDirectories { get; set; }
        IList<string> Load();
        void Save();
        void AddRecentDirectory(string gameFolder);
    }

}
