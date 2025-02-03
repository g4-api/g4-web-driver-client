using System;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// A set of flags to point out on which scope to add a capability.
    /// </summary>
    [Flags]
    public enum CapabilityScope
    {
        /// <summary>
        /// Add to "capabilities" collection.
        /// </summary>
        None = 0,

        /// <summary>
        /// Add to "capabilities.alwaysMatch" collection.
        /// </summary>
        FirstMatch = 2,

        /// <summary>
        /// Add to "capabilities.firstMatch" collection.
        /// </summary>
        AlwaysMatch = 4
    }
}
