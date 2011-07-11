using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using ShellLight.Contract;

namespace ShellLight.ViewModels
{
    public enum LauncherMode
    {
        AllFeatures,
        TopFeatures,
        Search
    } ;

    public class SearchWindowViewModel: ViewModelBase
    {
        public SearchWindowViewModel()
        {
            topScoreCommands = new ObservableCollection<UICommand>();
            searchResultCommands = new ObservableCollection<UICommand>();
        }

        private ObservableCollection<UICommand> topScoreCommands;
        public ObservableCollection<UICommand> TopScoreCommands
        {
            get { return topScoreCommands; }
            set { SetValue(ref topScoreCommands, value, "TopScoreCommands"); }
        }

        private ObservableCollection<UICommand> allCommands;
        public ObservableCollection<UICommand> AllCommands
        {
            get { return allCommands; }
            set { SetValue(ref allCommands, value, "AllCommands"); }
        }

        private ObservableCollection<UICommand> searchResultCommands;
        public ObservableCollection<UICommand> SearchResultCommands
        {
            get { return searchResultCommands; }
            set { SetValue(ref searchResultCommands, value, "SearchResultCommands"); }
        }

        public Visibility TopScoreVisibility
        {
            get { return mode == LauncherMode.TopFeatures && string.IsNullOrEmpty(searchText) ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility SearchResultVisibility
        {
            get { return !string.IsNullOrEmpty(searchText) ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility AllVisibility
        {
            get { return mode == LauncherMode.AllFeatures && string.IsNullOrEmpty(searchText) ? Visibility.Visible : Visibility.Collapsed; }
        }

        private string searchText = string.Empty;
        public string SearchText
        {
            get { return searchText; }
            set { 
                SetValue(ref searchText, value, "SearchText"); 
                Refresh();
            }
        }

        public void Refresh()
        {
            OnPropertyChanged("SearchText", "TopScoreVisibility", "SearchResultVisibility", "AllVisibility");
        }

        public string Title
        {
            get { return Config.ApplicationName + " Launcher"; }
        }

        private LauncherMode mode = LauncherMode.TopFeatures;
        public LauncherMode Mode
        {
            get { return mode; }
            set { SetValue(ref mode, value, "Mode"); }
        }

    }
}