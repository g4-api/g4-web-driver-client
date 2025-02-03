using G4.WebDriver.Extensions;

namespace G4.WebDriver.Remote.Chrome
{
    /// <summary>
    /// Represents a service for managing the lifecycle of an Chrome WebDriver instance.
    /// </summary>
    public class ChromeWebDriverService : WebDriverService
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromeWebDriverService"/> class
        /// with default binary path (current directory).
        /// </summary>
        public ChromeWebDriverService()
            : this(binariesPath: ".")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromeWebDriverService"/> class
        /// with a specified binary path.
        /// </summary>
        /// <param name="binariesPath">The path to the ChromeDriver binaries.</param>
        public ChromeWebDriverService(string binariesPath)
            : this(binariesPath, executableName: "chromedriver.exe")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromeWebDriverService"/> class
        /// with specified binary path and executable name.
        /// </summary>
        /// <param name="binariesPath">The path to the ChromeDriver binaries.</param>
        /// <param name="executableName">The name of the ChromeDriver executable file.</param>
        public ChromeWebDriverService(string binariesPath, string executableName)
            : this(binariesPath, executableName, arguments: string.Empty)
        { }

        /// <inheritdoc />
        public ChromeWebDriverService(string binariesPath, string executableName, string arguments)
            : base(binariesPath, executableName, arguments, port: WebDriverUtilities.GetFreePort(defaultPort: 9515))
        { }

        /// <inheritdoc />
        public ChromeWebDriverService(string binariesPath, string executableName, string arguments, int port)
            : base(binariesPath, executableName, arguments, port, downloadUrl: string.Empty)
        { }

        /// <inheritdoc />
        public ChromeWebDriverService(string binariesPath, string executableName, string arguments, int port, string downloadUrl)
            : base(binariesPath, executableName, arguments, port, downloadUrl)
        { }
        #endregion
    }
}
