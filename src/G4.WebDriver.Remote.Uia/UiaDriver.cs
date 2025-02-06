using G4.WebDriver.Extensions;
using G4.WebDriver.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading;
using System.Collections.ObjectModel;
using System.Linq;

namespace G4.WebDriver.Remote.Uia
{
    /// <summary>
    /// Represents a WebDriver implementation for UI Automation (UIA), extending the <see cref="RemoteWebDriver"/>.
    /// </summary>
    public class UiaDriver : RemoteWebDriver, IUser32Driver
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

        #region *** Methods      ***
        /// <inheritdoc />
        protected override IEnumerable<(string Name, WebDriverCommandModel Command)> GetCustomCommands()
        {
            // Helper method to create a new WebDriverCommandModel with the specified HTTP method and route.
            static WebDriverCommandModel NewCommand(HttpMethod method, string route) => new()
            {
                Method = method,
                Route = route
            };

            // Return a list of custom commands, each with a unique name and corresponding route.
            return
            [
                // Element
                ("GetUser32Attribute", NewCommand(HttpMethod.Get, "/session/$[session]/element/$[element]/attribute/$[attributeName]")),
                ("MoveUser32MouseToElement", NewCommand(HttpMethod.Post, "/session/$[session]/user32/element/$[element]/mouse/move")),
                ("SendUser32ClickToElement", NewCommand(HttpMethod.Post, "/session/$[session]/user32/element/$[element]/click")),
                ("SendUser32DoubleClickToElement", NewCommand(HttpMethod.Post, "/session/$[session]/user32/element/$[element]/dclick")),
                ("SetUser32Focus", NewCommand(HttpMethod.Get, "/session/$[session]/user32/element/$[element]/focus")),
                // Session
                ("SendUser32Inputs", NewCommand(HttpMethod.Post, "/session/$[session]/user32/inputs")),
                ("SendUser32Keys", NewCommand(HttpMethod.Post, "/session/$[session]/user32/value"))
            ];
        }

        /// <inheritdoc />
        new public IWebElement FindElement(By by)
        {
            // Find the element using the base class method
            var element = base.FindElement(by);

            // Return a new UiaElement instance with the found element ID
            return new UiaElement(driver: this, id: element.Id);
        }

        /// <inheritdoc />
        new public IEnumerable<IWebElement> FindElements(By by)
        {
            // Find the elements using the base class method
            var elements = base
                .FindElements(by)
                .Select(i => (IWebElement)new UiaElement(driver: this, id: i.Id))
                .ToList();

            // Return a read-only collection of the found elements
            return new ReadOnlyCollection<IWebElement>(elements);
        }

        /// <inheritdoc />
        public void SendInputs(params string[] codes)
        {
            SendInputs(1, codes);
        }

        /// <inheritdoc />
        public void SendInputs(int repeat, params string[] codes)
        {
            // Prepare the WebDriver command for sending scan codes to the server for input simulation
            var command = Invoker.Commands["SendUser32Inputs"];
            command.Session = Session.OpaqueKey;
            command.Data = new ScanCodesInputModel
            {
                ScanCodes = codes
            };

            // Send the scan codes to the server for the specified number of repeats
            for (var i = 0; i < repeat; i++)
            {
                // Invoke the SendUser32Inputs WebDriver command
                Invoker.Invoke(command);

                // Wait for a short interval before sending the next input
                Thread.Sleep(100);
            }
        }

        /// <inheritdoc />
        public void SendKeys(string text)
        {
            SendKeys(repeat: 1, text);
        }

        /// <inheritdoc />
        public void SendKeys(string text, TimeSpan delay)
        {
            // Iterate through each character in the provided text.
            foreach (var character in text)
            {
                // Send the current character as a key input.
                // 'repeat: 1' indicates that the key is sent once.
                SendKeys(repeat: 1, text: $"{character}");

                // Wait for the specified delay before sending the next key.
                Thread.Sleep(delay);
            }
        }

        /// <inheritdoc />
        public void SendKeys(int repeat, string text)
        {
            // Prepare the WebDriver command for sending text input to the server for input simulation
            var command = Invoker.Commands["SendUser32Keys"];
            command.Session = Session.OpaqueKey;
            command.Data = new TextInputModel
            {
                Text = text
            };

            // Send the text to the server for the specified number of repeats
            for (var i = 0; i < repeat; i++)
            {
                // Invoke the SendUser32Keys WebDriver command
                Invoker.Invoke(command);

                // Wait for a short interval before sending the next input
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Model for sending scan codes as input.
        /// </summary>
        private sealed class ScanCodesInputModel
        {
            /// <summary>
            /// Gets or sets the collection of scan codes.
            /// </summary>
            /// <value>A collection of scan codes required for the operation.</value>
            [Required]
            public IEnumerable<string> ScanCodes { get; set; }
        }

        /// <summary>
        /// Model for sending text input.
        /// </summary>
        private sealed class TextInputModel
        {
            /// <summary>
            /// Gets or sets the text to be input.
            /// </summary>
            /// <value>The text to be input.</value>
            [Required]
            public string Text { get; set; }
        }
        #endregion
    }
}
