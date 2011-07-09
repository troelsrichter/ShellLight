using System.Windows;
using ShellLight.Contract;
using ShellLight.Contract.Attributes;
using ShellLight.Framework.Views;

namespace ShellLight.ModuleExample.Commands
{
    [RequiresElevatedTrust(Reason = "Uses Html WebBrowser")]
    public class UserFeedbackCommand:UICommand
    {
        public override UIElement DoShow()
        {
            return new UserFeedbackView();
        }
    }
}