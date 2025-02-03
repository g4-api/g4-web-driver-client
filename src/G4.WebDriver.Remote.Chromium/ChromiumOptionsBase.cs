/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/Chromium/ChromiumOptions.cs
 * https://learn.microsoft.com/en-us/microsoft-edge/webdriver-chromium/capabilities-edge-options
 */
using G4.WebDriver.Attributes;
using G4.WebDriver.Models;
using G4.WebDriver.Remote.Chromium.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace G4.WebDriver.Remote.Chromium
{
    /// <summary>
    /// Represents the options specific to the Chromium browser.
    /// This class serves as a base for defining and configuring various Chromium-specific options that can be used when creating a Chromium WebDriver instance.
    /// Use derived classes of ChromiumOptions to set specific options for Chromium browser behavior, such as command-line arguments, binary location, debugging options, and more.
    /// </summary>
    public abstract class ChromiumOptionsBase : WebDriverOptionsBase
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromiumOptionsBase"/> class.
        /// </summary>
        protected ChromiumOptionsBase()
        { }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets or sets a list of command-line arguments to pass to the Chromium process on launch.
        /// Arguments with an associated value should be separated by an = sign (for example, ['start-maximized', 'user-data-dir=/tmp/temp_profile']).
        /// If you're launching a WebView2 app, then these arguments are passed to your app instead of the underlying Chromium browser process.
        /// To pass arguments to the browser process when launching a WebView2 app, use webviewOptions.additionalBrowserArguments instead.
        /// </summary>
        [VendorCapability(capabilityName: "args")]
        public IEnumerable<string> Arguments { get; set; }

        /// <summary>
        /// Gets or sets the path to the Chromium binary to use (on macOS, the path should be the actual binary,
        /// not just the app. for example, '/Applications/Chromium.app/Contents/MacOS/Chromium').
        /// </summary>
        [VendorCapability(capabilityName: "binary")]
        public string BinaryLocation { get; set; }

        /// <summary>
        /// Gets or sets the address of a debugger server to which to connect, in the form of hostname/ip:port,
        /// for example '127.0.0.1:38947'.
        /// </summary>
        [VendorCapability(capabilityName: "debuggerAddress")]
        public string DebuggerAddress { get; set; }

        /// <summary>
        /// Default value = false.
        /// If false, Chromium quits when the WebDriver service shuts down, even if the WebDriver local end hasn't closed the session.
        /// If true, Chromium only quits if the WebDriver local end closes the session.
        /// If true, and the WebDriver local end doesn't close the session, EdgeDriver doesn't clean up the temporary user data folder used by the Chromium instance.
        /// </summary>
        [VendorCapability(capabilityName: "detach")]
        public bool KeepBrowserRunning { get; set; }

        /// <summary>
        /// Gets or sets a list of Chromium command line switches to exclude that EdgeDriver by default passes when starting Chromium.
        /// Avoid the -- prefix for switches.
        /// </summary>
        [VendorCapability(capabilityName: "excludeSwitches")]
        public IEnumerable<string> ExcludeSwitches { get; set; }

        /// <summary>
        /// Gets a list of extensions to install on startup.
        /// Each item in the list should be a base-64 encoded packed extension (.crx).
        /// </summary>
        [VendorCapability(capabilityName: "extensions")]
        public IEnumerable<(string Base64, string FilePath)> Extensions { get; private set; }

        /// <summary>
        /// Gets or sets a dictionary with each entry consisting of the name of the preference and the value.
        /// The preferences are applied to the Local State file in the user data folder.
        /// </summary>
        [VendorCapability(capabilityName: "localState")]
        public IDictionary<string, object> LocalState { get; set; }

        /// <summary>
        /// Gets or sets the directory to store Chromium minidumps. (Supported only on Linux.)
        /// </summary>
        [VendorCapability(capabilityName: "minidumpPath")]
        public string MinidumpPath { get; set; }

        /// <summary>
        /// gets or sets a dictionary with either a value for deviceName, or values for deviceMetrics and userAgent.
        /// </summary>
        [VendorCapability(capabilityName: "mobileEmulation")]
        public IDictionary<string, object> MobileEmulation { get; set; }

        /// <summary>
        /// Gets or sets an optional dictionary that specifies performance logging preferences.
        /// For more information, see <see cref="ChromiumPerformanceLoggingPreferencesModel"/> object.
        /// </summary>
        [VendorCapability(capabilityName: "perfLoggingPrefs")]
        public ChromiumPerformanceLoggingPreferencesModel PerformanceLoggingPreferences { get; set; }

        /// <summary>
        /// Gets or sets the a dictionary with each entry consisting of the name of the preference and the value.
        /// The preferences are only applied to the user profile in use.
        /// For examples, see the Preferences file in the user data folder of Edge or Chrome.
        /// </summary>
        [VendorCapability(capabilityName: "prefs")]
        public IDictionary<string, object> ProfilePreferences { get; set; }

        /// <summary>
        /// Gets or sets a list of window types that are displayed in the list of window handles.
        /// For access to Android webview elements, include webview in the list.
        /// </summary>
        [VendorCapability(capabilityName: "windowTypes")]
        public IEnumerable<string> WindowTypes { get; set; }
        #endregion

        #region *** Methods      ***
        /// <summary>
        /// Adds extensions to the Chromium options by specifying the file paths to the extension files.
        /// </summary>
        /// <param name="filesPath">An array of file paths to the extension files.</param>
        /// <returns>The updated <see cref="ChromiumOptionsBase"/> instance with the added extensions.</returns>
        public ChromiumOptionsBase AddExtensions(params string[] filesPath)
        {
            // Initialize the file paths to an empty array if they are null.
            filesPath ??= [];

            // Initialize a list to store the extensions in (Base64, FilePath) format.
            var extensions = new List<(string Base64, string FilePath)>();

            // Iterate over each provided file path.
            foreach (string path in filesPath)
            {
                // Skip to the next file path if the file does not exist.
                if (!File.Exists(path))
                {
                    continue;
                }

                // Read the file's bytes and convert them to a Base64 string.
                var bytes = File.ReadAllBytes(path);
                var base64 = Convert.ToBase64String(bytes);

                // Add the Base64 string and file path to the list of extensions.
                extensions.Add((base64, path));
            }

            // Initialize the Extensions list if it is null and add the new extensions to it.
            Extensions ??= new List<(string, string)>();
            Extensions = Extensions.Concat(extensions).ToList();

            // Return the updated instance to allow for fluent chaining.
            return this;
        }

        /// <inheritdoc />
        protected override CapabilitiesModel OnConvertToCapabilities(CapabilitiesModel capabilities)
        {
            // Retrieve the key used for vendor-specific options.
            var optionsKey = VendorOptionsKey;

            // Check if the capabilities contain the vendor-specific options.
            var isOptions = capabilities.AlwaysMatch.TryGetValue(optionsKey, out object options);

            // If no options are found or the options object is not a dictionary, return the capabilities as is.
            if (!isOptions || options is not IDictionary<string, object>)
            {
                return capabilities;
            }

            // If there are no extensions to add, return the capabilities as is.
            if (Extensions?.Any() != true)
            {
                return capabilities;
            }

            // Add the extensions to the vendor-specific options in the capabilities model.
            ((IDictionary<string, object>)options)["extensions"] = Extensions.Select(i => i.Base64);

            // Return the updated capabilities model.
            return capabilities;
        }

        /// <inheritdoc />
        protected override void OnSetLoggingPreferences(
            string vendorPrefix,
            IDictionary<string, object> capabilitiesDictionary,
            IDictionary<string, object> optionsDictionary,
            params LogPreferencesModel[] types)
        {
            // Construct the logging preferences key using the vendor prefix.
            var loggingKey = $"{vendorPrefix}:loggingPrefs";

            // Create a dictionary to store the logging preferences.
            var logObject = new Dictionary<string, object>();

            // Iterate over the provided log types and add them to the capabilities dictionary.
            foreach (var logType in types)
            {
                // Set each log type's level in the capabilities dictionary.
                logObject[logType.LogType] = logType.LogLevel;
            }

            // Store the logging preferences in the capabilitiesDictionary using the constructed logging key.
            capabilitiesDictionary[loggingKey] = logObject;
        }
        #endregion

        #region *** Nested Types ***
        /// <summary>
        /// Defines constants for various log levels used in WebDriver logging.
        /// </summary>
        public static class ChromiumLogLevels
        {
            /// <summary>
            /// Represents all log levels. Typically used to enable logging of all events.
            /// </summary>
            public const string All = "ALL";

            /// <summary>
            /// Represents the 'config' log level, which is used for logging configuration messages.
            /// </summary>
            public const string Config = "CONFIG";

            /// <summary>
            /// Represents the 'fine' log level, used for general detailed logging.
            /// </summary>
            public const string Fine = "FINE";

            /// <summary>
            /// Represents the 'finer' log level, which provides detailed logging, but less verbose than 'finest'.
            /// </summary>
            public const string Finer = "FINER";

            /// <summary>
            /// Represents the 'finest' log level, which is the most detailed level of logging.
            /// </summary>
            public const string Finest = "FINEST";

            /// <summary>
            /// Represents the 'info' log level, typically used for informational messages.
            /// </summary>
            public const string Info = "INFO";

            /// <summary>
            /// Represents the 'off' log level, which disables logging.
            /// </summary>
            public const string Off = "OFF";

            /// <summary>
            /// Represents the 'severe' log level, typically used for error messages that indicate a serious failure.
            /// </summary>
            public const string Severe = "SEVERE";

            /// <summary>
            /// Represents the 'warning' log level, typically used for warning messages that do not prevent the application from running.
            /// </summary>
            public const string Warning = "WARNING";
        }
        #endregion
    }
}
