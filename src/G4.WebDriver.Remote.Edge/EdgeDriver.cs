/*
 * RESOURCES
 * https://github.com/SeleniumHQ/selenium/blob/trunk/dotnet/src/webdriver/Chromium/ChromiumDriver.cs#L233
 */
using G4.WebDriver.Extensions;
using G4.WebDriver.Models;
using G4.WebDriver.Remote.Chromium;

using System;
using System.Collections.Generic;
using System.Net.Http;

namespace G4.WebDriver.Remote.Edge
{
    /// <summary>
    /// Represents a WebDriver implementation for Microsoft Edge, extending the ChromiumDriverBase.
    /// </summary>
    public class EdgeDriver : ChromiumDriverBase
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the EdgeDriver class.
        /// </summary>
        public EdgeDriver()
            : this(new EdgeOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the EdgeDriver class using the specified options.
        /// </summary>
        /// <param name="options">The EdgeOptions to be used with the Edge driver.</param>
        public EdgeDriver(EdgeOptions options)
            : this(options.ConvertToCapabilities().NewSessionModel())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the EdgeDriver class using the specified options.
        /// </summary>
        /// <param name="session">The SessionModel to be used with the Edge driver.</param>
        public EdgeDriver(SessionModel session)
            : this(new EdgeWebDriverService(), session, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the EdgeDriver class using the specified driver service.
        /// </summary>
        /// <param name="driverService">The EdgeWebDriverService used to initialize the driver.</param>
        public EdgeDriver(EdgeWebDriverService driverService)
            : this(driverService, new EdgeOptions(), TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the EdgeDriver class using the specified path
        /// to the directory containing msedgedriver.exe.
        /// </summary>
        /// <param name="binariesPath">The full path to the directory containing msedgedriver.exe.</param>
        public EdgeDriver(string binariesPath)
            : this(binariesPath, new EdgeOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the EdgeDriver class using the specified path
        /// to the directory containing msedgedriver.exe.
        /// </summary>
        /// <param name="binariesPath">The full path to the directory containing msedgedriver.exe.</param>
        /// <param name="options">The EdgeOptions to be used with the Edge driver.</param>
        public EdgeDriver(string binariesPath, EdgeOptions options)
            : this(binariesPath, options.ConvertToCapabilities().NewSessionModel())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the EdgeDriver class using the specified path
        /// to the directory containing msedgedriver.exe.
        /// </summary>
        /// <param name="binariesPath">The full path to the directory containing msedgedriver.exe.</param>
        /// <param name="session">The SessionModel to be used with the Edge driver.</param>
        public EdgeDriver(string binariesPath, SessionModel session)
            : this(new EdgeWebDriverService(binariesPath), session, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the EdgeDriver class using the specified path
        /// to the directory containing msedgedriver.exe.
        /// </summary>
        /// <param name="driverService">The EdgeWebDriverService used to initialize the driver.</param>
        /// <param name="options">The EdgeOptions to be used with the Edge driver.</param>
        /// <param name="timeout">The timeout within which the server must respond.</param>
        public EdgeDriver(EdgeWebDriverService driverService, EdgeOptions options, TimeSpan timeout)
            : this(driverService, options.ConvertToCapabilities().NewSessionModel(), timeout)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the EdgeDriver class using the specified path
        /// to the directory containing msedgedriver.exe.
        /// </summary>
        /// <param name="driverService">The EdgeWebDriverService used to initialize the driver.</param>
        /// <param name="session">The SessionModel to be used with the Edge driver.</param>
        /// <param name="timeout">The timeout within which the server must respond.</param>
        public EdgeDriver(EdgeWebDriverService driverService, SessionModel session, TimeSpan timeout)
            : base(new WebDriverCommandInvoker(driverService, timeout), session)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeDriver"/> class with the specified remote server address.
        /// Uses default <see cref="EdgeOptions"/> and timeout of 1 minute.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        public EdgeDriver(Uri remoteServerAddress)
            : this(remoteServerAddress, new EdgeOptions())
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeDriver"/> class with the specified remote server address and options.
        /// Uses default timeout of 1 minute.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        /// <param name="options">The options for the Edge driver.</param>
        public EdgeDriver(Uri remoteServerAddress, EdgeOptions options)
            : this(remoteServerAddress, options, TimeSpan.FromMinutes(1))
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeDriver"/> class with the specified remote server address, options, and timeout.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        /// <param name="options">The options for the Edge driver.</param>
        /// <param name="timeout">The command execution timeout.</param>
        public EdgeDriver(Uri remoteServerAddress, EdgeOptions options, TimeSpan timeout)
            : this(remoteServerAddress, options.ConvertToCapabilities().NewSessionModel(), timeout)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeDriver"/> class using the specified remote server address, session, and timeout.
        /// </summary>
        /// <param name="remoteServerAddress">The address of the remote WebDriver server.</param>
        /// <param name="session">The session model associated with the WebDriver driver.</param>
        /// <param name="timeout">The timeout duration for WebDriver commands.</param>
        public EdgeDriver(Uri remoteServerAddress, SessionModel session, TimeSpan timeout)
            : this(new WebDriverCommandInvoker(remoteServerAddress, timeout), session)
        {
            // Additional initialization logic can be added here if needed.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeDriver"/> class using the specified WebDriver command invoker and session.
        /// </summary>
        /// <param name="invoker">The WebDriver command invoker for the driver.</param>
        /// <param name="session">The session model associated with the WebDriver driver.</param>
        public EdgeDriver(IWebDriverCommandInvoker invoker, SessionModel session)
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
                ("InvokeCdp", GetCommand(HttpMethod.Post, "/session/$[session]/ms/cdp/execute")),
                ("GetSinks", GetCommand(HttpMethod.Get, "/session/$[session]/ms/cast/get_sinks")),
                ("SetSinkToUse", GetCommand(HttpMethod.Post, "/session/$[session]/ms/cast/set_sink_to_use")),
                ("StartTabMirroring", GetCommand(HttpMethod.Post, "/session/$[session]/ms/cast/start_tab_mirroring")),
                ("StartDesktopMirroring", GetCommand(HttpMethod.Post, "/session/$[session]/ms/cast/start_desktop_mirroring")),
                ("GetIssueMessage", GetCommand(HttpMethod.Get, "/session/$[session]/ms/cast/get_issue_message")),
                ("StopCasting", GetCommand(HttpMethod.Post, "/session/$[session]/ms/cast/stop_casting"))
            ];
        }
        #endregion
    }
}
