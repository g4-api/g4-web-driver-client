using System;

namespace G4.WebDriver.Attributes
{
    /// <summary>
    /// Specifies a class as a WebDriver commands container.
    /// </summary>
    /// <param name="name">The commands container name.</param>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class WebDriverCommandsContainerAttribute(string name) : Attribute
    {
        /// <summary>
        /// Gets the commands container name.
        /// </summary>
        public string Name { get; } = name;
    }
}
