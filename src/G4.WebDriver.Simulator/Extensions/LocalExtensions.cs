using System;
using System.Collections.Generic;
using System.Linq;

namespace G4.WebDriver.Extensions
{
    /// <summary>
    /// Provides extension methods for various utility operations used in the G4 WebDriver.
    /// </summary>
    internal static class LocalExtensions
    {
        /// <summary>
        /// Attempts to get the value associated with the specified key in a case-insensitive manner.
        /// If the dictionary is already using a case-insensitive comparer, it utilizes the standard TryGetValue.
        /// Otherwise, it performs a case-insensitive search.
        /// </summary>
        /// <typeparam name="TValue">The type of the value associated with the key.</typeparam>
        /// <param name="data">The dictionary to search.</param>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter.</param>
        /// <returns>true if the dictionary contains an element with the specified key; otherwise, false.</returns>
        public static bool TryGetValueCaseInsensitive<TValue>(this IDictionary<string, TValue> data, string key, out TValue value)
        {
            // Attempt to get the value using the standard TryGetValue
            if (data.TryGetValue(key, out value))
            {
                return true;
            }

            // Determine if the dictionary's comparer is case-insensitive
            bool isCaseInsensitive = false;

            // Check if the dictionary uses a case-insensitive comparer
            if (data is Dictionary<string, TValue> concreteDict)
            {
                // Check if the dictionary uses a case-insensitive comparer
                isCaseInsensitive = concreteDict.Comparer.Equals(StringComparer.OrdinalIgnoreCase) ||
                    concreteDict.Comparer.Equals(StringComparer.InvariantCultureIgnoreCase) ||
                    concreteDict.Comparer.Equals(StringComparer.CurrentCultureIgnoreCase);
            }
            else if (data is IReadOnlyDictionary<string, TValue> readOnlyDict)
            {
                // Attempt to access the comparer via reflection for IReadOnlyDictionary implementations
                var comparerProperty = typeof(IReadOnlyDictionary<string, TValue>).GetProperty("Comparer");

                // Check if the comparer is case-insensitive for the IReadOnlyDictionary implementation
                if (comparerProperty?.GetValue(readOnlyDict) is IEqualityComparer<string> comparer)
                {
                    isCaseInsensitive = comparer.Equals(StringComparer.OrdinalIgnoreCase) ||
                        comparer.Equals(StringComparer.InvariantCultureIgnoreCase) ||
                        comparer.Equals(StringComparer.CurrentCultureIgnoreCase);
                }
            }

            if (isCaseInsensitive)
            {
                // If the comparer is case-insensitive and key wasn't found, return false
                value = default;
                return false;
            }

            // Perform a case-insensitive search manually
            foreach (var (k, v) in data.Select(i => (i.Key, i.Value)))
            {
                if (string.Equals(k, key, StringComparison.OrdinalIgnoreCase))
                {
                    value = v;
                    return true;
                }
            }

            // If no match is found, set value to default and return false
            value = default;
            return false;
        }
    }
}
