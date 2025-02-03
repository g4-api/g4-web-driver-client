using G4.WebDriver.Models;

using System;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Represents a remote WebDriver that allows communication with a remote WebDriver server.
    /// </summary>
    /// <param name="invoker">The command invoker used for executing WebDriver commands.</param>
    /// <param name="session">The session model representing the browser session.</param>
    public class RemoteWebDriver(IWebDriverCommandInvoker invoker, SessionModel session) : WebDriverBase(invoker, session)
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteWebDriver"/> class with the specified server address and session.
        /// </summary>
        /// <param name="serverAddress">The address of the remote WebDriver server as a string.</param>
        /// <param name="session">The session model representing the browser session.</param>
        public RemoteWebDriver(string serverAddress, SessionModel session)
            : this(new WebDriverCommandInvoker(new Uri(serverAddress)), session)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteWebDriver"/> class with the specified server address and session.
        /// </summary>
        /// <param name="serverAddress">The address of the remote WebDriver server as a Uri.</param>
        /// <param name="session">The session model representing the browser session.</param>
        public RemoteWebDriver(Uri serverAddress, SessionModel session)
            : this(new WebDriverCommandInvoker(serverAddress), session)
        { }
    }
}
