using System;

namespace ShellLight.Contract.Attributes
{
    /// <summary>
    /// Ensures that the command only can be executed in elevated trust mode (Out of Browser)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RequiresElevatedTrustAttribute: Attribute
    {
        public string Reason { get; set; }
    }
}