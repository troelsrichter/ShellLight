using System.Windows;
using ShellLight.Contract;
using ShellLight.HowTo.Views;

namespace ShellLight.HowTo.Commands
{
    public class HowToCallAnotherCommandCommand : UICommand
    {
        public override UIElement DoShow()
        {
            return new HowToCallAnotherCommandView(Context);
        }
    }
}