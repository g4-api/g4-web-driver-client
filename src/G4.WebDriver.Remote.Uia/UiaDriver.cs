using G4.WebDriver.Extensions;
using G4.WebDriver.Models;

using System;

namespace G4.WebDriver.Remote.Uia
{
    /// <summary>
    /// Represents a WebDriver implementation for UI Automation (UIA), extending the <see cref="RemoteWebDriver"/>.
    /// </summary>
    public class UiaDriver : RemoteWebDriver
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the UiaDriver class.
        /// </summary>
        public UiaDriver()
            : this(new UiaOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the UiaDriver class using the specified options.
        /// </summary>
        /// <param name="options">The UiaOptions to be used with the Uia driver.</param>
        public UiaDriver(UiaOptions options)
            : this(options.ConvertToCapabilities().NewSessionModel())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the UiaDriver class using the specified options.
        /// </summary>
        /// <param name="session">The SessionModel to be used with the Uia driver.</param>
        public UiaDriver(SessionModel session)
            : this(new UiaWebDriverService(), session, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the UiaDriver class using the specified driver service.
        /// </summary>
        /// <param name="driverService">The UiaWebDriverService used to initialize the driver.</param>
        public UiaDriver(UiaWebDriverService driverService)
            : this(driverService, new UiaOptions(), TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the UiaDriver class using the specified path
        /// to the directory containing Uia.DriverServer.exe.
        /// </summary>
        /// <param name="binariesPath">The full path to the directory containing Uia.DriverServer.exe.</param>
        public UiaDriver(string binariesPath)
            : this(binariesPath, new UiaOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the UiaDriver class using the specified path
        /// to the directory containing Uia.DriverServer.exe.
        /// </summary>
        /// <param name="binariesPath">The full path to the directory containing Uia.DriverServer.exe.</param>
        /// <param name="options">The UiaOptions to be used with the Uia driver.</param>
        public UiaDriver(string binariesPath, UiaOptions options)
            : this(binariesPath, options.ConvertToCapabilities().NewSessionModel())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the UiaDriver class using the specified path
        /// to the directory containing Uia.DriverServer.exe.
        /// </summary>
        /// <param name="binariesPath">The full path to the directory containing Uia.DriverServer.exe.</param>
        /// <param name="session">The SessionModel to be used with the Uia driver.</param>
        public UiaDriver(string binariesPath, SessionModel session)
            : this(new UiaWebDriverService(binariesPath), session, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the UiaDriver class using the specified path
        /// to the directory containing Uia.DriverServer.exe.
        /// </summary>
        /// <param name="driverService">The UiaWebDriverService used to initialize the driver.</param>
        /// <param name="options">The UiaOptions to be used with the Uia driver.</param>
        /// <param name="timeout">The timeout within which the server must respond.</param>
        public UiaDriver(UiaWebDriverService driverService, UiaOptions options, TimeSpan timeout)
            : this(driverService, options.ConvertToCapabilities().NewSessionModel(), timeout)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the UiaDriver class using the specified path
        /// to the directory containing Uia.DriverServer.exe.
        /// </summary>
        /// <param name="driverService">The UiaWebDriverService used to initialize the driver.</param>
        /// <param name="session">The SessionModel to be used with the Uia driver.</param>
        /// <param name="timeout">The timeout within which the server must respond.</param>
        public UiaDriver(UiaWebDriverService driverService, SessionModel session, TimeSpan timeout)
            : base(new WebDriverCommandInvoker(driverService, timeout), session)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiaDriver"/> class with the specified remote server address.
        /// Uses default <see cref="UiaOptions"/> and timeout of 1 minute.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        public UiaDriver(Uri remoteServerAddress)
            : this(remoteServerAddress, new UiaOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiaDriver"/> class with the specified remote server address and options.
        /// Uses default timeout of 1 minute.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        /// <param name="options">The options for the Uia driver.</param>
        public UiaDriver(Uri remoteServerAddress, UiaOptions options)
            : this(remoteServerAddress, options, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiaDriver"/> class with the specified remote server address, options, and timeout.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        /// <param name="options">The options for the Uia driver.</param>
        /// <param name="timeout">The command execution timeout.</param>
        public UiaDriver(Uri remoteServerAddress, UiaOptions options, TimeSpan timeout)
            : this(remoteServerAddress, options.ConvertToCapabilities().NewSessionModel(), timeout)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiaDriver"/> class using the specified remote server address, session, and timeout.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        /// <param name="session">The session model associated with the WebDriver driver.</param>
        /// <param name="timeout">The timeout duration for WebDriver commands.</param>
        public UiaDriver(Uri remoteServerAddress, SessionModel session, TimeSpan timeout)
            : this(new WebDriverCommandInvoker(remoteServerAddress, timeout), session)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiaDriver"/> class using the specified WebDriver command invoker and session.
        /// </summary>
        /// <param name="invoker">The WebDriver command invoker for the driver.</param>
        /// <param name="session">The session model associated with the WebDriver driver.</param>
        public UiaDriver(IWebDriverCommandInvoker invoker, SessionModel session)
            : base(invoker, session)
        {
            // Additional initialization logic can be added here if needed.
        }
        #endregion
    }
}
