using System;

namespace ShellLight.Contract.Attributes
{
    /// <summary>
    /// Executes the command at startup and attaches it to the tray taskbar
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AttachToTrayAttribute:Attribute
    {
        
    }
}