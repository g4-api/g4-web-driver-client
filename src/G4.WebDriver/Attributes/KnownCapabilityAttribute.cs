using System;

namespace G4.WebDriver.Attributes
{
    /// <summary>
    /// Specifies a property as a Known Capability.
    /// </summary>
    /// <param name="capabilityName">The Known Capability name.</param>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class KnownCapabilityAttribute(string capabilityName) : Attribute
    {
        /// <summary>
        /// Gets or sets the Known Capability name.
        /// </summary>
        public string CapabilityName { get; set; } = capabilityName;
    }
}
