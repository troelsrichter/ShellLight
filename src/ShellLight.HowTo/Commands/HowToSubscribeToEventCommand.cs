using System.Windows;
using ShellLight.Contract;
using ShellLight.HowTo.Views;

namespace ShellLight.HowTo.Commands
{
    public class HowToSubscribeToEventCommand: UICommand
    {
        private HowToSubscribeToEventView view;

        public override UIElement DoShow()
        {
            return view ?? (view = new HowToSubscribeToEventView(Context));
        }
    }
}