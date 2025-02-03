/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/IWebElement.cs
 */
using G4.WebDriver.Models;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Defines the interface through which the user controls elements on the page.
    /// </summary>
    public interface IWebElement :
        ISearchContext,
        IDriverReference,
        IDisplayable,
        IIdentifierReference,
        ITakesScreenshot
    {
        #region *** Properties ***
        /// <summary>
        /// Gets a value indicating whether the WebElement is enabled.
        /// </summary>
        /// <remarks>This property represents the enabled status of the WebElement, indicating whether it can be interacted with.</remarks>
        bool Enabled { get; }

        /// <summary>
        /// Gets the information about the position and size of the WebElement on the web page.
        /// The RectModel includes details such as X, Y coordinates, width, and height.
        /// </summary>
        RectModel Rect { get; }

        /// <summary>
        /// Gets a value indicating whether it is currently chosen or activated.
        /// </summary>
        bool Selected { get; }

        /// <summary>
        /// Gets the tag name of the WebElement.
        /// </summary>
        string TagName { get; }

        /// <summary>
        /// Gets the visible text content of the WebElement.
        /// </summary>
        string Text { get; }
        #endregion

        #region *** Methods    ***
        /// <summary>
        /// Simulates a click on the associated element.
        /// </summary>
        void Click();

        /// <summary>
        /// Clears the text content of the associated element.
        /// </summary>
        /// <returns>The current instance of the <see cref="IWebElement"/> after clearing the text content.</returns>
        IWebElement Clear();

        /// <summary>
        /// Retrieves the value of a specified attribute for the current WebElement.
        /// This method invokes the GetElementAttribute WebDriver command for the specific attribute and processes the response.
        /// </summary>
        /// <param name="attributeName">The name of the attribute for which to retrieve the value.</param>
        /// <returns>The value of the specified attribute, or null if the attribute is not found or the value is null.</returns>
        string GetAttribute(string attributeName);

        /// <summary>
        /// Retrieves the computed CSS value of a specified property for the current WebElement.
        /// This method invokes the GetElementCssValue WebDriver command for the specific property and processes the response.
        /// </summary>
        /// <param name="propertyName">The name of the CSS property for which to retrieve the computed value.</param>
        /// <returns>The computed CSS value of the specified property, or an empty string if the property is not found or the value is null.</returns>
        string GetCssValue(string propertyName);

        /// <summary>
        /// Retrieves the value of a specified property for the current WebElement.
        /// This method invokes the GetElementProperty WebDriver command for the specific property and processes the response.
        /// </summary>
        /// <param name="propertyName">The name of the property for which to retrieve the value.</param>
        /// <returns>The value of the specified property, or null if the property is not found or the value is null.</returns>
        string GetProperty(string propertyName);

        /// <summary>
        /// Simulates typing the specified text into the associated element.
        /// </summary>
        /// <param name="text">The text to be entered into the element.</param>
        void SendKeys(string text);
        #endregion
    }
}
