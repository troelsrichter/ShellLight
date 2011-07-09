using System;

namespace ShellLight.Contract.Attributes
{
  /// <summary>
  /// Executes the command at startup
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class ExecuteAtStartupAttribute: Attribute
  {
    
  }
}