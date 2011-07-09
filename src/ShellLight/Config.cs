using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;

namespace ShellLight
{
    public static class Config
    {
        public static class IsolatedStorage
        {
            public const string WindowState = "Shelllight_WindowState";
            public const string WindowTop = "Shelllight_WindowTop";
            public const string WindowLeft = "Shelllight_WindowLeft";
            public const string WindowWidth = "Shelllight_WindowWidth";
            public const string WindowHeight = "Shelllight_WindowHeight";
            private const string PinnedCommandsKey = "Shelllight_PinnedCommands";

            public static Dictionary<string,bool> PinnedCommands
            {
                get
                {
                    Dictionary<string, bool> pinnedCommands;
                    if (!IsolatedStorageSettings.ApplicationSettings.Contains(PinnedCommandsKey))
                    {
                        pinnedCommands = new Dictionary<string,bool>();
                        IsolatedStorageSettings.ApplicationSettings[PinnedCommandsKey] = pinnedCommands;
                        IsolatedStorageSettings.ApplicationSettings.Save();
                    } else
                    {
                        pinnedCommands = IsolatedStorageSettings.ApplicationSettings[PinnedCommandsKey] as Dictionary<string, bool>;
                    }
                    return pinnedCommands;
                }
            }
        }

        /// <summary>
        /// Uris of all the modules
        /// use uris.add to add a new module to the application
        /// </summary>
        public static List<Uri> ModuleUris
        {
            get { 
                var uris = new List<Uri>(); 
                uris.Add(new Uri("ShellLight.Framework.xap", UriKind.Relative));
                uris.Add(new Uri("ShellLight.HowTo.xap", UriKind.Relative)); //todo ShellLight.HowTo.xap should be removed before you deploy you application
                return uris;}
        }

        public static string ApplicationName
        {
            get { return "ShellLight"; }
        }

        public static string BackgroundImageSource
        {
            get { return "http://www.tdfast.com/wallpapers_res1/439_10672_1.jpg"; }
        }

        public static string SupportEmail
        {
            get { return "http://shelllight.development@gmail.com"; }
         }

        public static int NumberOfRetriesWhenPackageDownloadFails
        {
            get { return 3; }
        }
    }
}
