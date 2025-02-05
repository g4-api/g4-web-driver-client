using System.Collections.Generic;
using System.Text.Json;

namespace G4.WebDriver.Extensions
{
    /// <summary>
    /// Provides extension methods for various utility operations used in the G4 WebDriver.
    /// </summary>
    internal static class LocalExtensions
    {
        /// <summary>
        /// Converts a JsonElement to a dictionary representation.
        /// </summary>
        /// <param name="element">The JsonElement to convert.</param>
        /// <returns>A dictionary representing the JsonElement.</returns>
        public static IDictionary<string, object> ConvertToDictionary(this JsonElement element)
        {
            // Get the raw JSON text
            var json = element.GetRawText();

            // Deserialize the JSON into a dictionary
            return JsonSerializer.Deserialize<IDictionary<string, object>>(json);
        }
    }
}
