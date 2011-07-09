using System;

namespace ShellLight.Contract.Attributes
{
    public enum VisibilityType { Visible, Hidden } ;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class LauncherAttribute: Attribute
    {
        public LauncherAttribute(VisibilityType visibilityType)
        {
            VisibilityType = visibilityType;
        }

        public VisibilityType VisibilityType { get; set; }
        
    }
}