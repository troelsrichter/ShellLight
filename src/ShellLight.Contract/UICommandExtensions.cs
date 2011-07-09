using System;
using System.Windows;

namespace ShellLight.Contract
{
    public static class UICommandExtensions
    {
        public static bool HasAttribute<T>(this UICommand uiCommand) where T : Attribute
        {
            var attribute = uiCommand.GetType().GetCustomAttributes(typeof(T), true);
            return (attribute != null && attribute.Length > 0);
        }

        public static T GetAttribute<T>(this UICommand uiCommand) where T : Attribute
        {
            var attribute = uiCommand.GetType().GetCustomAttributes(typeof(T), true);
            return (T)attribute[0];
        }

        public static string ConventionalName(this UICommand uiCommand)
        {
            var name = System.Text.RegularExpressions.Regex.Replace(uiCommand.GetType().Name, "([A-Z])", " $1").Trim();
            if (name.EndsWith("Command"))
            {
                name = name.Substring(0, name.Length - 8);
            }
            return name;
        }

        public static string ConventionalIconSource(this UICommand uiCommand)
        {
            var iconFileName = uiCommand.GetType().Name;
            if (iconFileName.EndsWith("Command"))
            {
                iconFileName = iconFileName.Substring(0, iconFileName.Length - 7);
                iconFileName += ".png";
            }

            var assemblyFullName = uiCommand.GetType().Assembly.FullName;
            var assemblyName = assemblyFullName.Substring(0, assemblyFullName.IndexOf(','));
            var iconsource = "/" + assemblyName + ";component/Icons/" + iconFileName;

            var streamResourceInfo = Application.GetResourceStream(new Uri(iconsource, UriKind.Relative));
            if (streamResourceInfo != null)
            {
                return iconsource;
            }
            return "/ShellLight;component/Images/default.png";
        }
    }
}