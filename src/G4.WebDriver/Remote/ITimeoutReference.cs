using System;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Represents an interface for objects that have a timeout property.
    /// </summary>
    public interface ITimeoutReference
    {
        /// <summary>
        /// Gets the timeout duration.
        /// </summary>
        TimeSpan Timeout { get; }
    }
}
