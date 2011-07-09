using System.Windows;
using System.Windows.Input;
using ShellLight.Contract;
using ShellLight.Contract.Attributes;

namespace ShellLight.HowTo.Commands
{
    [KeyboardShortcut(ModifierKeys.Control, Key.N)]
    public class HowToUseKeyboardShortcutsCommand: UICommand
    {
        public override UIElement DoShow()
        {
            MessageBox.Show("HowToUseKeyboardShortcutsCommand executed!");
            return null;
        }
    }
}