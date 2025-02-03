using G4.WebDriver.Extensions;
using G4.WebDriver.Models;
using G4.WebDriver.Remote.Chromium;

using System;
using System.Collections.Generic;
using System.Net.Http;

namespace G4.WebDriver.Remote.Chrome
{
    public class ChromeDriver : ChromiumDriverBase
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the ChromeDriver class.
        /// </summary>
        public ChromeDriver()
            : this(new ChromeOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the ChromeDriver class using the specified options.
        /// </summary>
        /// <param name="options">The ChromeOptions to be used with the Chrome driver.</param>
        public ChromeDriver(ChromeOptions options)
            : this(options.ConvertToCapabilities().NewSessionModel())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the ChromeDriver class using the specified options.
        /// </summary>
        /// <param name="session">The SessionModel to be used with the Chrome driver.</param>
        public ChromeDriver(SessionModel session)
            : this(new ChromeWebDriverService(), session, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the ChromeDriver class using the specified driver service.
        /// </summary>
        /// <param name="driverService">The ChromeWebDriverService used to initialize the driver.</param>
        public ChromeDriver(ChromeWebDriverService driverService)
            : this(driverService, new ChromeOptions(), TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the ChromeDriver class using the specified path
        /// to the directory containing msedgedriver.exe.
        /// </summary>
        /// <param name="binariesPath">The full path to the directory containing msedgedriver.exe.</param>
        public ChromeDriver(string binariesPath)
            : this(binariesPath, new ChromeOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the ChromeDriver class using the specified path
        /// to the directory containing msedgedriver.exe.
        /// </summary>
        /// <param name="binariesPath">The full path to the directory containing msedgedriver.exe.</param>
        /// <param name="options">The ChromeOptions to be used with the Chrome driver.</param>
        public ChromeDriver(string binariesPath, ChromeOptions options)
            : this(binariesPath, options.ConvertToCapabilities().NewSessionModel())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the ChromeDriver class using the specified path
        /// to the directory containing msedgedriver.exe.
        /// </summary>
        /// <param name="binariesPath">The full path to the directory containing msedgedriver.exe.</param>
        /// <param name="session">The SessionModel to be used with the Chrome driver.</param>
        public ChromeDriver(string binariesPath, SessionModel session)
            : this(new ChromeWebDriverService(binariesPath), session, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the ChromeDriver class using the specified path
        /// to the directory containing msedgedriver.exe.
        /// </summary>
        /// <param name="driverService">The ChromeWebDriverService used to initialize the driver.</param>
        /// <param name="options">The ChromeOptions to be used with the Chrome driver.</param>
        /// <param name="timeout">The timeout within which the server must respond.</param>
        public ChromeDriver(ChromeWebDriverService driverService, ChromeOptions options, TimeSpan timeout)
            : this(driverService, options.ConvertToCapabilities().NewSessionModel(), timeout)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the ChromeDriver class using the specified path
        /// to the directory containing msedgedriver.exe.
        /// </summary>
        /// <param name="driverService">The ChromeWebDriverService used to initialize the driver.</param>
        /// <param name="session">The SessionModel to be used with the Chrome driver.</param>
        /// <param name="timeout">The timeout within which the server must respond.</param>
        public ChromeDriver(ChromeWebDriverService driverService, SessionModel session, TimeSpan timeout)
            : base(new WebDriverCommandInvoker(driverService, timeout), session)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromeDriver"/> class with the specified remote server address.
        /// Uses default <see cref="ChromeOptions"/> and timeout of 1 minute.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        public ChromeDriver(Uri remoteServerAddress)
            : this(remoteServerAddress, new ChromeOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromeDriver"/> class with the specified remote server address and options.
        /// Uses default timeout of 1 minute.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        /// <param name="options">The options for the Chrome driver.</param>
        public ChromeDriver(Uri remoteServerAddress, ChromeOptions options)
            : this(remoteServerAddress, options, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromeDriver"/> class with the specified remote server address, options, and timeout.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        /// <param name="options">The options for the Chrome driver.</param>
        /// <param name="timeout">The command execution timeout.</param>
        public ChromeDriver(Uri remoteServerAddress, ChromeOptions options, TimeSpan timeout)
            : this(remoteServerAddress, options.ConvertToCapabilities().NewSessionModel(), timeout)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromeDriver"/> class using the specified remote server address, session, and timeout.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        /// <param name="session">The session model associated with the WebDriver driver.</param>
        /// <param name="timeout">The timeout duration for WebDriver commands.</param>
        public ChromeDriver(Uri remoteServerAddress, SessionModel session, TimeSpan timeout)
            : this(new WebDriverCommandInvoker(remoteServerAddress, timeout), session)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChromeDriver"/> class using the specified WebDriver command invoker and session.
        /// </summary>
        /// <param name="invoker">The WebDriver command invoker for the driver.</param>
        /// <param name="session">The session model associated with the WebDriver driver.</param>
        public ChromeDriver(IWebDriverCommandInvoker invoker, SessionModel session)
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
                ("InvokeCdp", GetCommand(HttpMethod.Post, "/session/$[session]/goog/cdp/execute")),
                ("GetSinks", GetCommand(HttpMethod.Get, "/session/$[session]/goog/cast/get_sinks")),
                ("SetSinkToUse", GetCommand(HttpMethod.Post, "/session/$[session]/goog/cast/set_sink_to_use")),
                ("StartTabMirroring", GetCommand(HttpMethod.Post, "/session/$[session]/goog/cast/start_tab_mirroring")),
                ("GetIssueMessage", GetCommand(HttpMethod.Get, "/session/$[session]/goog/cast/get_issue_message")),
                ("StopCasting", GetCommand(HttpMethod.Post, "/session/$[session]/goog/cast/stop_casting"))
            ];
        }
        #endregion
    }
}
