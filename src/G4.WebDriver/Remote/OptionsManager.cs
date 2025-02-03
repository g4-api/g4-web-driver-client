using System;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Provides a mechanism for setting options needed for the WebDriver during automation.
    /// Implements <see cref="IOptions"/> and <see cref="IDriverReference"/> interfaces.
    /// </summary>
    /// <param name="driver">Instance of the WebDriver currently in use.</param>
    public class OptionsManager(IWebDriver driver) : IOptions, IDriverReference
    {
        /// <summary>
        /// Gets an object allowing the user to manipulate cookies on the page.
        /// </summary>
        public ICookieJar Cookies { get; } = new CookieJar(driver);

        /// <summary>
        /// Gets the underlying WebDriver instance.
        /// </summary>
        public IWebDriver Driver { get; } = driver ?? throw new ArgumentNullException(nameof(driver));

        /// <summary>
        /// Gets the log manager, allowing retrieval of logs from the WebDriver.
        /// </summary>
        public ILogManager Logs { get; } = new LogManager(driver);

        /// <summary>
        /// Gets the network manager. (Not implemented in this class.)
        /// </summary>
        public INetworkManager Network => throw new NotImplementedException();

        /// <summary>
        /// Gets a timeouts configuration record of the different timeouts that control
        /// the behavior of script evaluation, navigation, and element retrieval.
        /// </summary>
        public ITimeouts Timeouts { get; } = new WebDriverTimeouts(driver);

        /// <summary>
        /// Gets methods for getting and setting the size and position of the browser window.
        /// </summary>
        public IWindow Window { get; } = new WebDriverWindow(driver);
    }
}
