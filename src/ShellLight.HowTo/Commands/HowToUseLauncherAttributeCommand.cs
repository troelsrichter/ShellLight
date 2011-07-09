using System.Windows;
using ShellLight.Contract;
using ShellLight.Contract.Attributes;

namespace ShellLight.HowTo.Commands
{
    [Launcher(VisibilityType.Hidden)]
    public class HowToUseLauncherAttributeCommand: UICommand
    {
        public override UIElement DoShow()
        {
            return null;
        }
    }
}