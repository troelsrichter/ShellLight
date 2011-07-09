using System.Windows;
using ShellLight.Contract;
using ShellLight.HowTo.Views;

namespace ShellLight.HowTo.Commands
{
    /// <summary>
    /// Convention: if you place an icon under /Icons/<name of command>.png this will be used
    /// </summary>
    public class HowToUseCustomIconsCommand:UICommand
    {
        public override UIElement DoShow()
        {
            return new HowToUseCustomIconsView();
        }
    }
}