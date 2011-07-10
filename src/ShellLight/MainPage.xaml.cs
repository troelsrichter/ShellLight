using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Composite.Events;
using ShellLight.Contract;
using ShellLight.Contract.Attributes;
using ShellLight.ViewModels;
using ShellLight.Views;

namespace ShellLight
{
    public partial class MainPage : UserControl
    {
        [ImportMany(AllowRecomposition = true)]
        public ObservableCollection<UICommand> RegisteredCommands { get; set; }

        private IEventAggregator events;
        private SearchWindow searchWindow;
        private List<Uri> packagesToDownload;
        int numberOfRetriesWhenPackageDownloadFails = Config.NumberOfRetriesWhenPackageDownloadFails;
        private AggregateCatalog mainCatalog;

        public MainPage()
        {
            InitializeComponent();

            DataContext = new MainPageViewModel();
            events = new EventAggregator();
            this.KeyDown += MainPage_KeyDown;
            this.Loaded += MainPage_Loaded;

            DownloadShellLightModulesAsync();
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetFocusToSilverlightPlugin();
        }

        private void SetFocusToSilverlightPlugin()
        {
            if (!Application.Current.IsRunningOutOfBrowser)
            {
                System.Windows.Browser.HtmlPage.Plugin.Invoke("focus");
            }
        }

        /// <summary>
        /// Modules listet in Config.ModuleUris
        /// </summary>
        private void DownloadShellLightModulesAsync()
        {
            mainCatalog = new AggregateCatalog();
          
            packagesToDownload = new List<Uri>(Config.ModuleUris);
            foreach (var uri in packagesToDownload)
            {
                DownloadCatalogAsync(uri);
            }
            
            // Pass the catalog to the MEF and compose the parts
            var container = new CompositionContainer(mainCatalog);
            container.ComposeParts(this);
        }

        private void DownloadCatalogAsync(Uri uri)
        {
            var catalog = new DeploymentCatalog(uri);
            catalog.DownloadCompleted += catalog_DownloadCompleted;
            catalog.DownloadAsync();
            mainCatalog.Catalogs.Add(catalog);
        }

        void catalog_DownloadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            var catalog = sender as DeploymentCatalog;

            if (!e.Cancelled && (e.Error == null))
            {
                packagesToDownload.Remove(catalog.Uri);
                InitializeCommands();
            }
            else
            {
                //when running out of browser retires are necessary
                if (numberOfRetriesWhenPackageDownloadFails > 0)
                {
                    --numberOfRetriesWhenPackageDownloadFails;
                    mainCatalog.Catalogs.Remove(catalog);
                    DownloadCatalogAsync(catalog.Uri);
                }
                else
                {
                    var message =
                         string.Format("Could not load module {0} after {1} retries.\n\n" +
                                       "If you are a developer please ensure that ShellLight.Web is your startup project",
                                      catalog.Uri, Config.NumberOfRetriesWhenPackageDownloadFails);
                    ViewHelper.ShowError(message, "Could not load module");
                }
            }
        }

       private void InitializeCommands()
        {
            foreach (var command in RegisteredCommands.Where(c => c.State == UICommandState.Created))
            {
                AttachScore(command);
                RegisterCommandEvents(command);
            }

            //when all commands are able to be executed within a module behaviours are applied
            foreach (var command in RegisteredCommands.Where(c => c.State == UICommandState.Created))
            {
              SetupBehaviorBasedOnAttributes(command);
              PinCommand(command); //pincommands depends on behaviors and is therefore called last
              command.State = UICommandState.Initialized;
            }
  
        }

        private void SetupBehaviorBasedOnAttributes(UICommand command)
        {
            if (command.HasAttribute<PinToTaskbarAttribute>())
            {
                if (!Config.IsolatedStorage.PinnedCommands.ContainsKey(command.Id))
                {
                    var datacontext = DataContext as MainPageViewModel;
                    Config.IsolatedStorage.PinnedCommands[command.Id] = true;
                    //datacontext.TaskbarCommands.Add(command);
                }
            }

            if (command.HasAttribute<AttachToTrayAttribute>())
            {
                //MessageBox.Show("attach to tray " + command.Name);
                var context = new UICommandContext() { Events = events, RegiseredCommands = RegisteredCommands.ToList() };
                command.Execute(context);
            }

            if (command.HasAttribute<ExecuteAtStartupAttribute>())
            {
              var context = new UICommandContext() { Events = events, RegiseredCommands = RegisteredCommands.ToList() };
              command.Execute(context);
            }
           
            if (command.HasAttribute<KeyboardShortcutAttribute>())
            {
                var attribute = command.GetAttribute<KeyboardShortcutAttribute>();
                command.AddParameterDiscription(attribute.ToString());
            }
        }

        private void RegisterCommandEvents(UICommand command)
        {
            command.BeforeShow += command_BeforeShow;
            command.AfterShow += command_AfterShow;
            command.Closing += command_Closing;
        }

        private void AttachScore(UICommand command)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(command.Id))
            {
                command.Score = (int)IsolatedStorageSettings.ApplicationSettings[command.Id];
                //MessageBox.Show(command.Name + "->" + command.Score.ToString());
            }
        }

        private void PinCommand(UICommand command)
        {
            if (Config.IsolatedStorage.PinnedCommands.ContainsKey(command.Id) && Config.IsolatedStorage.PinnedCommands[command.Id])
            {
                var datacontext = DataContext as MainPageViewModel;
                var context = new UICommandContext() { Events = events, RegiseredCommands = RegisteredCommands.ToList() };
                command.Context = context;
                datacontext.TaskbarCommands.Add(command);
            }
        }

        private void IncrementScore(UICommand command)
        {
            var score = 1;
            if (IsolatedStorageSettings.ApplicationSettings.Contains(command.Id))
            {
                score = (int)IsolatedStorageSettings.ApplicationSettings[command.Id] + 1;
            }
            //MessageBox.Show("issolated storage update hits " + score);
            IsolatedStorageSettings.ApplicationSettings[command.Id] = score;
        }

        void command_BeforeShow(object sender, EventArgs e)
        {
            if (searchWindow != null)
            {
                searchWindow.Close();
            }
        }

        void command_AfterShow(object sender, UICommandEventArgs e)
        {
            var datacontext = DataContext as MainPageViewModel;
            var command = sender as UICommand;

            if (command != null)
            {
                IncrementScore(command);

                if (command.View != null)
                {
                    if (command.HasAttribute<IsModalAttribute>())
                    {
                        var modalWindow = new ModalWindow(command,
                                                          command.GetAttribute<IsModalAttribute>().HasCloseButton);
                        modalWindow.Show();
                        modalWindow.Closed += modalWindow_Closed;
                    }
                    else if (command.HasAttribute<AttachToTrayAttribute>())
                    {
                        if (!datacontext.TrayCommands.Contains(command))
                        {
                            datacontext.TrayCommands.Add(command);
                        }
                        else
                        {
                            var trayWindow = new TrayWindow(command.View, command.Name);
                            trayWindow.Show();
                        }
                    }
                    else
                    {
                        datacontext.CommandInFocus = command;

                        if (!datacontext.TaskbarCommands.Contains(command))
                        {
                            datacontext.TaskbarCommands.Add(command);
                        }
                    }
                }
            }
        }

        void modalWindow_Closed(object sender, EventArgs e)
        {
            //set focus back to the command that was in focus before the modal window was opened
            var datacontext = DataContext as MainPageViewModel;
            if (datacontext != null)
            {
                datacontext.SetFocus();
            }
        }

        void MainPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.F)
            {
                OpenSearchWindow();
            } else
            {
                //look for KeyboardShortcutAttribute and execute command if found
                var result = (from c in RegisteredCommands where c.HasAttribute<KeyboardShortcutAttribute>() && c.GetAttribute<KeyboardShortcutAttribute>().ModifierKey == Keyboard.Modifiers &&  c.GetAttribute<KeyboardShortcutAttribute>().Key == e.Key select c).FirstOrDefault();
                if (result != null)
                {
                    var context = new UICommandContext() { Events = events, RegiseredCommands = RegisteredCommands.ToList() };
                    result.Execute(context);
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            OpenSearchWindow();
        }

        private void OpenSearchWindow()
        {
            searchWindow = new SearchWindow(RegisteredCommands, events);
            searchWindow.VerticalAlignment = VerticalAlignment.Bottom;
            searchWindow.HorizontalAlignment = HorizontalAlignment.Left;
            searchWindow.Show();
            searchWindow.Closed += modalWindow_Closed;
            searchWindow.CommandPinnedToTaskbar += searchWindow_CommandPinnedToTaskbar;
        }

        void searchWindow_CommandPinnedToTaskbar(object sender, UICommandEventArgs e)
        {
            var datacontext = DataContext as MainPageViewModel;
            Config.IsolatedStorage.PinnedCommands[e.Command.Id] = true;
            if (!datacontext.TaskbarCommands.Contains(e.Command))
            {
                datacontext.TaskbarCommands.Add(e.Command);
            }
        }

        private void CloseCommandButton_Click(object sender, RoutedEventArgs e)
        {
            var datacontext = DataContext as MainPageViewModel;
            if (datacontext.CommandInFocus != null)
            {
                RemoveFromTaskbarIfCommandIsNotPinned(datacontext.CommandInFocus);
                datacontext.CommandInFocus = null;
            }
        }

        private void RemoveFromTaskbarIfCommandIsNotPinned(UICommand command)
        {
            var datacontext = DataContext as MainPageViewModel;
            if (!(Config.IsolatedStorage.PinnedCommands.ContainsKey(command.Id) && Config.IsolatedStorage.PinnedCommands[command.Id]))
            {
                datacontext.TaskbarCommands.Remove(command);
            }
        }

        void command_Closing(object sender, EventArgs e)
        {
            var datacontext = DataContext as MainPageViewModel;
            var command = sender as UICommand;
            if (datacontext.TaskbarCommands.Contains(command))
            {
                RemoveFromTaskbarIfCommandIsNotPinned(command);
                if (datacontext.CommandInFocus == command)
                {
                    datacontext.CommandInFocus = null;
                }
            }
        }

        private void UnpinFromTaskbar_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var pinnedCommand = menuItem.DataContext as UICommand;
                var datacontext = DataContext as MainPageViewModel;
                datacontext.TaskbarCommands.Remove(pinnedCommand);
                Config.IsolatedStorage.PinnedCommands[pinnedCommand.Id] = false;
            }
        }
    }
}
