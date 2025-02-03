using G4.WebDriver.Extensions;

namespace G4.WebDriver.Remote.Appium
{
    /// <summary>
    /// Represents a service for managing the lifecycle of an Appium WebDriver instance.
    /// </summary>
    public abstract class AppiumWebDriverServiceBase : WebDriverService
    {
        #region *** Constructors ***
        /// <inheritdoc />
        protected AppiumWebDriverServiceBase()
            : this(binariesPath: ".", executableName: "appium")
        { }

        /// <inheritdoc />
        protected AppiumWebDriverServiceBase(string binariesPath)
            : this(binariesPath, executableName: "appium")
        { }

        /// <inheritdoc />
        protected AppiumWebDriverServiceBase(string binariesPath, string executableName)
            : this(binariesPath, executableName, arguments: "-pa /wd/hub")
        { }

        /// <inheritdoc />
        protected AppiumWebDriverServiceBase(string binariesPath, string executableName, string arguments)
            : base(binariesPath, executableName, arguments: FormatArguments(arguments), WebDriverUtilities.GetFreePort(defaultPort: 4723))
        { }

        /// <inheritdoc />
        protected AppiumWebDriverServiceBase(string binariesPath, string executableName, string arguments, int port)
            : base(binariesPath, executableName, arguments, port, string.Empty)
        { }

        /// <inheritdoc />
        protected AppiumWebDriverServiceBase(string binariesPath, string executableName, string arguments, int port, string downloadUrl)
            : base(binariesPath, executableName, arguments, port, downloadUrl)
        { }
        #endregion

        #region *** Methods      ***
        // Formats the WebDriver arguments, ensuring that the Appium port is set to /wd/hub.
        private static string FormatArguments(string arguments)
        {
            // If the original arguments are null or empty, set the Appium port to /wd/hub
            return string.IsNullOrEmpty(arguments)
                ? "-pa /wd/hub"
                : $"{arguments} -pa /wd/hub";
        }
        #endregion
    }
}
