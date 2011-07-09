using System.Windows;
using System.Windows.Controls;
using ShellLight.Contract;
using ShellLight.HowTo.Events;

namespace ShellLight.HowTo.Views
{
    public partial class HowToPublishEventView : UserControl
    {
        private readonly UICommandContext context;

        public HowToPublishEventView()
        {
            InitializeComponent();
        }

        public HowToPublishEventView(UICommandContext context):this()
        {
            this.context = context;
        }

        private void PublishEventButton_Click(object sender, RoutedEventArgs e)
        {
            context.Events.GetEvent<DemoEvent>().Publish(new Demo() { Text = "Hello from HowToPublishEvent" });
        }
    }
}
