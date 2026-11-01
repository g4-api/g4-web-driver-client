using G4.WebDriver.Exceptions;
using G4.WebDriver.Extensions;
using G4.WebDriver.Models;
using G4.WebDriver.Remote.Sap.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;

namespace G4.WebDriver.Remote.Sap
{
    /// <summary>
    /// Represents a WebDriver client for the SAP GUI WebDriver server, extending <see cref="RemoteWebDriver"/> with
    /// the SAP-specific session commands.
    /// </summary>
    /// <remarks>
    /// Creating the driver opens a WebDriver session that the server attaches to one running SAP GUI session, chosen
    /// by <see cref="SapOptions"/>; the server never starts SAP GUI or logs on, and SAP GUI Scripting has to be
    /// enabled on the client and the SAP server beforehand. The standard W3C commands come from the base driver;
    /// element lookups return <see cref="SapElement"/> instances. The SAP server does not support raw User32 input,
    /// so this driver offers none.
    /// </remarks>
    public class SapDriver : RemoteWebDriver, ISapDriver
    {
        #region *** Constructors ***
        /// <summary>
        /// Initializes a new instance of the <see cref="SapDriver"/> class that starts Sap.DriverServer.exe from the
        /// current directory and attaches to the only open SAP GUI session.
        /// </summary>
        public SapDriver()
            : this(options: new SapOptions())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SapDriver"/> class that starts Sap.DriverServer.exe from the
        /// current directory with the specified options.
        /// </summary>
        /// <param name="options">The options that select the SAP GUI session.</param>
        public SapDriver(SapOptions options)
            : this(session: options.ConvertToCapabilities().NewSessionModel())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SapDriver"/> class that starts Sap.DriverServer.exe from the
        /// current directory with the specified session model and a one-minute command timeout.
        /// </summary>
        /// <param name="session">The session model sent to the server.</param>
        public SapDriver(SessionModel session)
            : this(driverService: new SapWebDriverService(), session, timeout: TimeSpan.FromMinutes(1))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SapDriver"/> class with the specified driver service, default
        /// options, and a one-minute command timeout.
        /// </summary>
        /// <param name="driverService">The service that starts and owns the server process.</param>
        public SapDriver(SapWebDriverService driverService)
            : this(driverService, options: new SapOptions(), timeout: TimeSpan.FromMinutes(1))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SapDriver"/> class that starts Sap.DriverServer.exe from the
        /// specified directory with default options.
        /// </summary>
        /// <param name="binariesPath">The directory that contains Sap.DriverServer.exe.</param>
        public SapDriver(string binariesPath)
            : this(binariesPath, options: new SapOptions())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SapDriver"/> class with the specified remote server address,
        /// default options, and a one-minute command timeout.
        /// </summary>
        /// <param name="remoteServerAddress">The address of a running SAP driver server or grid.</param>
        public SapDriver(Uri remoteServerAddress)
            : this(remoteServerAddress, options: new SapOptions())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SapDriver"/> class with the specified command invoker and
        /// session model.
        /// </summary>
        /// <param name="invoker">The command invoker that sends every request.</param>
        /// <param name="session">The session model; <c>StartNewSession</c> false reuses an existing session.</param>
        public SapDriver(IWebDriverCommandInvoker invoker, SessionModel session)
            : base(invoker, session)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SapDriver"/> class that starts Sap.DriverServer.exe from the
        /// specified directory with the specified options.
        /// </summary>
        /// <param name="binariesPath">The directory that contains Sap.DriverServer.exe.</param>
        /// <param name="options">The options that select the SAP GUI session.</param>
        public SapDriver(string binariesPath, SapOptions options)
            : this(binariesPath, session: options.ConvertToCapabilities().NewSessionModel())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SapDriver"/> class that starts Sap.DriverServer.exe from the
        /// specified directory with the specified session model and a one-minute command timeout.
        /// </summary>
        /// <param name="binariesPath">The directory that contains Sap.DriverServer.exe.</param>
        /// <param name="session">The session model sent to the server.</param>
        public SapDriver(string binariesPath, SessionModel session)
            : this(driverService: new SapWebDriverService(binariesPath), session, timeout: TimeSpan.FromMinutes(1))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SapDriver"/> class with the specified remote server address,
        /// options, and a one-minute command timeout.
        /// </summary>
        /// <param name="remoteServerAddress">The address of a running SAP driver server or grid.</param>
        /// <param name="options">The options that select the SAP GUI session.</param>
        public SapDriver(Uri remoteServerAddress, SapOptions options)
            : this(remoteServerAddress, options, timeout: TimeSpan.FromMinutes(1))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SapDriver"/> class with the specified driver service, options,
        /// and command timeout.
        /// </summary>
        /// <param name="driverService">The service that starts and owns the server process.</param>
        /// <param name="options">The options that select the SAP GUI session.</param>
        /// <param name="timeout">The time within which the server has to answer each command.</param>
        public SapDriver(SapWebDriverService driverService, SapOptions options, TimeSpan timeout)
            : this(driverService, session: options.ConvertToCapabilities().NewSessionModel(), timeout)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SapDriver"/> class with the specified driver service, session
        /// model, and command timeout.
        /// </summary>
        /// <param name="driverService">The service that starts and owns the server process.</param>
        /// <param name="session">The session model sent to the server.</param>
        /// <param name="timeout">The time within which the server has to answer each command.</param>
        public SapDriver(SapWebDriverService driverService, SessionModel session, TimeSpan timeout)
            : base(new WebDriverCommandInvoker(driverService, timeout), session)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SapDriver"/> class with the specified remote server address,
        /// options, and command timeout.
        /// </summary>
        /// <param name="remoteServerAddress">The address of a running SAP driver server or grid.</param>
        /// <param name="options">The options that select the SAP GUI session.</param>
        /// <param name="timeout">The time within which the server has to answer each command.</param>
        public SapDriver(Uri remoteServerAddress, SapOptions options, TimeSpan timeout)
            : this(remoteServerAddress, session: options.ConvertToCapabilities().NewSessionModel(), timeout)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SapDriver"/> class with the specified remote server address,
        /// session model, and command timeout.
        /// </summary>
        /// <param name="remoteServerAddress">The address of a running SAP driver server or grid.</param>
        /// <param name="session">The session model sent to the server.</param>
        /// <param name="timeout">The time within which the server has to answer each command.</param>
        public SapDriver(Uri remoteServerAddress, SessionModel session, TimeSpan timeout)
            : this(invoker: new WebDriverCommandInvoker(remoteServerAddress, timeout), session)
        { }
        #endregion

        #region *** Methods      ***
        /// <summary>
        /// Finds the first element that matches the locator and returns it as a <see cref="SapElement"/>.
        /// </summary>
        /// <param name="by">The locator of the element.</param>
        /// <returns>The matching element.</returns>
        new public IWebElement FindElement(By by)
        {
            // Locate the element through the standard command, then wrap it to expose the SAP members.
            var element = base.FindElement(by);

            return new SapElement(driver: this, id: element.Id);
        }

        /// <summary>
        /// Finds every element that matches the locator and returns them as <see cref="SapElement"/> instances.
        /// </summary>
        /// <param name="by">The locator of the elements.</param>
        /// <returns>A read-only collection of the matching elements; empty when nothing matches.</returns>
        new public IEnumerable<IWebElement> FindElements(By by)
        {
            // Locate the elements through the standard command and wrap each one to expose the SAP members.
            var elements = base
                .FindElements(by)
                .Select(element => (IWebElement)new SapElement(driver: this, id: element.Id))
                .ToList();

            return new ReadOnlyCollection<IWebElement>(elements);
        }

        /// <inheritdoc />
        public string GetDocument()
        {
            // Build an invocation-owned command so the session placeholder never changes the registered template.
            var command = Invoker.NewCommand(commandName: "GetSapDocument");
            command.Session = Session.OpaqueKey;

            // Read the XML body from the raw response, because the server writes the document outside the JSON value.
            var response = Invoker.Invoke(command);

            return ReadDocument(response);
        }

        /// <inheritdoc />
        public string GetDocument(By by)
        {
            // Require the locator before sending a request whose body is built from it.
            ArgumentNullException.ThrowIfNull(argument: by);

            // Build an invocation-owned command carrying the W3C locator of the element whose subtree is returned.
            var command = Invoker.NewCommand(commandName: "GetSapElementDocument");
            command.Session = Session.OpaqueKey;
            command.Data = new
            {
                by.Using,
                by.Value
            };

            // Read the XML body from the raw response; a locator miss surfaces as a no-such-element exception.
            var response = Invoker.Invoke(command);

            return ReadDocument(response);
        }

        /// <inheritdoc />
        public SapStatusBarModel GetStatusBar()
        {
            // Build an invocation-owned command so the session placeholder never changes the registered template.
            var command = Invoker.NewCommand(commandName: "GetSapStatusBar");
            command.Session = Session.OpaqueKey;

            // Read the status bar and map the JSON value to the typed model.
            var response = Invoker.Invoke(command);

            return response.New<SapStatusBarModel>();
        }

        /// <inheritdoc />
        public void SendVirtualKey(int virtualKey)
        {
            // Target the current window by leaving the window name out.
            SendVirtualKey(virtualKey, window: null);
        }

        /// <inheritdoc />
        public void SendVirtualKey(int virtualKey, string window)
        {
            // Build an invocation-owned command so the key payload never changes the registered template.
            var command = Invoker.NewCommand(commandName: "SendSapVirtualKey");
            command.Session = Session.OpaqueKey;
            command.Data = new SapVirtualKeyModel
            {
                VirtualKey = virtualKey,
                Window = window
            };

            // Send the key; a missing window surfaces as a WebDriver exception.
            Invoker.Invoke(command);
        }

        /// <inheritdoc />
        protected override IEnumerable<(string Name, WebDriverCommandModel Command)> GetCustomCommands()
        {
            // Name the element routes once so the registry lines stay readable.
            const string ElementDocumentRoute = "/session/$[session]/element/dom";
            const string ElementMethodRoute = "/session/$[session]/sap/element/$[element]/call";
            const string ElementPropertyRoute = "/session/$[session]/sap/element/$[element]/property";

            // Register each SAP route once; every call copies its template, so the routes stay reusable.
            return
            [
                // Element
                ("GetSapElementDocument", NewCommand(HttpMethod.Post, route: ElementDocumentRoute)),
                ("InvokeSapElementMethod", NewCommand(HttpMethod.Post, route: ElementMethodRoute)),
                ("SetSapElementProperty", NewCommand(HttpMethod.Post, route: ElementPropertyRoute)),
                // Session
                ("GetSapDocument", NewCommand(HttpMethod.Get, route: "/session/$[session]/dom")),
                ("GetSapStatusBar", NewCommand(HttpMethod.Get, route: "/session/$[session]/sap/statusbar")),
                ("SendSapVirtualKey", NewCommand(HttpMethod.Post, route: "/session/$[session]/sap/vkey"))
            ];

            // Creates one command template for the registry; the template is copied before every invocation.
            static WebDriverCommandModel NewCommand(HttpMethod method, string route) => new()
            {
                Method = method,
                Route = route
            };
        }

        // Reads the XML document from a successful response. The server writes the document as raw XML, so the body
        // comes from the transport response instead of the JSON value; JSON protocol errors were already raised by
        // the invoker. A failed status with an XML error document is raised here as a WebDriver exception.
        private static string ReadDocument(WebDriverResponseModel response)
        {
            // Treat a missing transport response as an empty document, which only an empty server reply produces.
            var transportResponse = response.WebDriverResponse;
            if (transportResponse is null)
            {
                return string.Empty;
            }

            // Read the raw body once; it holds either the document or the server's XML error document.
            var document = transportResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            // Report a failed status with the server's error document so the caller sees why it failed.
            if (!transportResponse.IsSuccessStatusCode)
            {
                var statusCode = (int)transportResponse.StatusCode;
                var message = $"The SAP object model document request failed with status {statusCode}: {document}";
                throw new WebDriverException(message);
            }

            return document;
        }
        #endregion
    }
}
