using System.Windows.Controls;
using ShellLight.Contract;

namespace ShellLight.HowTo.Views
{
    public partial class HowToCreateAModalDialogView : UserControl
    {
        private UICommand command;
        
        public HowToCreateAModalDialogView()
        {
            InitializeComponent();
        }

        public HowToCreateAModalDialogView(UICommand command) : this()
        {
          this.command = command;
        }

         private void ButtonClose_Click(object sender, System.Windows.RoutedEventArgs e)
         {
           command.Close();
         }
    }
}
