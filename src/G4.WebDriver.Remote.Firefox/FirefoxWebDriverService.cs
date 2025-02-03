using G4.WebDriver.Extensions;

namespace G4.WebDriver.Remote.Firefox
{
    /// <summary>
    /// Represents the service for managing the Firefox WebDriver (geckodriver) instance.
    /// Provides various constructors to configure the service with different options such as binaries path, executable name, arguments, port, and download URL.
    /// </summary>
    public class FirefoxWebDriverService : WebDriverService
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="FirefoxWebDriverService"/> class with default binary path (current directory).
        /// </summary>
        public FirefoxWebDriverService()
            : this(binariesPath: ".")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FirefoxWebDriverService"/> class with a specified
        /// binaries path and default executable name.
        /// </summary>
        /// <param name="binariesPath">The path to the directory containing the WebDriver binaries.</param>
        public FirefoxWebDriverService(string binariesPath)
            : this(binariesPath, executableName: "geckodriver.exe")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FirefoxWebDriverService"/> class with specified
        /// binaries path and executable name.
        /// </summary>
        /// <param name="binariesPath">The path to the directory containing the WebDriver binaries.</param>
        /// <param name="executableName">The name of the WebDriver executable file.</param>
        public FirefoxWebDriverService(string binariesPath, string executableName)
            : this(binariesPath, executableName, arguments: string.Empty)
        { }

        /// <inheritdoc />
        public FirefoxWebDriverService(string binariesPath, string executableName, string arguments)
            : base(binariesPath, executableName, arguments, port: WebDriverUtilities.GetFreePort(defaultPort: 9517))
        { }

        /// <inheritdoc />
        public FirefoxWebDriverService(string binariesPath, string executableName, string arguments, int port)
            : base(binariesPath, executableName, arguments, port, downloadUrl: string.Empty)
        { }

        /// <inheritdoc />
        public FirefoxWebDriverService(string binariesPath, string executableName, string arguments, int port, string downloadUrl)
            : base(binariesPath, executableName, arguments, port, downloadUrl)
        { }
        #endregion
    }
}
