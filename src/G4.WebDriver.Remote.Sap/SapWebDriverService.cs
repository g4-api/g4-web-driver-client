using G4.WebDriver.Extensions;

namespace G4.WebDriver.Remote.Sap
{
    /// <summary>
    /// Represents the service that starts and owns a local SAP GUI WebDriver server process (Sap.DriverServer.exe).
    /// </summary>
    /// <remarks>
    /// The server attaches to SAP GUI sessions that are already running; it never starts SAP GUI or logs on. The
    /// convenience constructors pick the first free port from 5556, the server's own default, so the SAP server can
    /// run beside the UIA server.
    /// </remarks>
    /// <param name="binariesPath">The directory that contains the server executable.</param>
    /// <param name="executableName">The file name of the server executable.</param>
    /// <param name="arguments">The extra command-line arguments passed to the server.</param>
    /// <param name="port">The port the server listens on.</param>
    /// <param name="downloadUrl">The location the base service downloads a missing executable from.</param>
    public class SapWebDriverService(
        string binariesPath,
        string executableName,
        string arguments,
        int port,
        string downloadUrl)
        : WebDriverService(binariesPath, executableName, arguments, port, downloadUrl)
    {
        #region *** Constants    ***
        // Names the SAP driver server executable that the convenience constructors start from the binaries path.
        private const string ExecutableName = "Sap.DriverServer.exe";

        // Defines the first port probed for a free listener, matching the server's own default port.
        private const int DefaultPort = 5556;
        #endregion

        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="SapWebDriverService"/> class that starts Sap.DriverServer.exe
        /// from the current directory on the first free port from 5556.
        /// </summary>
        public SapWebDriverService()
            : this(binariesPath: ".", ExecutableName)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SapWebDriverService"/> class that starts Sap.DriverServer.exe
        /// from the specified directory on the first free port from 5556.
        /// </summary>
        /// <param name="binariesPath">The directory that contains Sap.DriverServer.exe.</param>
        public SapWebDriverService(string binariesPath)
            : this(binariesPath, ExecutableName)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SapWebDriverService"/> class that starts the specified
        /// executable without extra arguments on the first free port from 5556.
        /// </summary>
        /// <param name="binariesPath">The directory that contains the server executable.</param>
        /// <param name="executableName">The file name of the server executable.</param>
        public SapWebDriverService(string binariesPath, string executableName)
            : this(binariesPath, executableName, arguments: string.Empty)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SapWebDriverService"/> class that starts the specified
        /// executable with extra arguments on the first free port from 5556.
        /// </summary>
        /// <param name="binariesPath">The directory that contains the server executable.</param>
        /// <param name="executableName">The file name of the server executable.</param>
        /// <param name="arguments">The extra command-line arguments passed to the server.</param>
        public SapWebDriverService(string binariesPath, string executableName, string arguments)
            : this(binariesPath, executableName, arguments, port: GetFreePort())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SapWebDriverService"/> class that starts the specified
        /// executable on the specified port without a download location.
        /// </summary>
        /// <param name="binariesPath">The directory that contains the server executable.</param>
        /// <param name="executableName">The file name of the server executable.</param>
        /// <param name="arguments">The extra command-line arguments passed to the server.</param>
        /// <param name="port">The port the server listens on.</param>
        public SapWebDriverService(string binariesPath, string executableName, string arguments, int port)
            : this(binariesPath, executableName, arguments, port, downloadUrl: string.Empty)
        { }
        #endregion

        #region *** Properties   ***
        /// <inheritdoc />
        public override string PortParameterFormat => "--port {0}";
        #endregion

        #region *** Methods      ***
        // Finds the first free local port from the server's default port, so a second server on the same machine
        // moves to the next free port instead of failing to bind. The probe opens no lasting listener.
        private static int GetFreePort()
        {
            return WebDriverUtilities.GetFreePort(defaultPort: DefaultPort);
        }
        #endregion
    }
}
