using G4.WebDriver.Extensions;
using G4.WebDriver.Models;
using G4.WebDriver.Remote.Chromium;

using System;

namespace G4.WebDriver.Remote.Opera
{
    /// <summary>
    /// Represents a WebDriver implementation for Opera, extending the <see cref="ChromiumDriverBase"/>.
    /// </summary>
    public class OperaDriver : ChromiumDriverBase
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the OperaDriver class.
        /// </summary>
        public OperaDriver()
            : this(new OperaOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the OperaDriver class using the specified options.
        /// </summary>
        /// <param name="options">The OperaOptions to be used with the Opera driver.</param>
        public OperaDriver(OperaOptions options)
            : this(options.ConvertToCapabilities().NewSessionModel())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the OperaDriver class using the specified options.
        /// </summary>
        /// <param name="session">The SessionModel to be used with the Opera driver.</param>
        public OperaDriver(SessionModel session)
            : this(new OperaWebDriverService(), session, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the OperaDriver class using the specified driver service.
        /// </summary>
        /// <param name="driverService">The OperaWebDriverService used to initialize the driver.</param>
        public OperaDriver(OperaWebDriverService driverService)
            : this(driverService, new OperaOptions(), TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the OperaDriver class using the specified path
        /// to the directory containing msedgedriver.exe.
        /// </summary>
        /// <param name="binariesPath">The full path to the directory containing msedgedriver.exe.</param>
        public OperaDriver(string binariesPath)
            : this(binariesPath, new OperaOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the OperaDriver class using the specified path
        /// to the directory containing msedgedriver.exe.
        /// </summary>
        /// <param name="binariesPath">The full path to the directory containing msedgedriver.exe.</param>
        /// <param name="options">The OperaOptions to be used with the Opera driver.</param>
        public OperaDriver(string binariesPath, OperaOptions options)
            : this(binariesPath, options.ConvertToCapabilities().NewSessionModel())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the OperaDriver class using the specified path
        /// to the directory containing msedgedriver.exe.
        /// </summary>
        /// <param name="binariesPath">The full path to the directory containing msedgedriver.exe.</param>
        /// <param name="session">The SessionModel to be used with the Opera driver.</param>
        public OperaDriver(string binariesPath, SessionModel session)
            : this(new OperaWebDriverService(binariesPath), session, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the OperaDriver class using the specified path
        /// to the directory containing msedgedriver.exe.
        /// </summary>
        /// <param name="driverService">The OperaWebDriverService used to initialize the driver.</param>
        /// <param name="options">The OperaOptions to be used with the Opera driver.</param>
        /// <param name="timeout">The timeout within which the server must respond.</param>
        public OperaDriver(OperaWebDriverService driverService, OperaOptions options, TimeSpan timeout)
            : this(driverService, options.ConvertToCapabilities().NewSessionModel(), timeout)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the OperaDriver class using the specified path
        /// to the directory containing msedgedriver.exe.
        /// </summary>
        /// <param name="driverService">The OperaWebDriverService used to initialize the driver.</param>
        /// <param name="session">The SessionModel to be used with the Opera driver.</param>
        /// <param name="timeout">The timeout within which the server must respond.</param>
        public OperaDriver(OperaWebDriverService driverService, SessionModel session, TimeSpan timeout)
            : base(new WebDriverCommandInvoker(driverService, timeout), session)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperaDriver"/> class with the specified remote server address.
        /// Uses default <see cref="OperaOptions"/> and timeout of 1 minute.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        public OperaDriver(Uri remoteServerAddress)
            : this(remoteServerAddress, new OperaOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperaDriver"/> class with the specified remote server address and options.
        /// Uses default timeout of 1 minute.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        /// <param name="options">The options for the Opera driver.</param>
        public OperaDriver(Uri remoteServerAddress, OperaOptions options)
            : this(remoteServerAddress, options, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperaDriver"/> class with the specified remote server address, options, and timeout.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        /// <param name="options">The options for the Opera driver.</param>
        /// <param name="timeout">The command execution timeout.</param>
        public OperaDriver(Uri remoteServerAddress, OperaOptions options, TimeSpan timeout)
            : this(remoteServerAddress, options.ConvertToCapabilities().NewSessionModel(), timeout)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperaDriver"/> class using the specified remote server address, session, and timeout.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        /// <param name="session">The session model associated with the WebDriver driver.</param>
        /// <param name="timeout">The timeout duration for WebDriver commands.</param>
        public OperaDriver(Uri remoteServerAddress, SessionModel session, TimeSpan timeout)
            : this(new WebDriverCommandInvoker(remoteServerAddress, timeout), session)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperaDriver"/> class using the specified WebDriver command invoker and session.
        /// </summary>
        /// <param name="invoker">The WebDriver command invoker for the driver.</param>
        /// <param name="session">The session model associated with the WebDriver driver.</param>
        public OperaDriver(IWebDriverCommandInvoker invoker, SessionModel session)
            : base(invoker, session)
        {
            // Additional initialization logic can be added here if needed.
        }
        #endregion
    }
}
