using System.Windows;
using ShellLight.Contract;
using ShellLight.Contract.Attributes;
using ShellLight.HowTo.Views;

namespace ShellLight.HowTo.Commands
{
    [PinToTaskbar]
    [IsModal]
    public class HowToCreateAModalDialogCommand: UICommand
    {
        public override UIElement DoShow()
        {
            return new HowToCreateAModalDialogView(this);
        }
    }
}