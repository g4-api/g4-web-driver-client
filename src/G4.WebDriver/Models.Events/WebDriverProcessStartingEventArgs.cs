/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/DriverProcessStartingEventArgs.cs
 */
using System;
using System.Diagnostics;

namespace G4.WebDriver.Models.Events
{
    /// <summary>
    /// Fires right before the WebDriver is starting.
    /// </summary>
    /// <param name="startInfo">The <see cref="ProcessStartInfo"/> object with which the driver service process will be started.</param>
    public class WebDriverProcessStartingEventArgs(ProcessStartInfo startInfo) : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="ProcessStartInfo"/> object with which the driver service process will be started.
        /// </summary>
        public ProcessStartInfo StartInfo { get; set; } = startInfo;
    }
}
