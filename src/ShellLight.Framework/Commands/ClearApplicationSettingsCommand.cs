using System.IO.IsolatedStorage;
using System.Windows;
using ShellLight.Contract;

namespace ShellLight.Framework.Commands
{
    public class ClearApplicationSettingsCommand : UICommand
    {

        public override UIElement DoShow()
        {
            IsolatedStorageSettings.ApplicationSettings.Clear();
            return null;
        }
    }
}