using G4.WebDriver.Extensions;

namespace G4.WebDriver.Remote.Uia
{
    /// <summary>
    /// Represents the service for managing the UI Automation (UIA) WebDriver server.
    /// </summary>
    public class UiaWebDriverService : WebDriverService
    {
        #region *** Constructors ***
        /// <inheritdoc />
        public UiaWebDriverService()
            : this(binariesPath: ".", executableName: "Uia.DriverServer.exe")
        { }

        /// <inheritdoc />
        public UiaWebDriverService(string binariesPath)
            : this(binariesPath, executableName: "Uia.DriverServer.exe")
        { }

        /// <inheritdoc />
        public UiaWebDriverService(string binariesPath, string executableName)
            : this(binariesPath, executableName, arguments: string.Empty)
        { }

        /// <inheritdoc />
        public UiaWebDriverService(string binariesPath, string executableName, string arguments)
            : base(binariesPath, executableName, arguments, port: WebDriverUtilities.GetFreePort(defaultPort: 9577))
        { }

        /// <inheritdoc />
        public UiaWebDriverService(string binariesPath, string executableName, string arguments, int port)
            : base(binariesPath, executableName, arguments, port, downloadUrl: string.Empty)
        { }

        /// <inheritdoc />
        public UiaWebDriverService(string binariesPath, string executableName, string arguments, int port, string downloadUrl)
            : base(binariesPath, executableName, arguments, port, downloadUrl)
        { }
        #endregion

        #region *** Properties   ***
        /// <inheritdoc />
        public override string PortParameterFormat => "--port {0}";
        #endregion
    }
}
