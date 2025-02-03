using G4.WebDriver.Extensions;

namespace G4.WebDriver.Remote.Edge
{
    /// <summary>
    /// Represents a service for managing the lifecycle of an Edge WebDriver instance.
    /// </summary>
    public class EdgeWebDriverService : WebDriverService
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeWebDriverService"/> class with default binary path (current directory).
        /// </summary>
        public EdgeWebDriverService()
            : this(binariesPath: ".")
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeWebDriverService"/> class with a specified binaries path and default executable name.
        /// </summary>
        /// <param name="binariesPath">The path to the directory containing the WebDriver binaries.</param>
        public EdgeWebDriverService(string binariesPath)
            : this(binariesPath, executableName: "msedgedriver.exe")
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeWebDriverService"/> class with specified binaries path and executable name.
        /// </summary>
        /// <param name="binariesPath">The path to the directory containing the WebDriver binaries.</param>
        /// <param name="executableName">The name of the WebDriver executable file.</param>
        public EdgeWebDriverService(string binariesPath, string executableName)
            : this(binariesPath, executableName, arguments: string.Empty)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <inheritdoc />
        public EdgeWebDriverService(string binariesPath, string executableName, string arguments)
            : base(binariesPath, executableName, arguments, WebDriverUtilities.GetFreePort(defaultPort: 9516))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <inheritdoc />
        public EdgeWebDriverService(string binariesPath, string executableName, string arguments, int port)
            : base(binariesPath, executableName, arguments, port, string.Empty)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <inheritdoc />
        public EdgeWebDriverService(string binariesPath, string executableName, string arguments, int port, string downloadUrl)
            : base(binariesPath, executableName, arguments, port, downloadUrl)
        { }
        #endregion
    }
}
