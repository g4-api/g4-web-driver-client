using G4.WebDriver.Attributes;
using G4.WebDriver.Extensions;
using G4.WebDriver.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Represents the base class for WebDriver options that includes capabilities and options management.
    /// Implements the <see cref="ICapabilities"/> interface to provide the necessary browser capabilities.
    /// </summary>
    public abstract class WebDriverOptionsBase : ICapabilities
    {
        #region *** Fields       ***
        // Dictionary to store capabilities for the WebDriver.
        private readonly Dictionary<string, object> _capabilities;

        // Dictionary to store specific options for the WebDriver.
        private readonly Dictionary<string, object> _options;
        #endregion

        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="WebDriverOptionsBase"/> class.
        /// Ensures that the capabilities and options dictionaries are initialized.
        /// </summary>
        protected WebDriverOptionsBase()
        {
            // Initialize the capabilities dictionary.
            _capabilities = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            // Initialize the options dictionary.
            _options = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Indicates whether untrusted and self-signed TLS certificates are
        /// implicitly trusted on navigation for the duration of the session.
        /// </summary>
        [KnownCapability(capabilityName: "acceptInsecureCerts")]
        public bool AcceptInsecureCerts { get; set; }

        /// <summary>
        /// Identifies the user agent.
        /// </summary>
        [KnownCapability(capabilityName: "browserName")]
        public string BrowserName { get; set; }

        /// <summary>
        /// Identifies the version of the user agent.
        /// </summary>
        [KnownCapability(capabilityName: "browserVersion")]
        public string BrowserVersion { get; set; }

        /// <summary>
        /// Defines the current session's page load strategy.
        /// </summary>
        [KnownCapability(capabilityName: "pageLoadStrategy")]
        public string PageLoadStrategy { get; set; }

        /// <summary>
        /// Identifies the operating system of the endpoint node.
        /// </summary>
        [KnownCapability(capabilityName: "platformName")]
        public string PlatformName { get; set; }

        /// <summary>
        /// Defines the current session's proxy configuration.
        /// </summary>
        [KnownCapability(capabilityName: "proxy")]
        public ProxyModel Proxy { get; set; }

        /// <summary>
        /// Describes the timeouts imposed on certain session operations.
        /// </summary>
        [KnownCapability(capabilityName: "sessionTimeouts")]
        public SessionTimeoutsModel SessionTimeouts { get; set; }

        /// <summary>
        /// Defines the current session's strict file intractability.
        /// </summary>
        [KnownCapability(capabilityName: "strictFileInteractability")]
        public bool UseStrictFileInteractability { get; set; }

        /// <summary>
        /// Describes the current session's user prompt handler.
        /// </summary>
        [KnownCapability(capabilityName: "unhandledPromptBehavior")]
        public string UnhandledPromptBehavior { get; set; }

        /// <summary>
        /// Indicates whether the remote end supports all of the commands in Resizing and Positioning Windows.
        /// </summary>
        [KnownCapability(capabilityName: "windowRect")]
        public string WindowRect { get; set; }

        /// <summary>
        /// Indicating whether to request URL of a WebSocket connection for bidirectional communication with a driver.
        /// </summary>
        [KnownCapability(capabilityName: "webSocketUrl")]
        public string WebSocketUrl { get; set; }

        /// <summary>
        /// A list of known capabilities that are supported by the WebDriver.
        /// </summary>
        public IEnumerable<string> KnownCapabilities => GetKnownCapabilities(this);

        /// <summary>
        /// Gets the vendor prefix to apply to vendor-specific capability names.
        /// </summary>
        public virtual string VendorOptionsKey => GetVendorOptionsKey(this);
        #endregion

        #region *** Methods      ***
        /// <summary>
        /// Adds a single capability to the WebDriver options.
        /// </summary>
        /// <param name="key">The key of the capability to add.</param>
        /// <param name="value">The value of the capability to add.</param>
        /// <returns>The updated <see cref="WebDriverOptionsBase"/> instance.</returns>
        public WebDriverOptionsBase AddCapabilities(string key, object value)
        {
            // Add the capability to the internal dictionary.
            _capabilities[key] = value;

            // Return the updated WebDriverOptions instance.
            return this;
        }

        /// <summary>
        /// Adds multiple capabilities to the WebDriver options.
        /// </summary>
        /// <param name="capabilities">A dictionary of capabilities to add.</param>
        /// <returns>The updated <see cref="WebDriverOptionsBase"/> instance.</returns>
        public WebDriverOptionsBase AddCapabilities(IDictionary<string, object> capabilities)
        {
            // If the capabilities dictionary is null or empty, return the current instance.
            if (capabilities?.Any() != true)
            {
                return this;
            }

            // Iterate through each key-value pair in the capabilities dictionary.
            foreach (var capability in capabilities)
            {
                // Add the capability to the internal dictionary.
                _capabilities[capability.Key] = capability.Value;
            }

            // Return the updated WebDriverOptions instance.
            return this;
        }

        /// <summary>
        /// Adds a single option to the WebDriver options.
        /// </summary>
        /// <param name="key">The key of the option to add.</param>
        /// <param name="value">The value of the option to add.</param>
        /// <returns>The updated <see cref="WebDriverOptionsBase"/> instance.</returns>
        public WebDriverOptionsBase AddOptions(string key, object value)
        {
            // Add the option to the internal dictionary.
            _options[key] = value;

            // Return the updated WebDriverOptions instance.
            return this;
        }

        /// <summary>
        /// Adds multiple options to the WebDriver options.
        /// </summary>
        /// <param name="options">A dictionary of options to add.</param>
        /// <returns>The updated <see cref="WebDriverOptionsBase"/> instance.</returns>
        public WebDriverOptionsBase AddOptions(IDictionary<string, object> options)
        {
            // If the options dictionary is null or empty, return the current instance.
            if (options?.Any() != true)
            {
                return this;
            }

            // Iterate through each key-value pair in the options dictionary.
            foreach (var option in options)
            {
                // Add the option to the internal dictionary.
                _options[option.Key] = option.Value;
            }

            // Return the updated WebDriverOptions instance.
            return this;
        }

        /// <summary>
        /// Converts the current WebDriver options into a <see cref="CapabilitiesModel"/> object.
        /// This method builds a new capabilities object based on the current settings and allows subclasses to extend or modify it.
        /// </summary>
        /// <returns>A <see cref="CapabilitiesModel"/> representing the capabilities configured in the current WebDriver options.</returns>
        public CapabilitiesModel ConvertToCapabilities()
        {
            // Create a new capabilities object by building it based on the current WebDriverOptions.
            var capabilities = NewCapabilitiesModel(this);

            // Invoke the OnConvertToCapabilities method to allow subclasses to modify or extend the capabilities.
            return OnConvertToCapabilities(capabilities);
        }

        /// <summary>
        /// Allows subclasses to modify or extend the capabilities when converting to a <see cref="CapabilitiesModel"/> object.
        /// This method can be overridden in derived classes to provide custom capabilities conversion logic.
        /// </summary>
        /// <param name="capabilities">The <see cref="CapabilitiesModel"/> object to be modified or extended.</param>
        /// <returns>The modified or extended <see cref="CapabilitiesModel"/> object.</returns>
        protected virtual CapabilitiesModel OnConvertToCapabilities(CapabilitiesModel capabilities)
        {
            // By default, return the provided capabilities without any modifications.
            return capabilities;
        }

        /// <summary>
        /// Deletes a capability from the WebDriver options by its key.
        /// </summary>
        /// <param name="key">The key of the capability to remove.</param>
        /// <returns>The updated <see cref="WebDriverOptionsBase"/> instance.</returns>
        public WebDriverOptionsBase DeleteCapability(string key)
        {
            // Remove the capability with the specified key from the internal dictionary.
            _capabilities.Remove(key);

            // Return the updated WebDriverOptions instance.
            return this;
        }

        /// <summary>
        /// Deletes an option from the WebDriver options by its key.
        /// </summary>
        /// <param name="key">The key of the option to remove.</param>
        /// <returns>The updated <see cref="WebDriverOptionsBase"/> instance.</returns>
        public WebDriverOptionsBase DeleteOption(string key)
        {
            // Remove the option with the specified key from the internal dictionary.
            _options.Remove(key);

            // Return the updated WebDriverOptions instance.
            return this;
        }

        /// <summary>
        /// Asserts whether a given capability name is a known capability.
        /// </summary>
        /// <param name="capabilityName">The name of the capability to check.</param>
        /// <returns>A boolean value indicating whether the specified capability name is in the list of known capabilities.</returns>
        public bool AssertKnownCapability(string capabilityName) => KnownCapabilities
            .Select(i => i.Trim().ToUpper())
            .Contains(capabilityName.ToUpper());

        /// <summary>
        /// Sets the logging preferences for the current instance by determining the vendor prefix and invoking the method
        /// to apply the logging preferences to the capabilities and options dictionaries.
        /// </summary>
        /// <param name="types">An array of <see cref="LogPreferencesModel"/> objects that specify the types of logs and their corresponding levels.</param>
        /// <exception cref="InvalidOperationException">Thrown when the vendor prefix is not defined for the current instance.</exception>
        public void SetLoggingPreferences(params LogPreferencesModel[] types)
        {
            // Retrieve the vendor prefix key associated with the current instance.
            var vendorPrefix = GetVendorPrefixKey(this);

            // If the vendor prefix is not defined or is empty, throw an exception.
            if (string.IsNullOrEmpty(vendorPrefix))
            {
                throw new InvalidOperationException("Vendor prefix is not defined.");
            }

            // Invoke the method to set logging preferences using the vendor prefix, capabilities, and options.
            OnSetLoggingPreferences(vendorPrefix, _capabilities, _options, types);
        }

        /// <summary>
        /// Sets the logging preferences for a specific vendor by adding the logging types and levels to the capabilities and options dictionaries.
        /// This method can be overridden in a derived class to provide specific handling for logging preferences.
        /// </summary>
        /// <param name="vendorPrefix">The prefix associated with the vendor for which the logging preferences are being set.</param>
        /// <param name="capabilitiesDictionary">The dictionary that stores various capabilities including logging preferences.</param>
        /// <param name="optionsDictionary">An additional dictionary that may store specific options related to the logging configuration.</param>
        /// <param name="types">An array of <see cref="LogPreferencesModel"/> objects that specify the types of logs and their corresponding levels.</param>
        protected virtual void OnSetLoggingPreferences(
            string vendorPrefix,
            IDictionary<string, object> capabilitiesDictionary,
            IDictionary<string, object> optionsDictionary,
            params LogPreferencesModel[] types)
        {
            // This method is intended to be overridden in derived classes to implement the logic
            // for setting logging preferences based on the provided parameters.

            // Use the vendorPrefix to construct a unique key for storing logging preferences.
            // Add logging types and levels to both capabilitiesDictionary and optionsDictionary
            // as per the requirements.
        }

        // Creates a new CapabilitiesModel object based on the specified WebDriver options.
        // This method extracts known and vendor-specific capabilities from the provided WebDriverOptionsBase instance.
        private static CapabilitiesModel NewCapabilitiesModel(WebDriverOptionsBase options)
        {
            // Initialize a new CapabilitiesModel to store the extracted capabilities.
            var capabilities = new CapabilitiesModel();

            // Retrieve the vendor-specific options key.
            var optionsKey = GetVendorOptionsKey(options);

            // Initialize a dictionary to store vendor-specific options.
            var vendorOptions = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            // Get all public instance properties from the options object, including those from base classes.
            var properties = options
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            // Extract known and vendor-specific capabilities from the properties.
            var (knownCapabilities, vendorCapabilities) = GetCapabilities(properties);

            // Populate AlwaysMatch with known capabilities that have non-null and non-default values.
            foreach (var capability in knownCapabilities)
            {
                var value = capability.GetValue(options);
                if (value.AssertNullOrDefault())
                {
                    continue; // Skip capabilities with null or default values.
                }

                // Retrieve the capability name from the KnownCapabilityAttribute.
                var name = capability.GetCustomAttribute<KnownCapabilityAttribute>().CapabilityName;

                // Add the capability to the AlwaysMatch dictionary.
                capabilities.AlwaysMatch[name] = value;
            }

            // Include capabilities stored directly in the options' internal dictionary.
            foreach (var capability in options._capabilities)
            {
                capabilities.AlwaysMatch[capability.Key] = capability.Value;
            }

            // Populate vendorOptions with vendor-specific capabilities that have non-null and non-default values.
            foreach (var capability in vendorCapabilities)
            {
                var value = capability.GetValue(options);
                if (value.AssertNullOrDefault())
                {
                    continue; // Skip vendor capabilities with null or default values.
                }

                // Retrieve the vendor capability name from the VendorCapabilityAttribute.
                var name = capability.GetCustomAttribute<VendorCapabilityAttribute>().CapabilityName;

                // Add the vendor capability to the vendorOptions dictionary.
                vendorOptions[name] = value;
            }

            // Include vendor options stored directly in the options' internal dictionary.
            foreach (var capability in options._options)
            {
                vendorOptions[capability.Key] = capability.Value;
            }

            // If there are any vendor capabilities, add them to the AlwaysMatch dictionary under the vendor options key.
            if (vendorOptions.Count != 0)
            {
                capabilities.AlwaysMatch[optionsKey] = vendorOptions;
            }

            // Return the populated CapabilitiesModel.
            return capabilities;
        }

        // Retrieves the vendor-specific options key for the given WebDriver options.
        private static string GetVendorOptionsKey(WebDriverOptionsBase options)
        {
            // Get the type of the provided WebDriverOptionsBase instance.
            var type = options.GetType();

            // Retrieve the WebDriverOptionsAttribute from the type, if it exists.
            var attribute = type.GetCustomAttribute<WebDriverOptionsAttribute>();

            // If the attribute is not present, return an empty string.
            // Otherwise, return the formatted vendor options key using the prefix and options key from the attribute.
            return attribute == null
                ? string.Empty
                : $"{attribute.Prefix}:{attribute.OptionsKey}";
        }

        // Retrieves the vendor-specific prefix key for the given WebDriver options.
        private static string GetVendorPrefixKey(WebDriverOptionsBase options)
        {
            // Get the type of the provided WebDriverOptionsBase instance.
            var type = options.GetType();

            // Retrieve the WebDriverOptionsAttribute from the type, if it exists.
            var attribute = type.GetCustomAttribute<WebDriverOptionsAttribute>();

            // If the attribute is not present, return an empty string.
            // Otherwise, return the prefix from the attribute.
            return attribute == null
                ? string.Empty
                : attribute.Prefix;
        }

        // Retrieves the properties of a class that are marked as known or vendor-specific capabilities.
        private static (IEnumerable<PropertyInfo> Known, IEnumerable<PropertyInfo> Vendor) GetCapabilities(IEnumerable<PropertyInfo> properties)
        {
            // Initialize lists to store known and vendor-specific capabilities.
            var known = new List<PropertyInfo>();
            var vendor = new List<PropertyInfo>();

            // Iterate over each property in the provided collection.
            foreach (var property in properties)
            {
                // Check if the property is marked with the KnownCapabilityAttribute.
                var isKnown = property.GetCustomAttribute<KnownCapabilityAttribute>() != null;

                // Check if the property is marked with the VendorCapabilityAttribute.
                var isVendor = property.GetCustomAttribute<VendorCapabilityAttribute>() != null;

                // If the property does not have either attribute, skip it.
                if (!isKnown && !isVendor)
                {
                    continue;
                }

                // If the property is a known capability, add it to the known list.
                if (isKnown)
                {
                    known.Add(property);
                }

                // If the property is a vendor-specific capability, add it to the vendor list.
                if (isVendor)
                {
                    vendor.Add(property);
                }
            }

            // Return a tuple containing both known and vendor-specific capabilities.
            return (known, vendor);
        }

        // Retrieves the names of known capabilities from the given WebDriverOptionsBase instance.
        private static IEnumerable<string> GetKnownCapabilities(WebDriverOptionsBase options) => options.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
            .Where(i => i.GetCustomAttribute<KnownCapabilityAttribute>() != null)
            .Select(i => i.GetCustomAttribute<KnownCapabilityAttribute>().CapabilityName);
        #endregion
    }
}
