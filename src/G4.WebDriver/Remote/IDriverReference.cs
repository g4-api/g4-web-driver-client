/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/IWebElement.cs
 */
namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Represents an interface for objects that wrap an <see cref="IWebDriver"/>.
    /// </summary>
    public interface IDriverReference
    {
        /// <summary>
        /// Gets the <see cref="IWebDriver"/> associated with this WebElement.
        /// </summary>
        /// <remarks>This property represents the WebDriver instance responsible for interacting with the web page.</remarks>
        IWebDriver Driver { get; }
    }
}
