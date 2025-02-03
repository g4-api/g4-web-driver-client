using G4.WebDriver.DeveloperTools.Models;
using G4.WebDriver.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace G4.WebDriver.Remote.Chromium.DeveloperTools
{
    /// <summary>
    /// A domain for interacting with Cast, Presentation API, and Remote Playback API functionalities
    /// through Chrome DevTools Protocol (CDP).
    /// </summary>
    /// <param name="driver">The ChromiumDriver instance.</param>
    public class CastDomain(ChromiumDriverBase driver)
    {
        #region *** Fields  ***
        // Options for JSON serialization and deserialization, using camel
        // case naming and case insensitivity for property names.
        private readonly static JsonSerializerOptions s_options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        // The command invoker for executing WebDriver commands.
        private readonly IWebDriverCommandInvoker _invoker = driver.Invoker;
        #endregion

        #region *** Methods ***
        /// <summary>
        /// Disables the Cast domain in the Chromium browser, stopping any ongoing casting operations.
        /// </summary>
        public void Disable()
        {
            // Invoke the "Cast.disable" command using the Chrome DevTools Protocol (CDP).
            driver.SendDeveloperToolsCommand(new CdpRequestModel
            {
                Command = "Cast.disable",
                Parameters = new Dictionary<string, object>()
            });
        }

        /// <summary>
        /// Enables the Cast domain in the Chromium browser, allowing casting operations to start.
        /// </summary>
        public void Enable()
        {
            // Invoke the "Cast.enable" command using the Chrome DevTools Protocol (CDP).
            driver.SendDeveloperToolsCommand(new CdpRequestModel
            {
                Command = "Cast.enable",
                Parameters = new Dictionary<string, object>()
            });
        }

        /// <summary>
        /// Returns the error message if there is any issue in a Cast session.
        /// </summary>
        /// <returns>An error message.</returns>
        public string GetIssueMessage()
        {
            // Invoke the custom command 'GetCastIssueMessageCommand'
            var response = _invoker.Invoke("GetIssueMessage");

            // Extract the JSON response value
            var value = (JsonElement)response.Value;

            // Get
            return value.ToString();
        }

        /// <summary>
        /// Retrieves a list of available sinks (e.g., Chromecast devices) for casting with a default timeout of 15 seconds.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerable{SinkModel}"/> containing the list of available sinks. If no sinks are found, returns an empty collection.
        /// </returns>
        public IEnumerable<SinkModel> GetSinks() => GetSinks(timeout: 15000);

        /// <summary>
        /// Retrieves a list of available sinks (e.g., Chromecast devices) for casting within a specified timeout period.
        /// </summary>
        /// <param name="timeout">A <see cref="TimeSpan"/> representing the maximum time to wait for sinks to be discovered.</param>
        /// <returns>
        /// An <see cref="IEnumerable{SinkModel}"/> containing the list of available sinks. If no sinks are found, returns an empty collection.
        /// </returns>
        public IEnumerable<SinkModel> GetSinks(TimeSpan timeout) => GetSinks(timeout.TotalMilliseconds);

        /// <summary>
        /// Retrieves a list of available sinks (e.g., Chromecast devices) for casting within a specified timeout period.
        /// </summary>
        /// <param name="timeout">The maximum time, in milliseconds, to wait for sinks to be discovered.</param>
        /// <returns>An <see cref="IEnumerable{SinkModel}"/> containing the list of available sinks. If no sinks are found, returns an empty collection.</returns>
        public IEnumerable<SinkModel> GetSinks(double timeout)
        {
            // Calculate the expiration time based on the provided timeout.
            var expiration = DateTime.UtcNow.AddMilliseconds(timeout);

            // Initialize the collection of sinks as empty.
            var sinks = Enumerable.Empty<SinkModel>();

            // Continue to poll for available sinks until the timeout is reached.
            while (expiration > DateTime.UtcNow)
            {
                // Invoke the command to get available sinks using the WebDriver command invoker.
                var response = _invoker.Invoke("GetSinks");

                // Deserialize the response value into a collection of SinkModel objects.
                var value = (JsonElement)response.Value;
                sinks = value.Deserialize<IEnumerable<SinkModel>>(s_options);

                // If any sinks are found, exit the loop early.
                if (sinks.Any())
                {
                    break;
                }

                // Wait for 100 milliseconds before trying again.
                Thread.Sleep(millisecondsTimeout: 100);
            }

            // Return the collection of discovered sinks (or empty if none found).
            return sinks;
        }

        /// <summary>
        /// Sets the specified sink device to be used for casting operations.
        /// </summary>
        /// <param name="deviceName">The name of the sink device to use for casting.</param>
        /// <exception cref="ArgumentException">Thrown when the provided <paramref name="deviceName"/> is <c>null</c>.</exception>
        public void SetSinkToUse(string deviceName)
        {
            // Check if the deviceName is null and throw an exception if it is.
            if (deviceName == null)
            {
                const string error400 = "The device name cannot be null.";
                throw new ArgumentException(error400, nameof(deviceName));
            }

            // Invoke the "SetSinkToUse" command with the specified sink name using the WebDriver command invoker.
            _invoker.Invoke("SetSinkToUse", new Dictionary<string, object>
            {
                ["sinkName"] = deviceName // Set the sinkName parameter to the provided deviceName.
            });
        }

        /// <summary>
        /// Starts tab mirroring for the desktop on the specified device using Chrome DevTools Protocol (CDP).
        /// </summary>
        /// <param name="deviceName">The name of the target sink (device).</param>
        public void StartDesktopMirroring(string deviceName)
        {
            StartMirroring(
                _invoker,
                command: "StartDesktopMirroring",
                deviceName,
                timeout: 15000);
        }

        /// <summary>
        /// Starts tab mirroring for the desktop on the specified device using Chrome DevTools Protocol (CDP).
        /// </summary>
        /// <param name="deviceName">The name of the target sink (device).</param>
        /// <param name="timeout">The maximum time to wait for the operation to complete.</param>
        public void StartDesktopMirroring(string deviceName, TimeSpan timeout)
        {
            StartMirroring(
                _invoker,
                command: "StartDesktopMirroring",
                deviceName,
                timeout.TotalMilliseconds);
        }

        /// <summary>
        /// Starts tab mirroring for the current browser tab on the specified device using Chrome DevTools Protocol (CDP).
        /// </summary>
        /// <param name="deviceName">The name of the target sink (device).</param>
        public void StartTabMirroring(string deviceName)
        {
            StartMirroring(
                _invoker,
                command: "StartTabMirroring",
                deviceName,
                timeout: 15000);
        }

        /// <summary>
        /// Starts tab mirroring for the current browser tab on the specified device using Chrome DevTools Protocol (CDP).
        /// </summary>
        /// <param name="deviceName">The name of the target sink (device).</param>
        /// <param name="timeout">The maximum time to wait for the operation to complete.</param>
        public void StartTabMirroring(string deviceName, TimeSpan timeout)
        {
            StartMirroring(
                _invoker,
                command: "StartTabMirroring",
                deviceName,
                timeout.TotalMilliseconds);
        }

        /// <summary>
        /// Stops the casting operation on the specified device.
        /// </summary>
        /// <param name="deviceName">The name of the device (sink) on which casting is to be stopped.</param>
        /// <exception cref="ArgumentException">Thrown when the provided <paramref name="deviceName"/> is <c>null</c>.</exception>
        public void StopCasting(string deviceName)
        {
            // Check if the deviceName is null and throw an exception if it is.
            if (deviceName == null)
            {
                const string error400 = "Device name cannot be null for stopping casting.";
                throw new ArgumentException(error400, nameof(deviceName));
            }

            // Invoke the "StopCasting" command with the specified sink name using the WebDriver command invoker.
            _invoker.Invoke("StopCasting", new Dictionary<string, object>
            {
                ["sinkName"] = deviceName // Set the sinkName parameter to the provided deviceName.
            });
        }

        // Starts the screen mirroring to a specified device using the given WebDriver command.
        private static void StartMirroring(
            IWebDriverCommandInvoker invoker,
            string command,
            string deviceName,
            double timeout)
        {
            // Check if the deviceName is null and throw an exception if it is.
            if (deviceName == null)
            {
                const string error400 = "Device name cannot be null for mirroring operation.";
                throw new ArgumentException(error400, nameof(deviceName));
            }

            // Calculate the expiration time based on the provided timeout.
            var end = DateTime.UtcNow.AddMilliseconds(timeout);
            Exception exception = null; // Initialize the variable to store any caught exception.

            // Continue attempting to start mirroring until the timeout is reached.
            while (end > DateTime.UtcNow)
            {
                try
                {
                    // Invoke the mirroring command with the specified device name.
                    var response = invoker.Invoke(command, new Dictionary<string, object>
                    {
                        ["sinkName"] = deviceName // Set the sinkName parameter to the provided deviceName.
                    });

                    // Check if the command was successful; if not, retry after a short delay.
                    if (!response.WebDriverResponse.IsSuccessStatusCode)
                    {
                        Thread.Sleep(millisecondsTimeout: 100); // Wait for 100 milliseconds before retrying.
                        continue;
                    }
                    return; // Exit the method if the command was successful.
                }
                catch (Exception e)
                {
                    // Capture the exception and continue retrying until the timeout is reached.
                    exception = e;
                    Thread.Sleep(millisecondsTimeout: 100); // Wait for 100 milliseconds before retrying.
                }
            }

            // If an exception was captured during the attempts, throw the base exception.
            if (exception != null)
            {
                throw exception.GetBaseException();
            }
        }
        #endregion
    }
}
