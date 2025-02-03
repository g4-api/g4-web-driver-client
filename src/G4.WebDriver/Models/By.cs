/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/By.cs
 */
using System;
using System.Text.Json.Serialization;

namespace G4.WebDriver.Models
{
    /// <summary>
    /// A model for a lookup of an individual element and collections of elements.
    /// </summary>
    /// <param name="using">An enumerated attribute deciding what technique should be used to search for elements in the current browsing context.</param>
    /// <param name="value">The selector to use with the searching technique.</param>
    public class By(string @using, string value)
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the By class.
        /// </summary>
        public By()
            : this(@using: string.Empty, value: string.Empty)
        { }
        #endregion

        #region *** Properties   ***
        /// <summary>
        /// Get or sets an enumerated attribute deciding what technique should be
        /// used to search for elements in the current browsing context.
        /// </summary>
        public string Using { get; set; } = @using;

        /// <summary>
        /// Gets or sets the selector to use with the searching technique.
        /// </summary>
        public string Value { get; set; } = value;

        /// <summary>
        /// Gets or sets the a descriptive phrase of the By object.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets a new By instance.
        /// </summary>
        [JsonIgnore]
        public static By Custom => new();
        #endregion

        #region *** Methods      ***
        /// <summary>
        /// Creates a new By object that locates elements using a CSS selector.
        /// </summary>
        /// <param name="cssSelectorToFind">The CSS selector string used to locate elements.</param>
        /// <returns>Returns a By object configured to find elements using the specified CSS selector.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the cssSelectorToFind is null or empty.</exception>
        public static By CssSelector(string cssSelectorToFind)
        {
            // Check if the provided CSS selector is null or empty.
            if (string.IsNullOrEmpty(cssSelectorToFind))
            {
                // Error message explaining the issue.
                const string error = "The CSS selector cannot be null or empty.";

                 // Throw an ArgumentNullException if the CSS selector is null or empty.
                throw new ArgumentNullException(paramName: nameof(cssSelectorToFind), message: error);
            }

            // Return a new By object configured to use the specified CSS selector.
            return new By
            {
                Description = $"By.CssSelector: {cssSelectorToFind}",
                Using = "css selector",
                Value = cssSelectorToFind
            };
        }

        /// <summary>
        /// Creates a new By object that locates elements using the visible link text.
        /// </summary>
        /// <param name="linkTextToFind">The visible text of the link used to locate elements.</param>
        /// <returns>Returns a By object configured to find elements using the specified link text.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the linkTextToFind is null or empty.</exception>
        public static By LinkText(string linkTextToFind)
        {
            // Check if the provided link text is null or empty.
            if (string.IsNullOrEmpty(linkTextToFind))
            {
                // Error message explaining the issue.
                const string error = "The link text cannot be null or empty.";

                // Throw an ArgumentNullException if the link text is null or empty.
                throw new ArgumentNullException(paramName: nameof(linkTextToFind), message: error);
            }

            // Return a new By object configured to use the specified link text.
            return new By
            {
                Description = $"By.LinkText: {linkTextToFind}",
                Using = "link text",
                Value = linkTextToFind
            };
        }

        /// <summary>
        /// Creates a new By object that locates elements using a portion of the visible link text.
        /// </summary>
        /// <param name="partialLinkTextToFind">The partial visible text of the link used to locate elements.</param>
        /// <returns>Returns a By object configured to find elements using the specified partial link text.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the partialLinkTextToFind is null or empty.</exception>
        public static By PartialLinkText(string partialLinkTextToFind)
        {
            // Check if the provided partial link text is null or empty.
            if (string.IsNullOrEmpty(partialLinkTextToFind))
            {
                // Error message explaining the issue.
                const string error = "The partial link text cannot be null or empty.";

                // Throw an ArgumentNullException if the partial link text is null or empty.
                throw new ArgumentNullException(paramName: nameof(partialLinkTextToFind), message: error);
            }

            // Return a new By object configured to use the specified partial link text.
            return new By
            {
                Description = $"By.PartialLinkText: {partialLinkTextToFind}",
                Using = "partial link text",
                Value = partialLinkTextToFind
            };
        }

        /// <summary>
        /// Creates a new By object that locates elements using their tag name.
        /// </summary>
        /// <param name="tagNameToFind">The tag name of the elements to locate.</param>
        /// <returns>Returns a By object configured to find elements using the specified tag name.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the tagNameToFind is null or empty.</exception>
        public static By TagName(string tagNameToFind)
        {
            // Check if the provided tag name is null or empty.
            if (string.IsNullOrEmpty(tagNameToFind))
            {
                // Error message explaining the issue.
                const string error = "The tag name cannot be null or empty.";

                // Throw an ArgumentNullException if the tag name is null or empty.
                throw new ArgumentNullException(paramName: nameof(tagNameToFind), message: error);
            }

            // Return a new By object configured to use the specified tag name.
            return new By
            {
                Description = $"By.TagName: {tagNameToFind}",
                Using = "tag name",
                Value = tagNameToFind
            };
        }

        /// <summary>
        /// Creates a new By object that locates elements using an XPath expression.
        /// </summary>
        /// <param name="xpathToFind">The XPath expression used to locate elements.</param>
        /// <returns>Returns a By object configured to find elements using the specified XPath expression.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the xpathToFind is null or empty.</exception>
        public static By Xpath(string xpathToFind)
        {
            // Check if the provided XPath expression is null or empty.
            if (string.IsNullOrEmpty(xpathToFind))
            {
                // Error message explaining the issue.
                const string error = "The XPath expression cannot be null or empty.";

                // Throw an ArgumentNullException if the XPath expression is null or empty.
                throw new ArgumentNullException(paramName: nameof(xpathToFind), message: error);
            }

            // Return a new By object configured to use the specified XPath expression.
            return new By
            {
                Description = $"By.XPath: {xpathToFind}",
                Using = "xpath",
                Value = xpathToFind
            };
        }
        #endregion
    }
}
