using SkeletonGameManager.Base.Interface;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SkeletonGameManager.Module.Menus.Model
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class AppSettingsModel : IAppSettingsModel
    {
        public ObservableCollection<string> RecentDirectories { get; set; }

        public AppSettingsModel()
        {
            RecentDirectories = new ObservableCollection<string>();
        }

        public IList<string> Load()
        {            
            RecentDirectories.Add(Properties.Settings.Default.RecentDir5);
            RecentDirectories.Add(Properties.Settings.Default.RecentDir4);
            RecentDirectories.Add(Properties.Settings.Default.RecentDir3);
            RecentDirectories.Add(Properties.Settings.Default.RecentDir2);
            RecentDirectories.Add(Properties.Settings.Default.RecentDir1);

            return RecentDirectories;
        }

        public void Save()
        {            
            Properties.Settings.Default.RecentDir1 = RecentDirectories[4];
            Properties.Settings.Default.RecentDir2 = RecentDirectories[3];
            Properties.Settings.Default.RecentDir3 = RecentDirectories[2];
            Properties.Settings.Default.RecentDir4 = RecentDirectories[1];
            Properties.Settings.Default.RecentDir5 = RecentDirectories[0];
            Properties.Settings.Default.Save();
        }

        public void AddRecentDirectory(string gameFolder)
        {
            //Set recent directory and save
            if (RecentDirectories?.Count > 0)
            {
                //Check if folder exists
                var dupeFolder = RecentDirectories.FirstOrDefault(x => x == gameFolder);
                if (dupeFolder == null)
                    RecentDirectories.RemoveAt(RecentDirectories.Count - 1);
                else
                    RecentDirectories.Remove(gameFolder);

                RecentDirectories.Insert(0, gameFolder);
            }

            Save();
        }
    }
}
