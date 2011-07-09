using System.Windows;
using ShellLight.Contract;
using ShellLight.HowTo.Views;

namespace ShellLight.HowTo.Commands
{
    public class HowToCreateACommandCommand: UICommand
    {
        public override UIElement DoShow()
        {
            //when a command is executed this method is hit
            return new HowToCreateACommandView();
        }
    }
}