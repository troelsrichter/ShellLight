using System.Windows.Controls;
using ShellLight.Contract;
using ShellLight.HowTo.Events;

namespace ShellLight.HowTo.Views
{
    public partial class HowToSubscribeToEventView : UserControl
    {
        private readonly UICommandContext context;

        public HowToSubscribeToEventView()
        {
            InitializeComponent();
        }

        public HowToSubscribeToEventView(UICommandContext context): this()
        {
            this.context = context;
            context.Events.GetEvent<DemoEvent>().Subscribe(DemoEventHandler);
        }

        /// <summary>
        /// Remember: eventhandler has to be public
        /// </summary>
        public void DemoEventHandler(Demo e)
        {
            eventTextBox.Text = e.Text;
        }
    }
}
