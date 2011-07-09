using System;
using System.Windows.Controls;

namespace ShellLight.Framework.Views
{
    public partial class UserFeedbackView : UserControl
    {
        public UserFeedbackView()
        {
            InitializeComponent();
            MyBrowserControl.Navigate(new Uri("http://shelllight.codeplex.com/Thread/List.aspx"));
        }
    }
}
