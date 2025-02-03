using G4.WebDriver.Extensions;

using System;
using System.Collections.Generic;
using System.Text.Json;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Manages timeouts for various WebDriver operations such as implicit waits, page load, and script execution.
    /// Implements the <see cref="ITimeouts"/> interface.
    /// </summary>
    public class WebDriverTimeouts(IWebDriver driver) : ITimeouts
    {
        #region *** Fields     ***
        // Options for JSON serialization/deserialization, with case-insensitive property names.
        private static readonly JsonSerializerOptions s_jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        // Command invoker used to send WebDriver commands.
        private readonly IWebDriverCommandInvoker _invoker = driver.Invoker;
        #endregion

        #region *** Properties ***
        /// <summary>
        /// Gets or sets the timeout for implicit waits. This is the time the WebDriver will
        /// wait when searching for an element if it is not immediately present.
        /// </summary>
        public TimeSpan Implicit
        {
            // Set the implicit wait timeout.
            set => SetTimeouts(_invoker, "implicit", value);

            // Get the current implicit wait timeout.
            get => GetTimeouts(_invoker, "implicit");
        }

        /// <summary>
        /// Gets or sets the timeout for page load. This is the time the WebDriver will wait
        /// for a page to load completely before throwing an exception.
        /// </summary>
        public TimeSpan PageLoad
        {
            // Set the page load timeout.
            set => SetTimeouts(_invoker, "pageLoad", value);

            // Get the current page load timeout.
            get => GetTimeouts(_invoker, "pageLoad");
        }

        /// <summary>
        /// Gets or sets the timeout for asynchronous script execution. This is the time the WebDriver
        /// will wait for an async script to finish execution.
        /// </summary>
        public TimeSpan Script
        {
            // Set the script execution timeout.
            set => SetTimeouts(_invoker, "script", value);

            // Get the current script execution timeout.
            get => GetTimeouts(_invoker, "script");
        }
        #endregion

        #region *** Methods    ***
        /// <summary>
        /// Retrieves the current timeout value for the specified type (implicit, pageLoad, or script).
        /// </summary>
        /// <param name="invoker">The WebDriver command invoker.</param>
        /// <param name="type">The type of timeout to retrieve (e.g., "implicit", "pageLoad", "script").</param>
        /// <returns>The current timeout value as a <see cref="TimeSpan"/>.</returns>
        private static TimeSpan GetTimeouts(IWebDriverCommandInvoker invoker, string type)
        {
            // Send command to get timeouts.
            var response = invoker
                .Invoke(invoker.NewCommand(nameof(WebDriverCommands.GetTimeouts)))
                .Value
                .ToString();

            // Deserialize the JSON response.
            var json = JsonSerializer.Deserialize<IDictionary<string, double>>(response, s_jsonOptions);

            // Try to get the timeout value for the specified type.
            _ = json.TryGetValue(type, out double timeout);

            // Convert the timeout value from milliseconds to TimeSpan.
            return TimeSpan.FromMilliseconds(timeout);
        }

        /// <summary>
        /// Sets the timeout value for the specified type (implicit, pageLoad, or script).
        /// </summary>
        /// <param name="invoker">The WebDriver command invoker.</param>
        /// <param name="type">The type of timeout to set (e.g., "implicit", "pageLoad", "script").</param>
        /// <param name="timeout">The timeout value to set, as a <see cref="TimeSpan"/>.</param>
        private static void SetTimeouts(IWebDriverCommandInvoker invoker, string type, TimeSpan timeout)
        {
            // Convert the TimeSpan to milliseconds for the command data.
            var data = new Dictionary<string, double>
            {
                [type] = timeout.TotalMilliseconds
            };

            // Create a new command to set the timeout.
            var command = invoker
                .NewCommand(nameof(WebDriverCommands.SetTimeouts))
                .SetData(data);

            // Send the command to set the timeout.
            invoker.Invoke(command);
        }
        #endregion
    }
}
