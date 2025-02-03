using G4.WebDriver.Extensions;
using G4.WebDriver.Models;

using System;
using System.Collections.Generic;
using System.Net.Http;

namespace G4.WebDriver.Remote.Firefox
{
    /// <summary>
    /// Represents a WebDriver implementation for Firefox, extending the <see cref="RemoteWebDriver"/>.
    /// </summary>
    public class FirefoxDriver : RemoteWebDriver
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the FirefoxDriver class.
        /// </summary>
        public FirefoxDriver()
            : this(new FirefoxOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the FirefoxDriver class using the specified options.
        /// </summary>
        /// <param name="options">The FirefoxOptions to be used with the Firefox driver.</param>
        public FirefoxDriver(FirefoxOptions options)
            : this(options.ConvertToCapabilities().NewSessionModel())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the FirefoxDriver class using the specified options.
        /// </summary>
        /// <param name="session">The SessionModel to be used with the Firefox driver.</param>
        public FirefoxDriver(SessionModel session)
            : this(new FirefoxWebDriverService(), session, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the FirefoxDriver class using the specified driver service.
        /// </summary>
        /// <param name="driverService">The FirefoxWebDriverService used to initialize the driver.</param>
        public FirefoxDriver(FirefoxWebDriverService driverService)
            : this(driverService, new FirefoxOptions(), TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the FirefoxDriver class using the specified path
        /// to the directory containing msedgedriver.exe.
        /// </summary>
        /// <param name="binariesPath">The full path to the directory containing msedgedriver.exe.</param>
        public FirefoxDriver(string binariesPath)
            : this(binariesPath, new FirefoxOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the FirefoxDriver class using the specified path
        /// to the directory containing msedgedriver.exe.
        /// </summary>
        /// <param name="binariesPath">The full path to the directory containing msedgedriver.exe.</param>
        /// <param name="options">The FirefoxOptions to be used with the Firefox driver.</param>
        public FirefoxDriver(string binariesPath, FirefoxOptions options)
            : this(binariesPath, options.ConvertToCapabilities().NewSessionModel())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the FirefoxDriver class using the specified path
        /// to the directory containing msedgedriver.exe.
        /// </summary>
        /// <param name="binariesPath">The full path to the directory containing msedgedriver.exe.</param>
        /// <param name="session">The SessionModel to be used with the Firefox driver.</param>
        public FirefoxDriver(string binariesPath, SessionModel session)
            : this(new FirefoxWebDriverService(binariesPath), session, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the FirefoxDriver class using the specified path
        /// to the directory containing msedgedriver.exe.
        /// </summary>
        /// <param name="driverService">The FirefoxWebDriverService used to initialize the driver.</param>
        /// <param name="options">The FirefoxOptions to be used with the Firefox driver.</param>
        /// <param name="timeout">The timeout within which the server must respond.</param>
        public FirefoxDriver(FirefoxWebDriverService driverService, FirefoxOptions options, TimeSpan timeout)
            : this(driverService, options.ConvertToCapabilities().NewSessionModel(), timeout)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the FirefoxDriver class using the specified path
        /// to the directory containing msedgedriver.exe.
        /// </summary>
        /// <param name="driverService">The FirefoxWebDriverService used to initialize the driver.</param>
        /// <param name="session">The SessionModel to be used with the Firefox driver.</param>
        /// <param name="timeout">The timeout within which the server must respond.</param>
        public FirefoxDriver(FirefoxWebDriverService driverService, SessionModel session, TimeSpan timeout)
            : base(new WebDriverCommandInvoker(driverService, timeout), session)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FirefoxDriver"/> class with the specified remote server address.
        /// Uses default <see cref="FirefoxOptions"/> and timeout of 1 minute.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        public FirefoxDriver(Uri remoteServerAddress)
            : this(remoteServerAddress, new FirefoxOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FirefoxDriver"/> class with the specified remote server address and options.
        /// Uses default timeout of 1 minute.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        /// <param name="options">The options for the Firefox driver.</param>
        public FirefoxDriver(Uri remoteServerAddress, FirefoxOptions options)
            : this(remoteServerAddress, options, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FirefoxDriver"/> class with the specified remote server address, options, and timeout.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        /// <param name="options">The options for the Firefox driver.</param>
        /// <param name="timeout">The command execution timeout.</param>
        public FirefoxDriver(Uri remoteServerAddress, FirefoxOptions options, TimeSpan timeout)
            : this(remoteServerAddress, options.ConvertToCapabilities().NewSessionModel(), timeout)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FirefoxDriver"/> class using the specified remote server address, session, and timeout.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        /// <param name="session">The session model associated with the WebDriver driver.</param>
        /// <param name="timeout">The timeout duration for WebDriver commands.</param>
        public FirefoxDriver(Uri remoteServerAddress, SessionModel session, TimeSpan timeout)
            : this(new WebDriverCommandInvoker(remoteServerAddress, timeout), session)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FirefoxDriver"/> class using the specified WebDriver command invoker and session.
        /// </summary>
        /// <param name="invoker">The WebDriver command invoker for the driver.</param>
        /// <param name="session">The session model associated with the WebDriver driver.</param>
        public FirefoxDriver(IWebDriverCommandInvoker invoker, SessionModel session)
            : base(invoker, session)
        {
            // Additional initialization logic can be added here if needed.
        }
        #endregion

        #region *** Methods      ***
        /// <inheritdoc />
        protected override IEnumerable<(string Name, WebDriverCommandModel Command)> GetCustomCommands()
        {
            // Helper method to create a new WebDriverCommandModel with the specified HTTP method and route.
            static WebDriverCommandModel GetCommand(HttpMethod method, string route) => new()
            {
                Method = method,
                Route = route
            };

            // Return a list of custom commands, each with a unique name and corresponding route.
            return
            [
                ("SetContextCommand", GetCommand(HttpMethod.Post, "/session/$[session]/moz/context")),
                ("GetContextCommand", GetCommand(HttpMethod.Get, "/session/$[session]/moz/context")),
                ("InstallAddOnCommand", GetCommand(HttpMethod.Post, "/session/$[session]/moz/addon/install")),
                ("UninstallAddOnCommand", GetCommand(HttpMethod.Post, "/session/$[session]/moz/addon/uninstall")),
                ("GetFullPageScreenshotCommand", GetCommand(HttpMethod.Get, "/session/$[session]/moz/screenshot/full")),
            ];
        }
        #endregion
    }
}
