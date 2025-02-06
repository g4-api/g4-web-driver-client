using G4.WebDriver.Attributes;
using G4.WebDriver.Models;

using System.Collections.Generic;

namespace G4.WebDriver.Remote.Firefox
{
    /// <summary>
    /// Represents the options specific to Firefox for configuring WebDriver sessions.
    /// Inherits from <see cref="WebDriverOptionsBase"/> to provide a base set of WebDriver options.
    /// </summary>
    [WebDriverOptions(prefix: "moz", optionsKey: "firefoxOptions")]
    public class FirefoxOptions : WebDriverOptionsBase
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="FirefoxOptions"/> class.
        /// Sets the browser name to "firefox" and adds it to the capabilities.
        /// </summary>
        public FirefoxOptions()
        {
            // Set the browser name to Firefox.
            BrowserName = "firefox";

            // Add the browser name to the capabilities.
            AddCapabilities("browserName", BrowserName);
        }
        #endregion

        #region *** Methods      ***
        /// <inheritdoc />
        protected override void OnSetLoggingPreferences(
            string vendorPrefix,
            IDictionary<string, object> capabilitiesDictionary,
            IDictionary<string, object> optionsDictionary,
            params LogPreferencesModel[] types)
        {
            // Define the key for storing logging preferences in the options dictionary.
            var loggingKey = "log";

            // Create a dictionary to hold the logging configuration.
            var logObject = new Dictionary<string, object>();

            // Determine the logging level. Default to "warn" if no types are provided or if the log level is not set.
            var level = types == null || types.Length == 0
                ? "warn"
                : types[0].LogLevel;

            // If the determined logging level is null or empty, default to "warn".
            if (string.IsNullOrEmpty(level))
            {
                level = "warn";
            }

            // Add the logging level to the logObject dictionary.
            logObject["level"] = level;

            // Store the logObject in the options dictionary using the defined logging key.
            optionsDictionary[loggingKey] = logObject;
        }
        #endregion

        #region *** Nested Types ***
        /// <summary>
        /// Defines constants for various log levels used by GeckoDriver for Firefox.
        /// These log levels control the verbosity and detail of the logging output.
        /// </summary>
        public static class FirefoxLogLevels
        {
            /// <summary>
            /// Logs detailed debug information, including WebDriver commands and responses.
            /// Useful for in-depth troubleshooting and development.
            /// </summary>
            public const string Debug = "debug";

            /// <summary>
            /// Logs information about the configuration of GeckoDriver, including settings and capabilities.
            /// Helps in understanding how GeckoDriver is set up and configured.
            /// </summary>
            public const string Config = "config";

            /// <summary>
            /// Logs only error messages that indicate failures or issues that occurred.
            /// Useful for identifying errors without cluttering the logs with informational messages.
            /// </summary>
            public const string Error = "error";

            /// <summary>
            /// Logs only fatal errors that lead to GeckoDriver termination.
            /// This level is useful for catching critical issues that cause crashes.
            /// </summary>
            public const string Fatal = "fatal";

            /// <summary>
            /// Logs general information about GeckoDriver operations.
            /// Suitable for understanding the general flow without too much detail.
            /// </summary>
            public const string Info = "info";

            /// <summary>
            /// Logs all events, including all HTTP requests and responses, WebDriver commands, and more.
            /// This level provides the most comprehensive logging detail.
            /// </summary>
            public const string Trace = "trace";

            /// <summary>
            /// Logs only warning messages that indicate potential issues.
            /// Useful for catching warnings that could become problems without full error logging.
            /// </summary>
            public const string Warn = "warn";
        }
        #endregion
    }
}
