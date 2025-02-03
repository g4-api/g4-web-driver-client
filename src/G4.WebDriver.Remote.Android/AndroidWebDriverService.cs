using G4.WebDriver.Extensions;
using G4.WebDriver.Remote.Appium;

namespace G4.WebDriver.Remote.Android
{
    /// <summary>
    /// Represents a service for managing the lifecycle of an Android WebDriver instance.
    /// </summary>
    public class AndroidWebDriverService : AppiumWebDriverServiceBase
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidWebDriverService"/> class.
        /// </summary>
        public AndroidWebDriverService()
            : this(binariesPath: ".", executableName: "appium")
        { }

        /// <inheritdoc />
        public AndroidWebDriverService(string binariesPath)
            : base(binariesPath)
        { }

        /// <inheritdoc />
        public AndroidWebDriverService(string binariesPath, string executableName)
            : base(binariesPath, executableName)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <inheritdoc />
        public AndroidWebDriverService(string binariesPath, string executableName, string arguments)
            : base(binariesPath, executableName, arguments, port: WebDriverUtilities.GetFreePort(defaultPort: 4723))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <inheritdoc />
        public AndroidWebDriverService(string binariesPath, string executableName, string arguments, int port)
            : base(binariesPath, executableName, arguments, port, downloadUrl: string.Empty)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <inheritdoc />
        public AndroidWebDriverService(string binariesPath, string executableName, string arguments, int port, string downloadUrl)
            : base(binariesPath, executableName, arguments, port, downloadUrl)
        { }
        #endregion
    }
}
