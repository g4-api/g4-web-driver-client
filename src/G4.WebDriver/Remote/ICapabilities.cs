/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/ICapabilities.cs
 */
using G4.WebDriver.Models;

using System.Collections.Generic;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Defines the interface through which the user can determine the capabilities of a driver.
    /// </summary>
    public interface ICapabilities
    {
        #region *** Properties ***
        /// <summary>
        /// Gets a collection of known capabilities supported by the driver.
        /// </summary>
        IEnumerable<string> KnownCapabilities { get; }
        #endregion

        #region *** Methods    ***
        /// <summary>
        /// Asserts whether a specific capability is known to the driver.
        /// </summary>
        /// <param name="capabilityName">The name of the capability to check.</param>
        /// <returns><c>true</c> if the capability is known; otherwise, <c>false</c>.</returns>
        bool AssertKnownCapability(string capabilityName);

        /// <summary>
        /// Converts the capabilities into a <see cref="CapabilitiesModel"/> object.
        /// </summary>
        /// <returns>A <see cref="CapabilitiesModel"/> object that represents the driver's capabilities.</returns>
        CapabilitiesModel ConvertToCapabilities();
        #endregion
    }
}
