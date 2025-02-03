using G4.WebDriver.Exceptions;
using G4.WebDriver.Extensions;
using G4.WebDriver.Models;
using G4.WebDriver.Remote;

using System;
using System.Text.Json;

namespace G4.WebDriver.Simulator
{
    /// <summary>
    /// Defines the interface through which the user can locate a given frame or window.
    /// </summary>
    /// <param name="driver">Driver by which this TargetLocator created.</param>
    public class SimulatorTargetLocator(IWebDriver driver) : ITargetLocator, IDriverReference
    {
        #region *** Fields     ***
        // Represents the window handle of the driver.
        private readonly string _windowHandle = driver.WindowHandle;
        #endregion

        #region *** Properties ***
        /// <inheritdoc />
        public IWebDriver Driver { get; } = driver;
        #endregion

        #region *** Methods    ***
        /// <inheritdoc />
        public IWebElement ActiveElement() => Driver.FindElement(By.Custom.Positive());

        /// <inheritdoc />
        public IUserPrompt Alert()
        {
            // Cast the driver to a SimulatorDriver.
            var onDriver = (SimulatorDriver)Driver;

            // Key to check if the simulator has an alert.
            const string key = SimulatorCapabilities.HasAlert;

            // Initialize a flag to determine if an alert is present.
            var hasAlert = false;

            // Check if the capabilities dictionary contains the alert key.
            var hasKey = onDriver.Capabilities.AlwaysMatch.ContainsKey(key);

            if (hasKey)
            {
                // Retrieve the value associated with the alert key.
                var value = onDriver.Capabilities.AlwaysMatch[key];

                // Check if the value is a JSON element and get the boolean value.
                hasAlert = value is JsonElement element
                    ? element.GetBoolean()
                    : (bool)value;
            }

            // If an alert is present, return a SimulatorAlert instance.
            // Otherwise, throw a NoSuchAlertException.
            return hasAlert
                ? new SimulatorAlert((SimulatorDriver)Driver)
                : throw new NoSuchAlertException("No alert dialog is present.");
        }

        /// <inheritdoc />
        public IWebDriver DefaultContent()
        {
            return Driver;
        }

        /// <inheritdoc />
        public IWebDriver Frame(IWebElement element)
        {
            // Check if the provided element is null
            if (element == null)
            {
                // Throw a NoSuchFrameException with a mock message
                throw new NoSuchFrameException("The requested frame is not available.");
            }

            // Cast the Driver to a SimulatorDriver
            var simulatorDriver = (SimulatorDriver)Driver;

            // Set the window handle in the simulator driver
            simulatorDriver.WindowHandle = element.Id;

            // Return the simulator driver for method chaining
            return simulatorDriver;
        }

        /// <inheritdoc />
        public IWebDriver Frame(int id)
        {
            // Check if the handle is `-1` "NoSuchWindow"
            if (id == 1)
            {
                // Throw a NoSuchFrameException with a mock message
                throw new NoSuchFrameException("The requested frame is not available.");
            }

            // Cast the Driver to a SimulatorDriver
            var simulatorDriver = (SimulatorDriver)Driver;

            // Set the window handle in the simulator driver
            simulatorDriver.WindowHandle = $"{id}";

            // Return the simulator driver for method chaining
            return simulatorDriver;
        }

        /// <inheritdoc />
        public IWebDriver NewWindow(string type)
        {
            // Cast the driver to SimulatorDriver (assuming SimulatorDriver is the actual driver type)
            var onDriver = (SimulatorDriver)Driver;

            // Add the new window handle to the list
            onDriver.NewWindowHandle();

            // Return the driver instance for the new window
            return onDriver;
        }

        /// <inheritdoc />
        public IWebDriver ParentFrame()
        {
            // Cast the Driver to a SimulatorDriver
            var simulatorDriver = (SimulatorDriver)Driver;

            // Set the window handle in the simulator driver
            simulatorDriver.WindowHandle = _windowHandle;

            // Return the simulator driver for method chaining
            return simulatorDriver;
        }

        /// <inheritdoc />
        public IWebDriver Window(string handle)
        {
            // Check if the handle contains "NoSuchWindow" (case-insensitive)
            if (handle.Contains("NoSuchWindow", StringComparison.OrdinalIgnoreCase))
            {
                // Throw a NoSuchWindowException with a mock message
                throw new NoSuchWindowException("The requested window is not available.");
            }

            // Cast the Driver to a SimulatorDriver
            var simulatorDriver = (SimulatorDriver)Driver;

            // Set the window handle in the simulator driver
            simulatorDriver.WindowHandle = handle;

            // Return the simulator driver for method chaining
            return simulatorDriver;
        }
        #endregion
    }
}
