/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/ShadowRoot.cs
 */
using G4.WebDriver.Extensions;
using G4.WebDriver.Models;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Represents a shadow DOM root, allowing interaction with shadow DOM elements.
    /// Implements the <see cref="ISearchContext"/>, <see cref="IDriverReference"/>, and <see cref="IIdentifierReference"/> interfaces.
    /// </summary>
    /// <param name="driver">The WebDriver instance used to interact with the browser.</param>
    /// <param name="id">The unique identifier of the shadow DOM root.</param>
    public class ShadowRoot(IWebDriver driver, string id) : ISearchContext, IDriverReference, IIdentifierReference
    {
        #region *** Fields     ***
        // Invoker to send WebDriver commands to the browser.
        private readonly IWebDriverCommandInvoker _invoker = driver.Invoker;

        // Session identifier associated with the WebDriver instance.
        private readonly SessionIdModel _sessionId = driver.Invoker.Session;
        #endregion

        #region *** Properties ***
        /// <summary>
        /// Gets the WebDriver instance used to interact with the browser.
        /// </summary>
        public virtual IWebDriver Driver { get; } = driver;

        /// <summary>
        /// Gets the unique identifier of the shadow DOM root.
        /// </summary>
        public virtual string Id { get; } = id;
        #endregion

        #region *** Methods    ***
        /// <summary>
        /// Finds an element within the shadow DOM root using the specified <see cref="By"/> locator.
        /// </summary>
        /// <param name="by">The <see cref="By"/> locator to use for finding the element.</param>
        /// <returns>The <see cref="IWebElement"/> found within the shadow DOM root.</returns>
        public IWebElement FindElement(By by)
        {
            // Prepare data for the WebDriver command using the locator details.
            var data = new { by.Using, by.Value };

            // Set up the command to find an element within the shadow DOM root.
            var command = WebDriverCommands
                .FindElementFromElement
                .SetSession(_sessionId.OpaqueKey)
                .SetElement(Id)
                .SetData(data);

            // Invoke the command and get the response from the browser.
            var response = _invoker.Invoke(command);

            // Extract the element ID from the response.
            var value = (JsonElement)response.Value;
            var elementId = value.ConvertToDictionary().First().Value.ToString();

            // Return a new WebElement representing the found element.
            return new WebElement(driver: Driver, elementId);
        }

        /// <summary>
        /// Finds all elements within the shadow DOM root using the specified <see cref="By"/> locator.
        /// </summary>
        /// <param name="by">The <see cref="By"/> locator to use for finding the elements.</param>
        /// <returns>A read-only collection of <see cref="IWebElement"/> objects found within the shadow DOM root.</returns>
        public IEnumerable<IWebElement> FindElements(By by)
        {
            // Prepare data for the WebDriver command using the locator details.
            var data = new { by.Using, by.Value };

            // Set up the command to find elements within the shadow DOM root.
            var command = WebDriverCommands
                .FindElementsFromElement
                .SetSession(_sessionId.OpaqueKey)
                .SetElement(Id)
                .SetData(data);

            // Invoke the command and get the response from the browser.
            var response = _invoker.Invoke(command);

            // Parse the JSON response to extract element information.
            var value = ((JsonElement)response.Value).EnumerateArray().ToArray();

            // Create a list of WebElement objects based on the extracted element IDs.
            var elements = value
                .Select(i => i.ConvertToDictionary())
                .Select(i => new WebElement(driver: Driver, id: i.First().Value.ToString()) as IWebElement)
                .ToList();

            // Return the list as a read-only collection.
            return new ReadOnlyCollection<IWebElement>(elements);
        }
        #endregion
    }
}
