/*
 * RESOURCES
 * https://www.w3.org/TR/webdriver/#contexts
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/ITargetLocator.cs
 */
using G4.WebDriver.Exceptions;

namespace G4.WebDriver.Remote
{
    public interface ITargetLocator
    {
        /// <summary>
        /// Retrieves and switches the focus to the currently focused element on the page.
        /// </summary>
        /// <returns>An <see cref="IWebElement"/> representing the currently active element.</returns>
        IWebElement ActiveElement();

        /// <summary>
        /// Retrieves and switches the focus to an alert dialog on the page.
        /// </summary>
        /// <returns>An <see cref="IUserPrompt"/> representing the alert dialog.</returns>
        IUserPrompt Alert();

        /// <summary>
        /// Represents a method to switch the WebDriver focus to a frame by its ID.
        /// </summary>
        /// <param name="element">The WebElement representing the frame to switch to.</param>
        /// <returns>The current instance of the WebDriver for method chaining.</returns>
        IWebDriver Frame(IWebElement element);

        /// <summary>
        /// Represents a method to switch the WebDriver focus to a frame by its ID.
        /// </summary>
        /// <param name="id">The index of the frame to switch to.</param>
        /// <returns>The current instance of the WebDriver for method chaining.</returns>
        IWebDriver Frame(int id);

        /// <summary>
        /// Opens a new window of the specified type and returns the current WebDriver instance.
        /// </summary>
        /// <param name="type">The type of window to open.</param>
        /// <returns>The current WebDriver instance for method chaining.</returns>
        IWebDriver NewWindow(string type);

        /// <summary>
        /// Switches the WebDriver focus to the parent frame.
        /// </summary>
        /// <returns>The current instance of the WebDriver for method chaining.</returns>
        IWebDriver ParentFrame();

        /// <summary>
        /// Switches the focus to a browser window with the specified name or handle.
        /// </summary>
        /// <param name="handle">The name or handle of the target window to switch to.</param>
        /// <returns>The <see cref="IWebDriver"/> instance after switching to the specified window.</returns>
        /// <exception cref="NoSuchWindowException">If the window cannot be found.</exception>
        IWebDriver Window(string handle);
    }
}
