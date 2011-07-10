using System.Windows.Controls;
using System.Windows.Input;
using ShellLight.Contract;

namespace ShellLight.Views
{
    public partial class ModalWindow : ChildWindow
    {
        private UICommand command;
        bool isWindowClosing = false;
 
        public ModalWindow()
        {
            InitializeComponent();
            this.Closing += ModalWindow_Closing;
            this.KeyDown += ModalWindow_KeyDown;
            this.GotFocus += ModalWindow_GotFocus;
        }

        void ModalWindow_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            this.GotFocus -= ModalWindow_GotFocus;
            //ensure that the usercontrol gets the focus if loaded modal
            var userControl = Content.Content as UserControl;
            if (userControl != null)
            {
                userControl.Focus();
            }
        }

        public ModalWindow(UICommand command, bool hasCloseButton):this()
        {
            this.command = command;
            this.HasCloseButton = hasCloseButton;
            Content.Content = command.View;
            Title = command.Name;
            command.Closing += command_Closing;
        }

        void command_Closing(object sender, System.EventArgs e)
        {
            command.Closing -= command_Closing;
            if (!isWindowClosing)
            {
                this.Close();
            }
        }

        void ModalWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
               command.Close();
            }
        }

        void ModalWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isWindowClosing = true;
            //if modal window is closed using the X button ensure to close command
            if (command.State == UICommandState.Active)
            {
                command.Close();
            }
        }
    }
}

