using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using ShellLight.Contract;

namespace ShellLight.ViewModels
{
    public class SearchWindowViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SearchWindowViewModel()
        {
            topScoreCommands = new ObservableCollection<UICommand>();
            searchResultCommands = new ObservableCollection<UICommand>();
        }

        private ObservableCollection<UICommand> topScoreCommands;
        public ObservableCollection<UICommand> TopScoreCommands
        {
            get { return topScoreCommands; }
            set
            {
                if (topScoreCommands != value)
                {
                    topScoreCommands = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("TopScoreCommands"));
                }
            }
        }

        private ObservableCollection<UICommand> searchResultCommands;
        public ObservableCollection<UICommand> SearchResultCommands
        {
            get { return searchResultCommands; }
            set
            {
                if (searchResultCommands != value)
                {
                    searchResultCommands = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("SearchResultCommands"));
                }
            }
        }

        public Visibility TopScoreVisibility
        {
            get { return string.IsNullOrEmpty(searchText) ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility SearchResultVisibility
        {
            get { return !string.IsNullOrEmpty(searchText) ? Visibility.Visible : Visibility.Collapsed; }
        }

        private string searchText = string.Empty;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (value != searchText)
                {
                    searchText = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("SearchText"));
                    PropertyChanged(this, new PropertyChangedEventArgs("TopScoreVisibility"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SearchResultVisibility"));
                }
            }
        }

        public string Title
        {
            get { return Config.ApplicationName + " Launcher"; }
        }
    }
}