using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ShellLight.Contract;

namespace ShellLight.ViewModels
{
    public class MainPageViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainPageViewModel()
        {
            taskbarCommands = new ObservableCollection<UICommand>();
            trayCommands = new ObservableCollection<UICommand>();
        }

        private ObservableCollection<UICommand> taskbarCommands;
        public ObservableCollection<UICommand> TaskbarCommands
        {
            get { return taskbarCommands; }
            set
            {
                if (taskbarCommands != value)
                {
                    taskbarCommands = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("TaskbarCommands"));
                }
            }
        }

        private UICommand commandInFocus = null;
        public UICommand CommandInFocus
        {
            get { return commandInFocus; }
            set
            {
                if (value != commandInFocus)
                {
                    commandInFocus = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("CommandInFocus"));
                    PropertyChanged(this, new PropertyChangedEventArgs("CommandInFocusVisibility"));
                    SetFocus();
                }
            }
        }

        public void SetFocus()
        {
            if (commandInFocus != null && commandInFocus.View != null)
            {
                var control = commandInFocus.View as UserControl;
                if (control != null)
                {
                    control.Dispatcher.BeginInvoke(() => control.Focus());
                }
            }
        }

        public Visibility CommandInFocusVisibility
        {
            get { return commandInFocus != null ? Visibility.Visible : Visibility.Collapsed; }
        }

        private ObservableCollection<UICommand> trayCommands;
        public ObservableCollection<UICommand> TrayCommands
        {
            get { return trayCommands; }
            set
            {
                if (trayCommands != value)
                {
                    trayCommands = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("TrayCommands"));
                }
            }
        }

        private string backgroundImageSource = Config.BackgroundImageSource;
        public string BackgroundImageSource
        {
            get { return backgroundImageSource; }
            set
            {
                if (value != backgroundImageSource)
                {
                    backgroundImageSource = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("BackgroundImageSource"));
                }
            }
        }

    }
}