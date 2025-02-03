using System.Collections;

namespace G4.WebDriver.Tests.Extensions
{
    /// <summary>
    /// Provides extension methods for working with .NET types.
    /// </summary>
    internal static class LocalExtensions
    {
        /// <summary>
        /// Gets the value associated with the specified key from the dictionary.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary to retrieve the value from.</param>
        /// <param name="key">The key of the value to retrieve.</param>
        /// <returns>The value associated with the specified key, or the default value for the type if the key is not found.</returns>
        public static T Get<T>(this IDictionary dictionary, string key)
        {
            // Check if the dictionary contains the specified key
            if (!dictionary.Contains(key))
            {
                // If the key is not found, return the default value for type T
                return default;
            }

            // Return the value associated with the key, casting it to type T
            return (T)dictionary[key];
        }

        /// <summary>
        /// Gets a value from the dictionary, or a default value if not found.
        /// </summary>
        /// <typeparam name="T">The expected type of the value.</typeparam>
        /// <param name="dictionary">The dictionary to retrieve the value from.</param>
        /// <param name="key">The key of the value to retrieve.</param>
        /// <param name="defaultValue">The default value to return if key is not found.</param>
        /// <returns>The retrieved value or the default value if not found.</returns>
        public static T Get<T>(this IDictionary dictionary, string key, T defaultValue)
        {
            // Check if the key is present in the dictionary.
            var isKey = dictionary.Contains(key);

            // If the key is not present, return the default value.
            if (!isKey)
            {
                return defaultValue;
            }

            // Check if the value associated with the key is of the expected type.
            if (dictionary[key] is T value)
            {
                return value;
            }

            // If the value is not of the expected type, return the default value.
            return defaultValue;
        }
    }
}
