using System;
using System.Windows.Input;

namespace ShellLight.Contract.Attributes
{
    /// <summary>
    /// This attribute can be applied to commands in order to execute it with a keyboard shortcut
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class KeyboardShortcutAttribute: Attribute
    {
        public Key Key { get; private set; }
        public ModifierKeys ModifierKey { get; private set; }

        public KeyboardShortcutAttribute(ModifierKeys modifierKey, Key key)
        {
            this.Key = key;
            this.ModifierKey = modifierKey;
        }

        public override string ToString()
        {
            var modifierKey = ModifierKey == ModifierKeys.Control ? "Ctrl" : ModifierKey.ToString();
            return modifierKey + "+" + this.Key;
        }
    }
}