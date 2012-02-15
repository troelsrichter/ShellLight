using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Composite.Events;
using ShellLight.Contract;
using ShellLight.ViewModels;

namespace ShellLight.Views
{
    public partial class SearchWindow : ChildWindow
    {
        public event EventHandler<UICommandEventArgs> CommandPinnedToTaskbar;
        private readonly ObservableCollection<UICommand> launcherCommands;
        private readonly ObservableCollection<UICommand> registeredCommands;

        private readonly IEventAggregator events;

        public SearchWindow()
        {
            InitializeComponent();
            this.searchTextBox.KeyDown += searchTextBox_KeyDown;
            this.searchTextBox.TextChanged += searchTextBox_TextChanged;
        }

        protected override void OnOpened()
        {
            base.OnOpened();
            searchTextBox.Focus();
        }

        void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var datacontext = DataContext as SearchWindowViewModel;

            var listbox = datacontext.SearchResultVisibility == Visibility.Visible ? searchResultListBox : datacontext.TopScoreVisibility == Visibility.Visible ? topScoreListBox : allListBox;

            if (listbox != null)
            {
                if (e.Key == Key.Escape)
                {
                    this.Close();
                }

                if ((listbox.SelectedItem == null) && (listbox.Items.Count == 1))
                {
                    listbox.SelectedIndex = 0;
                }

                if (listbox.Items.Count > 0)
                {
                    if (e.Key == Key.Enter)
                    {
                        var selectedCommand = listbox.SelectedItem as UICommand;
                        if (selectedCommand != null)
                        {
                            if (selectedCommand.CanExecute(null))
                            {
                                selectedCommand.Execute(selectedCommand.Context);
                            }
                        }
                    }
                    else if (e.Key == Key.Up)
                    {
                        if (listbox.SelectedItem != null)
                        {
                            listbox.SelectedIndex = listbox.SelectedIndex > 0 ? listbox.SelectedIndex - 1 : 0;
                        }
                        else
                        {
                            listbox.SelectedIndex = listbox.Items.Count - 1;
                        }
                    }
                    else if (e.Key == Key.Down)
                    {
                        if (listbox.SelectedItem != null)
                        {
                            listbox.SelectedIndex = listbox.Items.Count > listbox.SelectedIndex + 1
                                                      ? listbox.SelectedIndex + 1
                                                      : listbox.SelectedIndex;
                        }
                        else
                        {
                            listbox.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        public SearchWindow(ObservableCollection<UICommand> commands, IEventAggregator events)
            : this()
        {
            this.registeredCommands = commands;
            this.launcherCommands = CommandFinder.FilterCommands(commands);
            this.events = events;
            InitializeDataContext(launcherCommands, events);
        }

        private void InitializeDataContext(IEnumerable<UICommand> commands, IEventAggregator events)
        {
            var datacontext = new SearchWindowViewModel();

            var top3Commands = CommandFinder.CreateTop3Commands(commands);
            foreach (var command in top3Commands)
            {
                var executionContext = new UICommandContext() { Events = events, RegiseredCommands = new List<UICommand>(registeredCommands) };
                command.Context = executionContext;
                datacontext.TopScoreCommands.Add(command);
            }
            DataContext = datacontext;
        }

        void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            var datacontext = DataContext as SearchWindowViewModel;
            datacontext.SearchResultCommands.Clear();

            string parameter;
            var foundCommands = CommandFinder.Find(textBox.Text, launcherCommands.ToList(), out parameter);

            foreach (var command in foundCommands)
            {
                var executionContext = new UICommandContext() { Parameter = parameter, Events = events, RegiseredCommands = new List<UICommand>(registeredCommands) };
                command.Context = executionContext;
                datacontext.SearchResultCommands.Add(command);
            }

            textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void PinToTaskbar_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var commandToBePinnned = menuItem.DataContext as UICommand;

                if (CommandPinnedToTaskbar != null)
                {
                    CommandPinnedToTaskbar(this, new UICommandEventArgs(commandToBePinnned));
                }
            }
        }

        private void AllFeaturesButton_Click(object sender, RoutedEventArgs e)
        {
            var datacontext = DataContext as SearchWindowViewModel;
            datacontext.AllCommands = new ObservableCollection<UICommand>(launcherCommands);
            datacontext.Mode = LauncherMode.AllFeatures;
            datacontext.Refresh();
            searchTextBox.Focus();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var datacontext = DataContext as SearchWindowViewModel;
            datacontext.Mode = LauncherMode.TopFeatures;
            datacontext.Refresh();
            searchTextBox.Focus();
        }
    }
}

