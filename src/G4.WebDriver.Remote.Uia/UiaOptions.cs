using G4.WebDriver.Attributes;

using System.Collections.Generic;

namespace G4.WebDriver.Remote.Uia
{
    /// <summary>
    /// Provides options specific to the UI Automation (UIA) driver, used for configuring UIA sessions.
    /// </summary>
    [WebDriverOptions(prefix: "uia", optionsKey: "options")]
    public class UiaOptions : WebDriverOptionsBase
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="UiaOptions"/> class with default values.
        /// </summary>
        public UiaOptions()
        {
            UiaOptionsDictionary = [];
        }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Gets or sets the additional UIA options.
        /// </summary>
        public Dictionary<string, object> UiaOptionsDictionary { get; }

        /// <summary>
        /// Gets or sets the application to be automated.
        /// </summary>
        [VendorCapability(capabilityName: "app")]
        public string App
        {
            get => GetOption<string>("app");
            set => SetOption("app", value);
        }

        /// <summary>
        /// Gets or sets the arguments for the application.
        /// </summary>
        [VendorCapability(capabilityName: "arguments")]
        public string[] Arguments
        {
            get => GetOption<string[]>("arguments");
            set => SetOption("arguments", value);
        }

        /// <summary>
        /// Gets or sets the impersonation options for the UIA driver.
        /// Setting this will start the application process as the impersonated user.
        /// </summary>
        [VendorCapability(capabilityName: "impersonation")]
        public ImpersonationOptions Impersonation
        {
            get => GetOption<ImpersonationOptions>("impersonation");
            set => SetOption("impersonation", value);
        }

        /// <summary>
        /// Gets or sets the label for the UIA driver.
        /// This helps to target a driver on a specific machine when using a grid.
        /// </summary>
        [VendorCapability(capabilityName: "label")]
        public string Label
        {
            get => GetOption<string>("label");
            set => SetOption("label", value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to scope into an existing open application.
        /// </summary>
        [VendorCapability(capabilityName: "label")]
        public bool Mount
        {
            get => GetOption<bool>("mount");
            set => SetOption("mount", value);
        }

        /// <summary>
        /// Gets or sets the working directory for the application.
        /// </summary>
        [VendorCapability(capabilityName: "workingDirectory")]
        public string WorkingDirectory
        {
            get => GetOption<string>("workingDirectory");
            set => SetOption("workingDirectory", value);
        }
        #endregion

        #region *** Methods      ***
        // Gets the value of the specified option from the UIA options dictionary.
        private T GetOption<T>(string key)
        {
            // Check if the UIA options dictionary contains the specified key.
            return UiaOptionsDictionary.TryGetValue(key, out object value)
                ? (T)value
                : default;
        }

        // Sets the value of the specified option in the UIA options dictionary.
        private void SetOption<T>(string key, T value)
        {
            // Set the value of the specified key in the UIA options dictionary.
            UiaOptionsDictionary[key] = value;
        }
        #endregion

        #region *** Nested Types ***
        /// <summary>
        /// Represents the impersonation options for UI Automation (UIA) driver.
        /// </summary>
        public class ImpersonationOptions
        {
            /// <summary>
            /// Gets or sets the domain for impersonation.
            /// </summary>
            public string Domain { get; set; }

            /// <summary>
            /// Gets or sets the username for impersonation.
            /// </summary>
            public string Username { get; set; }

            /// <summary>
            /// Gets or sets the password for impersonation.
            /// </summary>
            public string Password { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether impersonation is enabled.
            /// </summary>
            public bool Enabled { get; set; }
        }
        #endregion
    }
}
