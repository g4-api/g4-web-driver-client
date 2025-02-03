using G4.WebDriver.Models;

using System;
using System.Diagnostics;

namespace G4.WebDriver.Remote
{
    /// <summary>
    /// Provides a way to manage the lifecycle of a server that understands remote commands.
    /// </summary>
    public interface IWebDriverService : IDisposable
    {
        #region *** Properties ***
        /// <summary>
        /// Gets or sets a value indicating whether the command prompt window of the service should be hidden.
        /// </summary>
        bool HideCommandPromptWindow { get; set; }

        /// <summary>
        /// The <see cref="System.Diagnostics.Process"/> object that holds the server.
        /// </summary>
        Process Process { get; }

        /// <summary>
        /// Gets a value indicating whether the service is running and responding to HTTP requests.
        /// </summary>
        bool Ready { get; }

        /// <summary>
        /// The server address including the port.
        /// </summary>
        Uri ServerAddress { get; set; }

        /// <summary>
        /// Gets the server status.
        /// </summary>
        WebDriverStatusModel Status { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the initial diagnostic information is suppressed
        /// when starting the driver server executable. Defaults to <see cref="false"/>, meaning
        /// diagnostic information should be shown by the driver server executable.
        /// </summary>
        bool SuppressDiagnostic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the time to wait for an initial connection before timing out.
        /// </summary>
        TimeSpan Timeout { get; set; }
        #endregion

        #region *** Methods    ***
        /// <summary>
        /// Download a WebDriver from the provided URL.
        /// </summary>
        void DeployWebDriver();

        /// <summary>
        /// Starts the server.
        /// </summary>
        void StartService();

        /// <summary>
        /// Stops the server.
        /// </summary>
        void StopService();
        #endregion
    }
}
