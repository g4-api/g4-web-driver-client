using System;

namespace G4.WebDriver.Attributes
{
    /// <summary>
    /// Attribute to associate a key name and its corresponding key code.
    /// </summary>
    /// <param name="keyName">The name of the key.</param>
    /// <param name="keyCode">The code associated with the key.</param>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class KeyCodeAttribute(string keyName, int keyCode) : Attribute
    {
        /// <summary>
        /// Gets the key code associated with the key.
        /// </summary>
        public int KeyCode { get; } = keyCode;

        /// <summary>
        /// Gets the name of the key.
        /// </summary>
        public string KeyName { get; } = keyName;
    }
}
