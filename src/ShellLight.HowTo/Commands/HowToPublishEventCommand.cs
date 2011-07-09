using System.Windows;
using ShellLight.Contract;
using ShellLight.HowTo.Views;

namespace ShellLight.HowTo.Commands
{
    public class HowToPublishEventCommand: UICommand
    {
        private HowToPublishEventView view;

        public override UIElement DoShow()
        {
            return view ?? (view = new HowToPublishEventView(Context));
        }
    }
}