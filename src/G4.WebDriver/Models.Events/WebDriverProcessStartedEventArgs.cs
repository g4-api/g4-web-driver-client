/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/DriverProcessStartedEventArgs.cs
 */
using System;
using System.Diagnostics;

namespace G4.WebDriver.Models.Events
{
    /// <summary>
    /// Fires right after the WebDriver had been started.
    /// </summary>
    /// <param name="process">The WebDriverServer <see cref="System.Diagnostics.Process"/> object.</param>
    public class WebDriverProcessStartedEventArgs(Process process) : EventArgs
    {
        /// <summary>
        /// Gets the WebDriverServer <see cref="System.Diagnostics.Process"/> object.
        /// </summary>
        public Process Process { get; } = process;
    }
}
