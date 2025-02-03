/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/IOptions.cs
 */
namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Defines an interface for managing various WebDriver options such as cookies, logs, timeouts, and window settings.
    /// </summary>
    public interface IOptions
    {
        /// <summary>
        /// Gets an interface for managing cookies in the browser.
        /// </summary>
        ICookieJar Cookies { get; }

        /// <summary>
        /// Gets an interface for managing log retrieval from the WebDriver.
        /// </summary>
        ILogManager Logs { get; }

        /// <summary>
        /// Gets an interface for managing network settings. (Not implemented in the current version.)
        /// </summary>
        INetworkManager Network { get; }

        /// <summary>
        /// Gets an interface for managing WebDriver timeouts, such as script and page load timeouts.
        /// </summary>
        ITimeouts Timeouts { get; }

        /// <summary>
        /// Gets an interface for managing the browser window, such as size and position.
        /// </summary>
        IWindow Window { get; }
    }
}
