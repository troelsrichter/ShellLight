using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShellLight.Views
{
    public partial class TrayWindow : ChildWindow
    {
        public TrayWindow()
        {
            InitializeComponent();
            this.Closing += TrayWindow_Closing;
            this.KeyDown += TrayWindow_KeyDown;
        }

        public TrayWindow(UIElement view, string title)
            : this()
        {
            Content.Content = view;
            Title = title;
        }

        void TrayWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        void TrayWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Content.Content = null;
        }
    }
}

