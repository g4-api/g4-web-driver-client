/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/ITimeouts.cs
 * https://www.w3.org/TR/webdriver1/#set-timeouts
 */
using System;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// A timeouts configuration is a record of the different timeouts that control
    /// the behavior of script evaluation, navigation, and element retrieval.
    /// </summary>
    public interface ITimeouts
    {
        /// <summary>
        /// Gets or sets the time to wait for the element location strategy to complete when locating an element.
        /// </summary>
        TimeSpan Implicit { get; set; }

        /// <summary>
        /// Gets or sets the timeout limit used to interrupt an explicit navigation attempt.
        /// </summary>
        TimeSpan PageLoad { get; set; }

        /// <summary>
        /// Specifies when to interrupt a script that is being evaluated.
        /// A null value implies that scripts should never be interrupted, but instead run indefinitely.
        /// </summary>
        TimeSpan Script { get; set; }
    }
}
