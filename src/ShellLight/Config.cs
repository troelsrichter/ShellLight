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
            private const string BackgroundImageUrlKey = "Shelllight_BackgroundImageUrl";

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

            public static string BackgroundImageUrl 
            {
                get
                {
                    if (IsolatedStorageSettings.ApplicationSettings.Contains(BackgroundImageUrlKey))
                    {
                        return (string) IsolatedStorageSettings.ApplicationSettings[BackgroundImageUrlKey];
                    }
                    return String.Empty;
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
                //uris.Add(new Uri("ShellLight.Framework.xap", UriKind.Relative));
                //uris.Add(new Uri("ShellLight.HowTo.xap", UriKind.Relative));
                uris.Add(new Uri("Kanbana.Client_1_6.xap", UriKind.Relative));
                return uris;
            }
        }

        public static string ApplicationName
        {
            get { return "Kanbana"; }
        }

        public static string BackgroundImageSource
        {
            get
            {
                var background = IsolatedStorage.BackgroundImageUrl;
                return background != string.Empty ? background : "http://resources.kanbana.com/wallpapers/kanbana_wallpaper_1.jpg";
            }

            //"http://www.tdfast.com/wallpapers_res1/431_9923_2.jpg";
            //get { return "http://www.tdfast.com/wallpapers_res1/439_10672_1.jpg"; }
            //get { return "http://www.tdfast.com/wallpapers_res1/1_6474_2.jpg"; }
            //get { return "http://www.tdfast.com/wallpapers_res1/22244.jpg"; }
            //get { return "http://www.tdfast.com/wallpapers_res1/431_9908_6.jpg"; }
            //get { return "http://www.tdfast.com/wallpapers_res1/389_8814_2.jpg"; }
            //get { return "http://www.tdfast.com/wallpapers_res1/389_8751_3.jpg"; }
        }

        public static string SupportEmail
        {
            get { return "support@kanbana.com"; }
        }

        public static int NumberOfRetriesWhenPackageDownloadFails
        {
            get { return 3; }
        }
    }
}
