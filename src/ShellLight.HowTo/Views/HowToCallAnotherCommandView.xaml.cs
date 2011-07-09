using System.Windows;
using System.Windows.Controls;
using ShellLight.Contract;
using ShellLight.HowTo.Commands;

namespace ShellLight.HowTo.Views
{
    public partial class HowToCallAnotherCommandView : UserControl
    {
        private readonly UICommandContext context;

        public HowToCallAnotherCommandView()
        {
            InitializeComponent();
        }

        public HowToCallAnotherCommandView(UICommandContext context):this()
        {
            this.context = context;
        }

        private void LookupCommandButton_Click(object sender, RoutedEventArgs e)
        {
            var command = context.LookUpCommand<HowToCreateAModalDialogCommand>();
            command.Show();
        }
    }
}
