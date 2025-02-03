using G4.WebDriver.Exceptions;
using G4.WebDriver.Models;

using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace G4.WebDriver.Extensions
{
    public static class ByExtensions
    {
        /// <summary>
        /// Finds elements using the ID attribute.
        /// </summary>
        /// <param name="_">The ignored instance parameter (extension method).</param>
        /// <param name="id">The ID to search for.</param>
        /// <returns>A By object for locating elements by ID.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> is null or empty.</exception>
        public static By Id(this By _, string id)
        {
            // Check if id is null or empty
            if (string.IsNullOrEmpty(id))
            {
                const string error = "The 'id' parameter cannot be null or empty.";
                throw new ArgumentNullException(paramName: nameof(id), message: error);
            }

            // Create a By object for locating elements by ID using XPath
            return new By
            {
                Description = $"By.Id: {id}",
                Using = "xpath",
                Value = $"//*[@id='{id}']"
            };
        }

        /// <summary>
        /// Finds elements using the name attribute.
        /// </summary>
        /// <param name="_">The ignored instance parameter (extension method).</param>
        /// <param name="name">The name attribute to search for.</param>
        /// <returns>A By object for locating elements by name attribute.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is null or empty.</exception>
        public static By Name(this By _, string name)
        {
            // Check if name is null or empty
            if (string.IsNullOrEmpty(name))
            {
                const string error = "The 'name' parameter cannot be null or empty.";
                throw new ArgumentNullException(paramName: nameof(name), message: error);
            }

            // Create a By object for locating elements by name attribute using XPath
            return new By
            {
                Description = $"By.Name: {name}",
                Using = "xpath",
                Value = $"//*[@name='{name}']"
            };
        }

        /// <summary>
        /// Finds elements using the class name attribute.
        /// </summary>
        /// <param name="_">The ignored instance parameter (extension method).</param>
        /// <param name="className">The class name to search for.</param>
        /// <returns>A By object for locating elements by class name.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="className"/> is null or empty.</exception>
        /// <exception cref="InvalidSelectorException">Thrown when <paramref name="className"/> contains whitespace or compound names.</exception>
        public static By PartialClassName(this By _, string className)
        {
            // Formats a CSS selector to escape special characters.
            static string FormatCssSelector(string selector)
            {
                // Use the invariant culture for consistent formatting
                var cultureInfo = CultureInfo.InvariantCulture;

                // Escape special characters in the selector using a regex pattern
                string escaped = Regex.Replace(input: selector, pattern: "([ '\"\\\\#.:;,!?+<>=~*^$|%&@`{}\\-/\\[\\]\\(\\)])", @"\$1");

                // Check if the selector starts with a digit and add a backslash if needed
                if (selector.Length > 0 && char.IsDigit(selector[0]))
                {
                    escaped = @"\" + (30 + int.Parse(selector[..1], cultureInfo)).ToString(cultureInfo) + " " + selector[1..];
                }

                // Return the formatted CSS selector
                return escaped;
            }

            // Check if className is null or empty
            if (string.IsNullOrEmpty(className))
            {
                const string error = "The 'className' parameter cannot be null or empty.";
                throw new ArgumentNullException(paramName: nameof(className), message: error);
            }

            // Format className as a valid CSS selector
            className = FormatCssSelector(className);

            // Check if className contains whitespace or compound names
            if (className.Contains(' '))
            {
                const string error = "The 'className' parameter contains invalid characters or compound names.";
                throw new InvalidSelectorException(error);
            }

            // Create a By object for locating elements by class name using XPath
            return new By
            {
                Description = $"By.ClassName[Contains]: {className}",
                Using = "xpath",
                Value = $"//*[contains(@class, '{className}')]"
            };
        }
    }
}
