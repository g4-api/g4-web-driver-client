using System;

namespace G4.WebDriver.Attributes
{
    /// <summary>
    /// Represents an attribute that defines WebDriver options for a specific vendor.
    /// </summary>
    /// <param name="prefix">The vendor prefix for the WebDriver options.</param>
    /// <param name="optionsKey">The options key for the WebDriver options.</param>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class WebDriverOptionsAttribute(string prefix, string optionsKey) : Attribute
    {
        /// <summary>
        /// Gets or sets the options key for the WebDriver options.
        /// </summary>
        public string OptionsKey { get; } = optionsKey;

        /// <summary>
        /// Gets the vendor prefix for the WebDriver options.
        /// </summary>
        public string Prefix { get; } = prefix;
    }
}
