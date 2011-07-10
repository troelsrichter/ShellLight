using System;

namespace ShellLight.Contract.Attributes
{
    /// <summary>
    /// Opens the view as a modal dialog
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class IsModalAttribute: Attribute
    {
        public bool HasCloseButton { get; private set; }

        public IsModalAttribute():this(true)
        {
        }

        /// <param name="hasCloseButton">Default true</param>
        public IsModalAttribute(bool hasCloseButton)
        {
            this.HasCloseButton = hasCloseButton;
        }
    }
}