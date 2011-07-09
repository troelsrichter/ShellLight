using System.Windows;
using ShellLight.Contract;
using ShellLight.Contract.Attributes;
using ShellLight.HowTo.Views;

namespace ShellLight.HowTo.Commands
{
    [PinToTaskbar]
    public class HowToCreateAModalDialogCommand: UICommand
    {
        public override UIElement DoShow()
        {
            IsModal = true;
            return new HowToCreateAModalDialogView(this);
        }

    }
}