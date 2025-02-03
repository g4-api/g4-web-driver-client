/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/WebElement.cs
 * https://www.w3.org/TR/webdriver/#elements
 */
using G4.WebDriver.Extensions;
using G4.WebDriver.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Represents a WebElement providing methods and properties to interact with web elements.
    /// </summary>
    /// <param name="driver">The <see cref="IWebDriver"/> instance associated with the WebElement.</param>
    /// <param name="id">The unique identifier of the WebElement.</param>
    /// <remarks>This constructor sets up the WebElement with the provided WebDriver, ID, and retrieves the tag name of the element.</remarks>
    public class WebElement(IWebDriver driver, string id) : IWebElement, ILocatable
    {
        #region *** Fields     ***
        // The invoker responsible for executing WebDriver commands.
        private readonly IWebDriverCommandInvoker _invoker = driver.Invoker;

        // The session ID associated with the WebDriver session.
        private readonly SessionIdModel _sessionId = driver.Invoker.Session;
        #endregion

        #region *** Properties ***
        /// <inheritdoc />
        public virtual bool Displayed => ConfirmElementVisibility(this);

        /// <inheritdoc />
        public virtual IWebDriver Driver { get; } = driver;

        /// <inheritdoc />
        public bool Enabled => _invoker
            .Invoke<bool>(WebDriverCommands.GetElementEnabled.SetSession(_sessionId.OpaqueKey).SetElement(Id));

        /// <inheritdoc />
        public virtual string Id { get; } = id;

        /// <inheritdoc />
        public RectModel Rect => _invoker
            .Invoke<RectModel>(WebDriverCommands.GetElementRect.SetSession(_sessionId.OpaqueKey).SetElement(Id));

        /// <inheritdoc />
        public bool Selected => _invoker
            .Invoke<bool>(WebDriverCommands.GetElementSelected.SetSession(_sessionId.OpaqueKey).SetElement(Id));

        /// <inheritdoc />
        public string TagName => _invoker
            .Invoke<string>(WebDriverCommands.GetElementTagName.SetSession(_sessionId.OpaqueKey).SetElement(Id));

        /// <inheritdoc />
        public string Text => _invoker
            .Invoke<string>(WebDriverCommands.GetElementText.SetSession(_sessionId.OpaqueKey).SetElement(Id));

        public PointModel LocationOnView => throw new NotImplementedException();

        public ICoordinates Coordinates => throw new NotImplementedException();
        #endregion

        #region *** Methods    ***
        /// <inheritdoc />
        public void Click()
        {
            // Create a command to simulate a click on the element using WebDriverCommands
            var command = WebDriverCommands
                .ClickElement
                .SetSession(_sessionId.OpaqueKey)
                .SetElement(Id);

            // Invoke the command to perform the click action on the element
            _invoker.Invoke(command);
        }

        /// <inheritdoc />
        public IWebElement Clear()
        {
            // Create a command to clear the element using WebDriverCommands
            var command = WebDriverCommands
                .ClearElement
                .SetSession(_sessionId.OpaqueKey)
                .SetElement(Id);

            // Invoke the command to clear the element's content
            _invoker.Invoke(command);

            // Return the current instance of the IWebElement after clearing
            return this;
        }

        /// <inheritdoc />
        public IWebElement FindElement(By by)
        {
            // Extracting the locator strategy and value from the 'By' object
            var data = new { by.Using, by.Value };

            // Creating a command to find the element using WebDriverCommands
            var command = WebDriverCommands
                .FindElementFromElement
                .SetSession(_sessionId.OpaqueKey)
                .SetElement(Id)
                .SetData(data);

            // Invoking the command and retrieving the response
            var response = _invoker.Invoke(command);

            // Extracting the element's ID from the response
            var value = (JsonElement)response.Value;
            var elementId = value.ConvertToDictionary().First().Value.ToString();

            // Creating and returning a new WebElement based on the driver and ID
            return new WebElement(driver: Driver, elementId);
        }

        /// <inheritdoc />
        public IEnumerable<IWebElement> FindElements(By by)
        {
            // Extracting the locator strategy and value from the 'By' object
            var data = new { by.Using, by.Value };

            // Creating a command to find elements using WebDriverCommands
            var command = WebDriverCommands
                .FindElementsFromElement
                .SetSession(_sessionId.OpaqueKey)
                .SetElement(Id)
                .SetData(data);

            // Invoking the command and retrieving the response
            var response = _invoker.Invoke(command);

            // Extracting the array of elements from the response
            var value = ((JsonElement)response.Value).EnumerateArray().ToArray();

            // Converting the JSON elements to dictionaries and then creating WebElement objects
            var elements = value
                .Select(i => i.ConvertToDictionary())
                .Select(i => new WebElement(driver: Driver, id: i.First().Value.ToString()) as IWebElement)
                .ToList();

            // Returning a read-only collection of IWebElement
            return new ReadOnlyCollection<IWebElement>(elements);
        }

        /// <inheritdoc />
        public string GetAttribute(string attributeName)
        {
            // Prepare the WebDriver command for retrieving the value of an attribute for the current WebElement
            var command = WebDriverCommands
                .GetElementAttribute
                .SetSession(_sessionId.OpaqueKey)
                .SetElement(Id);

            // Replace the placeholder in the route with the actual attribute name
            command.Route = command.Route.Replace($"$[{nameof(attributeName)}]", attributeName);

            // Invoke the GetElementAttribute WebDriver command
            var response = _invoker.Invoke(command);

            // Extract and return the attribute value, or null if the value is null
            return response.Value == null
                ? null
                : $"{response.Value}";
        }

        /// <inheritdoc />
        public string GetCssValue(string propertyName)
        {
            // Prepare the WebDriver command for retrieving the computed CSS value of a property for the current WebElement
            var command = WebDriverCommands
                .GetElementCssValue
                .SetSession(_sessionId.OpaqueKey)
                .SetElement(Id);

            // Replace the placeholder in the route with the actual property name
            command.Route = command.Route.Replace($"$[{nameof(propertyName)}]", propertyName);

            // Invoke the GetElementCssValue WebDriver command
            var response = _invoker.Invoke(command);

            // Extract and return the computed CSS value, or an empty string if the value is null
            return response.Value == null ? string.Empty : $"{response.Value}";
        }

        /// <inheritdoc />
        public string GetProperty(string propertyName)
        {
            // Prepare the WebDriver command for retrieving the value of a property for the current WebElement
            var command = WebDriverCommands
                .GetElementProperty
                .SetSession(_sessionId.OpaqueKey)
                .SetElement(Id);

            // Replace the placeholder in the route with the actual property name
            command.Route = command.Route.Replace($"$[{nameof(propertyName)}]", propertyName);

            // Invoke the GetElementProperty WebDriver command
            var response = _invoker.Invoke(command);

            // Extract and return the property value, or null if the value is null
            return response.Value == null
                ? null
                : $"{response.Value}";
        }

        /// <inheritdoc />
        public ScreenshotModel GetScreenshot()
        {
            // Prepare the WebDriver command for capturing a screenshot of the current WebElement
            var command = WebDriverCommands
                .GetElementScreenshot
                .SetSession(_sessionId.OpaqueKey)
                .SetElement(Id);

            // Invoke the GetElementScreenshot WebDriver command
            var response = _invoker.Invoke(command);

            // Extract the Base64 representation of the screenshot from the response
            var base64Encoded = response.Value == null
                ? string.Empty
                : $"{response.Value}";

            // Create and return a ScreenshotModel with the Base64 representation
            return new ScreenshotModel(base64Encoded);
        }

        /// <inheritdoc />
        public void SendKeys(string text)
        {
            // Prepare data for the SendKeys command, including the original text and its character array representation
            var data = new
            {
                Text = text,
                Value = text.ToCharArray()
            };

            // Create a command to simulate sending keys using WebDriverCommands
            var command = WebDriverCommands
                .SendKeys
                .SetSession(_sessionId.OpaqueKey)
                .SetElement(Id)
                .SetData(data);

            // Invoke the command to send keys to the element
            _invoker.Invoke(command);
        }

        /// <summary>
        /// Returns a string representation of the current WebElement.
        /// </summary>
        /// <returns>A string containing the element's ID.</returns>
        public override string ToString() => Id;

        /// <summary>
        /// Serves as a hash function for the current WebElement.
        /// </summary>
        /// <returns>A hash code based on the element's ID.</returns>
        public override int GetHashCode() => Id.GetHashCode();

        /// <summary>
        /// Determines whether the current WebElement is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare with the current WebElement.</param>
        /// <returns><c>true</c> if the specified object is a WebElement and has the same ID (case-insensitive) as the current WebElement; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            // Check if the provided object is a WebElement
            var isWebElement = obj is WebElement;

            // If the object is a WebElement, compare their IDs (case-insensitive)
            return
                isWebElement &&
                Id.Equals(((WebElement)obj).Id, StringComparison.OrdinalIgnoreCase);
        }

        // Confirms whether the specified WebElement is visible on the page.
        private static bool ConfirmElementVisibility(IWebElement element)
        {
            // Use case-insensitive string comparison
            const StringComparison Compare = StringComparison.OrdinalIgnoreCase;

            // Possible values indicating element is not visible
            var notVisibleStatuses = new List<string>
            {
                "hidden",
                "none",
                "undefined",
                null
            };

            // Prepare data for executing a script to check element visibility
            var data = new
            {
                Script = "return window.getComputedStyle(arguments[0],null).display;"
            };

            // Invoke the script to get the visibility status of the element
            var response = element.Driver.InvokeScript<string>(data.Script, element);

            // Check if the response matches any of the not visible statuses
            return !notVisibleStatuses.Exists(i => i?.Equals(response, Compare) == true);
        }
        #endregion
    }
}
