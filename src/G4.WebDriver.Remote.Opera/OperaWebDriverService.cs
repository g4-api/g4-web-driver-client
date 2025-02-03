using G4.WebDriver.Extensions;

namespace G4.WebDriver.Remote.Opera
{
    /// <summary>
    /// Represents the service for managing the Opera WebDriver (operadriver) instance.
    /// Provides various constructors to configure the service with different options such as binaries path, executable name, arguments, port, and download URL.
    /// </summary>
    public class OperaWebDriverService : WebDriverService
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="OperaWebDriverService"/> class with default
        /// binaries path and executable name.
        /// </summary>
        public OperaWebDriverService()
            : this(binariesPath: ".", executableName: "operadriver.exe")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperaWebDriverService"/> class with a specified
        /// binaries path and default executable name.
        /// </summary>
        /// <param name="binariesPath">The path to the directory containing the WebDriver binaries.</param>
        public OperaWebDriverService(string binariesPath)
            : this(binariesPath, executableName: "operadriver.exe")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperaWebDriverService"/> class with specified
        /// binaries path and executable name.
        /// </summary>
        /// <param name="binariesPath">The path to the directory containing the WebDriver binaries.</param>
        /// <param name="executableName">The name of the WebDriver executable file.</param>
        public OperaWebDriverService(string binariesPath, string executableName)
            : this(binariesPath, executableName, arguments: string.Empty)
        { }

        /// <inheritdoc />
        public OperaWebDriverService(string binariesPath, string executableName, string arguments)
            : base(binariesPath, executableName, arguments, port: WebDriverUtilities.GetFreePort(defaultPort: 9518))
        { }

        /// <inheritdoc />
        public OperaWebDriverService(string binariesPath, string executableName, string arguments, int port)
            : base(binariesPath, executableName, arguments, port, downloadUrl: string.Empty)
        { }

        /// <inheritdoc />
        public OperaWebDriverService(string binariesPath, string executableName, string arguments, int port, string downloadUrl)
            : base(binariesPath, executableName, arguments, port, downloadUrl)
        { }
        #endregion
    }
}
