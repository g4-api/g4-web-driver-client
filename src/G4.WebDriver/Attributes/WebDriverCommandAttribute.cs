using System;

namespace G4.WebDriver.Attributes
{
    /// <summary>
    /// Specifies a property as a WebDriver command.
    /// </summary>
    /// <param name="command">The WebDriver command name.</param>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class WebDriverCommandAttribute(string command) : Attribute
    {
        /// <summary>
        /// Gets the WebDriver command name.
        /// </summary>
        public string Command { get; } = command;
    }
}
