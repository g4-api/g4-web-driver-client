using G4.WebDriver.Attributes;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Provides a base class for converting properties marked with <see cref="KnownCapabilityAttribute"/>
    /// into a dictionary of capabilities for WebDriver sessions.
    /// </summary>
    public abstract class CapabilitiesDictionaryBase : ICapabilitiesDictionary
    {
        /// <summary>
        /// Converts the properties of the current instance that are marked with <see cref="KnownCapabilityAttribute"/>
        /// into a dictionary of key-value pairs representing WebDriver capabilities.
        /// </summary>
        /// <returns>A dictionary where the keys are the capability names and the values are their corresponding settings.</returns>
        public IDictionary<string, object> ConvertToDictionary()
        {
            // Define the binding flags to use when getting properties from the current instance.
            const BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance;

            // Get all public instance properties that are marked with the KnownCapabilityAttribute.
            var properties = GetType()
                .GetProperties(Flags)
                .Where(i => i.GetCustomAttribute<KnownCapabilityAttribute>() != null);

            // Initialize a dictionary to hold the capabilities.
            var capabilities = new Dictionary<string, object>();

            // Iterate over each property that is marked with the KnownCapabilityAttribute.
            foreach (var property in properties)
            {
                // Get the value of the property for the current instance.
                var value = property.GetValue(this);

                // Check if the property value is its default value or an empty string.
                var isDefault = value == default || (value is string && string.IsNullOrEmpty($"{value}"));

                // Check if the property value is an instance of ICapabilitiesDictionary.
                var isDictionary = value is ICapabilitiesDictionary;

                // If the property has its default value or an empty string, skip it.
                if (isDefault)
                {
                    continue;
                }

                // Get the capability name from the KnownCapabilityAttribute.
                var key = property.GetCustomAttribute<KnownCapabilityAttribute>().CapabilityName;

                // Add the property value to the dictionary, converting it to a dictionary if necessary.
                capabilities[key] = isDictionary
                    ? ((ICapabilitiesDictionary)value).ConvertToDictionary()
                    : value;
            }

            // Return the dictionary of capabilities.
            return capabilities;
        }
    }
}
